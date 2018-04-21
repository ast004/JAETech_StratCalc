namespace JAETech_StratCalc
{
    partial class JAETech_StratCalc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LoginPanel = new System.Windows.Forms.Panel();
            this.LoginStatusLabel = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.RegularPassword = new System.Windows.Forms.TextBox();
            this.Username = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.connectionPanel = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.stratInputTP = new System.Windows.Forms.TabPage();
            this.settingsTP = new System.Windows.Forms.TabPage();
            this.connectionsDGV = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ipTextbox = new System.Windows.Forms.TextBox();
            this.submitIPButton = new System.Windows.Forms.Button();
            this.LoginPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.settingsTP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.connectionsDGV)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoginPanel
            // 
            this.LoginPanel.Controls.Add(this.LoginStatusLabel);
            this.LoginPanel.Controls.Add(this.label14);
            this.LoginPanel.Controls.Add(this.RegularPassword);
            this.LoginPanel.Controls.Add(this.Username);
            this.LoginPanel.Controls.Add(this.label5);
            this.LoginPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.LoginPanel.Location = new System.Drawing.Point(0, 0);
            this.LoginPanel.Name = "LoginPanel";
            this.LoginPanel.Size = new System.Drawing.Size(474, 63);
            this.LoginPanel.TabIndex = 23;
            // 
            // LoginStatusLabel
            // 
            this.LoginStatusLabel.AutoSize = true;
            this.LoginStatusLabel.Location = new System.Drawing.Point(240, 41);
            this.LoginStatusLabel.Name = "LoginStatusLabel";
            this.LoginStatusLabel.Size = new System.Drawing.Size(151, 13);
            this.LoginStatusLabel.TabIndex = 11;
            this.LoginStatusLabel.Text = "Incorrect Username/Password";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 21);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(55, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "Username";
            // 
            // RegularPassword
            // 
            this.RegularPassword.Location = new System.Drawing.Point(128, 37);
            this.RegularPassword.Name = "RegularPassword";
            this.RegularPassword.Size = new System.Drawing.Size(106, 20);
            this.RegularPassword.TabIndex = 9;
            this.RegularPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SubmitAccountInfo_KeyPress);
            // 
            // Username
            // 
            this.Username.Location = new System.Drawing.Point(16, 37);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(106, 20);
            this.Username.TabIndex = 7;
            this.Username.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SubmitAccountInfo_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(125, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Password";
            // 
            // connectionPanel
            // 
            this.connectionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.connectionPanel.Location = new System.Drawing.Point(0, 63);
            this.connectionPanel.Name = "connectionPanel";
            this.connectionPanel.Size = new System.Drawing.Size(474, 54);
            this.connectionPanel.TabIndex = 24;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.stratInputTP);
            this.tabControl1.Controls.Add(this.settingsTP);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 117);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(474, 507);
            this.tabControl1.TabIndex = 25;
            // 
            // stratInputTP
            // 
            this.stratInputTP.Location = new System.Drawing.Point(4, 22);
            this.stratInputTP.Name = "stratInputTP";
            this.stratInputTP.Padding = new System.Windows.Forms.Padding(3);
            this.stratInputTP.Size = new System.Drawing.Size(466, 481);
            this.stratInputTP.TabIndex = 0;
            this.stratInputTP.Text = "Strategy Input";
            this.stratInputTP.UseVisualStyleBackColor = true;
            // 
            // settingsTP
            // 
            this.settingsTP.Controls.Add(this.connectionsDGV);
            this.settingsTP.Controls.Add(this.panel1);
            this.settingsTP.Location = new System.Drawing.Point(4, 22);
            this.settingsTP.Name = "settingsTP";
            this.settingsTP.Padding = new System.Windows.Forms.Padding(3);
            this.settingsTP.Size = new System.Drawing.Size(466, 481);
            this.settingsTP.TabIndex = 1;
            this.settingsTP.Text = "Settings";
            this.settingsTP.UseVisualStyleBackColor = true;
            // 
            // connectionsDGV
            // 
            this.connectionsDGV.AllowUserToAddRows = false;
            this.connectionsDGV.AllowUserToDeleteRows = false;
            this.connectionsDGV.AllowUserToResizeColumns = false;
            this.connectionsDGV.AllowUserToResizeRows = false;
            this.connectionsDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.connectionsDGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.connectionsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.connectionsDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectionsDGV.Location = new System.Drawing.Point(3, 57);
            this.connectionsDGV.Name = "connectionsDGV";
            this.connectionsDGV.RowHeadersVisible = false;
            this.connectionsDGV.ShowCellErrors = false;
            this.connectionsDGV.ShowCellToolTips = false;
            this.connectionsDGV.ShowEditingIcon = false;
            this.connectionsDGV.ShowRowErrors = false;
            this.connectionsDGV.Size = new System.Drawing.Size(460, 421);
            this.connectionsDGV.TabIndex = 26;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ipTextbox);
            this.panel1.Controls.Add(this.submitIPButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(460, 54);
            this.panel1.TabIndex = 25;
            // 
            // ipTextbox
            // 
            this.ipTextbox.Location = new System.Drawing.Point(9, 17);
            this.ipTextbox.Name = "ipTextbox";
            this.ipTextbox.Size = new System.Drawing.Size(115, 20);
            this.ipTextbox.TabIndex = 1;
            // 
            // submitIPButton
            // 
            this.submitIPButton.Location = new System.Drawing.Point(130, 16);
            this.submitIPButton.Name = "submitIPButton";
            this.submitIPButton.Size = new System.Drawing.Size(75, 23);
            this.submitIPButton.TabIndex = 0;
            this.submitIPButton.Text = "Submit IP";
            this.submitIPButton.UseVisualStyleBackColor = true;
            this.submitIPButton.Click += new System.EventHandler(this.submitIPButton_Click);
            // 
            // JAETech_StratCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 624);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.connectionPanel);
            this.Controls.Add(this.LoginPanel);
            this.Name = "JAETech_StratCalc";
            this.Text = "JAETech Strategy Calculator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JAETech_StratCalc_FormClosing);
            this.Load += new System.EventHandler(this.JAETech_StratCalc_Load);
            this.LoginPanel.ResumeLayout(false);
            this.LoginPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.settingsTP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.connectionsDGV)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LoginPanel;
        private System.Windows.Forms.Label LoginStatusLabel;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox RegularPassword;
        private System.Windows.Forms.TextBox Username;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel connectionPanel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage stratInputTP;
        private System.Windows.Forms.TabPage settingsTP;
        private System.Windows.Forms.DataGridView connectionsDGV;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox ipTextbox;
        private System.Windows.Forms.Button submitIPButton;
    }
}

