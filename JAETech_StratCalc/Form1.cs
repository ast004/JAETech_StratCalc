using Act;
using DataExchangeAPI;
using SumoLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace JAETech_StratCalc
{
    public partial class JAETech_StratCalc : Form
    {
        string user;
        bool closing = false;

        public JAETech_StratCalc()
        {
            InitializeComponent();
        }
        private void JAETech_StratCalc_Load(object sender, EventArgs e)
        {
            preLogin();

            if (Properties.Settings.Default.Size.Height > 0)
            {
                this.DesktopBounds = new Rectangle(Properties.Settings.Default.Location, Properties.Settings.Default.Size);
                this.Location = Properties.Settings.Default.Location;
            }

            user = Properties.Settings.Default.Username;
            string pass = Properties.Settings.Default.Password;
            UserLoginSettings login = new UserLoginSettings(user, pass, true);

            if (login.Logged_In)
            {
                postLogin();
                LoginPanel.Height = 0;
            }
            else
            {
                LoginStatusLabel.Text = "No Saved Password";
                LoginPanel.Height = this.Height;
            }
        }
        private void SubmitAccountInfo_KeyPress(object sender, KeyPressEventArgs e)
        {
            LoginStatusLabel.Text = "";
            if (e.KeyChar == 13)
            {
                user = Username.Text;
                string pass = RegularPassword.Text;
                UserLoginSettings login = new UserLoginSettings(user, pass, true);

                if (login.Logged_In)
                {
                    if (!login.Used_Master_Override)
                    {
                        Properties.Settings.Default.Username = user;
                        Properties.Settings.Default.Password = pass;
                        Properties.Settings.Default.Save();
                    }

                    postLogin();
                    LoginPanel.Height = 0;
                }
                else
                {
                    LoginStatusLabel.Text = login.Connection_Error;
                }
            }
        }
        private void JAETech_StratCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing = true;

            System.Drawing.Rectangle bounds = this.WindowState != FormWindowState.Normal ? this.RestoreBounds : this.DesktopBounds;
            Properties.Settings.Default.Location = bounds.Location;
            Properties.Settings.Default.Size = bounds.Size;
            Properties.Settings.Default.Save();
        }


        Dictionary<string, DataRow> connectionRows = new Dictionary<string, DataRow>();
        DataTable connectionsDT;

        stratServerConnection connection;

        #region initialize
        public void preLogin()
        {
            setupConnectionsDGV();
        }
        public void postLogin()
        {

        }

        public void setupConnectionsDGV()
        {
            connectionsDT = new DataTable();
            connectionsDT.Columns.Add("IP");
            connectionsDT.Columns.Add("Status");
            connectionsDT.Columns.Add("Tables Received");
            connectionsDT.Columns.Add("Last Update");

            connectionsDGV.DataSource = connectionsDT;
        }
        #endregion initialize

        private void submitIPButton_Click(object sender, EventArgs e)
        {
            string ip = ipTextbox.Text;
            connection = new stratServerConnection(ip);

            if (connection.initialize())
            {
                MessageBox.Show("Connected!");

            }
            else
            {
                MessageBox.Show("ERROR!");
            }
        }
    }

    public class stratServerConnection : IQueryResponseHandler, IStatusListener
    {
        public event optionTheoUpdated updateOption;

        private ActService m_actService = null;
        private ActSession m_actSession = null;
        const double PRICE_DENOMINATOR = 10000000;

        Dictionary<int, Dictionary<int, VariantType>> m_columnNameToType = new Dictionary<int, Dictionary<int, VariantType>>();
        List<KeyValuePair<int, IList<ColumnDescriptor>>> Column_Descriptor_List = new List<KeyValuePair<int, IList<ColumnDescriptor>>>();

        public string ip { get; private set; }
        public string optionMnemonic { get; private set; }
        public bool connected = false;

        BlockingCollection<Tuple<int, TableUpdate>> block = new BlockingCollection<Tuple<int, TableUpdate>>();
        string[] attributes;

        public stratServerConnection(string ip)
        {
            this.ip = ip;
            optionMnemonic = Utility.getOptionMnemonicFromIP(ip);
            attributes = (new stratOptionAttributes()).attDict.Keys.ToArray();

            Thread consumer = new Thread(new ThreadStart(processTableUpdates));
            consumer.IsBackground = true;
            consumer.Start();
        }
        public bool initialize()
        {
            #region connect
            if (!connected)
            {
                Thread ConnectionThread = new Thread(new ThreadStart(ConnectThread));
                ConnectionThread.IsBackground = true;
                ConnectionThread.Start();

                int ct = 0;
                while (!connected && ct < 20) { Thread.Sleep(100); ct++; }
                if (ct == 19) return false;
            }
            #endregion connect

            return connected;
        }

        int currentID = -1;
        bool recOptions = false;
        ConcurrentDictionary<string, stratOption> options = new ConcurrentDictionary<string, stratOption>();

        public ConcurrentDictionary<string, stratOption> getOptions(string symbol)
        {
            if (currentID > 0)
                m_actSession.DexSubSession.CloseQuery(currentID);

            options = new ConcurrentDictionary<string, stratOption>();
            recOptions = false;
            currentID = -1;
            int ct;

            symbol = optionMnemonic + "." + symbol.ToUpper() + ".O";
            if (initialize())
            {
                currentID = m_actSession.DexSubSession.RegisterQuery(this);
                if (currentID > 0)
                {
                    if (!m_actSession.DexSubSession.OpenQuery(currentID, new string[] { symbol }, attributes, new string[] { }, new string[] { }, 1000, false))
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            ct = 0;
            while (!recOptions && ct < 20) { Thread.Sleep(100); ct++; }
            if (ct == 19) return null;

            return options;
        }

        #region ACT
        public void processTableUpdates()
        {
            while (true)
            {
                foreach (var tu in block.GetConsumingEnumerable())
                {
                    if (tu.Item1 == currentID || true)
                    {
                        updateOptionValues(tu.Item1, tu.Item2);
                    }
                    else
                    {
                        try
                        {
                            m_actSession.DexSubSession.CloseQuery(tu.Item1);
                        }
                        catch { }
                    }
                }

                recOptions = true;
            }
        }
        public void updateOptionValues(int clientId, DataExchangeAPI.TableUpdate tableUpdate)
        {
            IList<ColumnDescriptor> c_descrip_list;
            if (tableUpdate.ColumnDescriptorCount > 0 && !m_columnNameToType.ContainsKey(clientId))
            {
                m_columnNameToType.Add(clientId, new Dictionary<int, VariantType>());
                for (int i = 0; i < tableUpdate.ColumnDescriptorCount; i++)
                {
                    VariantType type = tableUpdate.ColumnDescriptorList[i].Type;
                    m_columnNameToType[clientId][i] = type;
                }

                Column_Descriptor_List.Add(new KeyValuePair<int, IList<ColumnDescriptor>>(clientId, tableUpdate.ColumnDescriptorList));
                c_descrip_list = tableUpdate.ColumnDescriptorList;
            }
            else
            {
                c_descrip_list = Column_Descriptor_List.Find(n => n.Key == clientId).Value;
            }

            for (int itx = 0; itx < tableUpdate.RowCount; itx++)
            {
                Row row = tableUpdate.RowList[itx];
                string id = row.Key;

                if (!options.ContainsKey(id))
                {
                    stratOption opt = new stratOption(id);
                    while (!options.TryAdd(id, opt)) { Thread.Sleep(100); Console.WriteLine("Cant add: " + id); }
                }
                stratOption o = options[id];

                foreach (Cell cell in row.CellList)
                {
                    #region Update Rows
                    int columnIndex = cell.ColumnNumber;

                    if (m_columnNameToType.ContainsKey(clientId))
                    {
                        if (m_columnNameToType[clientId].ContainsKey(columnIndex))
                        {
                            VariantType type = m_columnNameToType[clientId][columnIndex];

                            if (cell.HasValue)
                            {
                                VariantValue value = cell.Value;

                                switch (type)
                                {
                                    case VariantType.VAR_DOUBLE:
                                        {
                                            if (value.HasVarDouble)
                                            {
                                                o.UpdateRow(c_descrip_list[columnIndex].Name, (float)value.VarDouble);
                                            }
                                            break;
                                        }
                                    case VariantType.VAR_INT32:
                                        {
                                            if (value.HasVarInt)
                                            {
                                                o.UpdateRow(c_descrip_list[columnIndex].Name, value.VarInt);
                                            }

                                            break;
                                        }
                                    case VariantType.VAR_PRICE:
                                        {
                                            if (value.HasVarPrice)
                                            {
                                                double val = Math.Round(value.VarPrice / PRICE_DENOMINATOR, 5);
                                                o.UpdateRow(c_descrip_list[columnIndex].Name, (float)val);
                                            }
                                            break;
                                        }

                                    case VariantType.VAR_STRING:
                                        {
                                            if (value.HasVarString)
                                            {
                                                o.UpdateRow(c_descrip_list[columnIndex].Name, value.VarString);
                                            }
                                            break;
                                        }
                                    case VariantType.VAR_UNKNOWN:
                                        {
                                            System.Diagnostics.Debug.Assert(false);
                                            break;
                                        }
                                }
                            }
                        }
                    }
                    #endregion Update Rows
                }
            }
        }
        public void OnQueryTableUpdate(int clientId, DataExchangeAPI.OperationStatus operationStatus, DataExchangeAPI.TableUpdate tableUpdate)
        {
            block.Add(new Tuple<int, TableUpdate>(clientId, tableUpdate));
        }

        #region UselessACT
        public void OnStartQueryResponse(int clientId, DataExchangeAPI.OperationStatus operationStatus)
        {
            Console.WriteLine("Query started: " + clientId);
        }
        public void OnStopQueryResponse(int clientId, DataExchangeAPI.OperationStatus operationStatus)
        {
            Console.WriteLine("Query stopped: " + clientId);
        }
        public void OnTableUpdateResponse(int clientId, DataExchangeAPI.OperationStatus operationStatus)
        {
        }
        public void OnQueryClosed(int clientId)
        {
            Console.WriteLine("Query closed: " + clientId);
        }
        public void OnConnectionStatus(bool connected)
        {
        }
        public void LoginStatusChanged(ActSession session)
        {
        }
        public void SessionStatusChanged(ActSession session)
        {
            if (session != m_actSession)
            {
                Console.WriteLine("session != actSession");
                return;
            }

            connected = session.IsConnected();
            Console.WriteLine("Session state: " + (connected ? "connected" : "disconnected"));
        }
        #endregion UselessACT
        #endregion ACT

        #region Connecting
        private void ConnectThread()
        {
            try
            {
                Connect(ip);
                Dispatcher.Run();
            }
            catch { Close(); }
        }
        private bool IsConnected()
        {
            return connected;
        }
        private void Connect(string ip)
        {
            if (IsConnected())
                return;

            if (m_actService == null)
                m_actService = new ActService(Dispatcher.CurrentDispatcher);
            else
            {
                Close();        //	don't keep failed connections
            }


            ConnectionSettings connection = new ConnectionSettings();
            connection.Dex = ip + ":" + 4722;

            m_actService.Connect(new List<ConnectionSettings>() { connection }, "", "");    //TODO: add username and password
            if (m_actService.GetAllActSessions().Count > 0)
            {
                m_actSession = m_actService.GetAllActSessions()[0] as ActSession;
                m_actSession.SubscribeToStatus(this);
            }
        }

        public void Close()
        {
            if (m_actService != null)
                m_actService.DisconnectAll();

            connected = false;
        }
        #endregion Connecting
    }

    public delegate void optionTheoUpdated(string id);    
    public class stratOption
    {
        public string id { get; private set; }
        public string symbol { get; private set; }
        public DateTime expiration { get; private set; }
        public string actantExpiration { get; private set; }
        public double strike { get; private set; }
        public char itype { get; private set; }

        public stratOption(string id)
        {
            this.id = id;
            var split = id.Split('.');

            symbol = split[1];
            actantExpiration = split[2];
            expiration = Utility.GetDateTimeFromActantExpiration(split[2]);
            itype = id[id.Length - 1];

            double s;
            if (double.TryParse(split[3].Replace(':', '.'), out s)) { strike = s; }
            else { Console.WriteLine("Can't parse strike for: " + id); }

            attDict = (new stratOptionAttributes()).attDict;
            data_row = new object[attDict.Count];
        }

        object[] data_row;
        Dictionary<string, int> attDict;
        public bool updated = false;

        public void UpdateRow(string header, object value)
        {
            if (attDict.ContainsKey(header))
            {
                data_row[attDict[header]] = value;

                updateOption(id);
                updated = true;
            }
            else
            {
                Console.WriteLine("Can't update: " + header);
            }
        }

        public double THEOR
        {
            get
            {
                double val;
                if (data_row[attDict["THEOR"]] != null)
                {
                    if (double.TryParse(data_row[attDict["THEOR"]].ToString(), out val))
                    {
                        return val;
                    }
                    else return 0;
                }
                else return 0;
            }
        }

        public event optionTheoUpdated updateOption;
    }
    public class stratOptionAttributes
    {
        string attList = "THEOR";
        public Dictionary<string, int> attDict = new Dictionary<string, int>();

        public stratOptionAttributes()
        {
            var split = attList.Split(',');
            for (int i = 0; i < split.Length; i++)
            {
                attDict.Add(split[i], i);
            }
        }
    }

}
