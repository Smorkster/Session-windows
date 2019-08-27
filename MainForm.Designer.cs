
namespace Session_windows
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button btnGetRunningProcesses;
		private System.Windows.Forms.Button btnSetProcessInfoLayout;
		private System.Windows.Forms.TextBox txtX;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtY;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtWidth;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtHeight;
		private System.Windows.Forms.ToolTip ttInfo;
		private System.Windows.Forms.TextBox txtProcess;
		private System.Windows.Forms.TextBox txtId;
		private System.Windows.Forms.ComboBox comboboxWindowPlacement;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ContextMenuStrip conmenuProcessAction;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox comboboxDocked;
		private System.Windows.Forms.ComboBox comboboxUndocked;
		private System.Windows.Forms.ToolStripMenuItem conmenuDelete;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkboxStart;
		private System.Windows.Forms.Button btnCreateNewSession;
		private System.Windows.Forms.Button btnWinInfo;
		private System.Windows.Forms.ContextMenuStrip conmsActiveWindows;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ListView lvProcesses;
		private System.Windows.Forms.ColumnHeader colID;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ContextMenuStrip conmSessions;
		private System.Windows.Forms.GroupBox groupBoxProcesses;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Button btnLoadSession;
		private System.Windows.Forms.Button btnDeleteLoadedSession;
		private System.Windows.Forms.Button btnApplySession;
		private System.Windows.Forms.Button btnSaveProcessInfoToSettings;
		private System.Windows.Forms.Button btnRemoveProcess;
		private System.Windows.Forms.CheckBox checkboxTaskbar;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnExcludedApplications;
		private System.Windows.Forms.ToolStripMenuItem conmenuExcludeApplication;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.btnGetRunningProcesses = new System.Windows.Forms.Button();
			this.btnSetProcessInfoLayout = new System.Windows.Forms.Button();
			this.txtX = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtY = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtWidth = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtHeight = new System.Windows.Forms.TextBox();
			this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
			this.btnSaveProcessInfoToSettings = new System.Windows.Forms.Button();
			this.txtProcess = new System.Windows.Forms.TextBox();
			this.txtId = new System.Windows.Forms.TextBox();
			this.comboboxWindowPlacement = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.conmenuProcessAction = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.conmenuDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.conmenuExcludeApplication = new System.Windows.Forms.ToolStripMenuItem();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.comboboxDocked = new System.Windows.Forms.ComboBox();
			this.comboboxUndocked = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnRemoveProcess = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.checkboxStart = new System.Windows.Forms.CheckBox();
			this.btnCreateNewSession = new System.Windows.Forms.Button();
			this.btnWinInfo = new System.Windows.Forms.Button();
			this.conmsActiveWindows = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.lvProcesses = new System.Windows.Forms.ListView();
			this.colID = new System.Windows.Forms.ColumnHeader();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.conmSessions = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.groupBoxProcesses = new System.Windows.Forms.GroupBox();
			this.btnApplySession = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.btnLoadSession = new System.Windows.Forms.Button();
			this.btnDeleteLoadedSession = new System.Windows.Forms.Button();
			this.checkboxTaskbar = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnExcludedApplications = new System.Windows.Forms.Button();
			this.conmenuProcessAction.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBoxProcesses.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnGetRunningProcesses
			// 
			this.btnGetRunningProcesses.AutoSize = true;
			this.btnGetRunningProcesses.Location = new System.Drawing.Point(12, 157);
			this.btnGetRunningProcesses.Name = "btnGetRunningProcesses";
			this.btnGetRunningProcesses.Size = new System.Drawing.Size(138, 23);
			this.btnGetRunningProcesses.TabIndex = 1;
			this.btnGetRunningProcesses.Text = "Get running processes";
			this.btnGetRunningProcesses.UseVisualStyleBackColor = true;
			this.btnGetRunningProcesses.Click += new System.EventHandler(this.btnGetRunningProcesses_Click);
			this.btnGetRunningProcesses.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			this.btnGetRunningProcesses.MouseHover += new System.EventHandler(this.btnGetProcesses_MouseHover);
			// 
			// btnSetProcessInfoLayout
			// 
			this.btnSetProcessInfoLayout.AutoSize = true;
			this.btnSetProcessInfoLayout.Enabled = false;
			this.btnSetProcessInfoLayout.Location = new System.Drawing.Point(183, 190);
			this.btnSetProcessInfoLayout.Name = "btnSetProcessInfoLayout";
			this.btnSetProcessInfoLayout.Size = new System.Drawing.Size(172, 23);
			this.btnSetProcessInfoLayout.TabIndex = 2;
			this.btnSetProcessInfoLayout.Text = "Set process layout";
			this.ttInfo.SetToolTip(this.btnSetProcessInfoLayout, "Apply this Window layout to the process (Does not save to the session)");
			this.btnSetProcessInfoLayout.UseVisualStyleBackColor = true;
			this.btnSetProcessInfoLayout.Click += new System.EventHandler(this.btnSetProcessInfo_Click);
			// 
			// txtX
			// 
			this.txtX.Location = new System.Drawing.Point(80, 59);
			this.txtX.Name = "txtX";
			this.txtX.Size = new System.Drawing.Size(55, 20);
			this.txtX.TabIndex = 3;
			this.txtX.TextChanged += new System.EventHandler(this.txtX_TextChanged);
			this.txtX.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			this.txtX.MouseHover += new System.EventHandler(this.txtCoordinate_MouseHover);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 62);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "X Top left";
			this.ttInfo.SetToolTip(this.label1, "X-coordinate for top left corner");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(141, 62);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Y Top left";
			this.ttInfo.SetToolTip(this.label2, "Y-coordinate for top left corner");
			// 
			// txtY
			// 
			this.txtY.Location = new System.Drawing.Point(214, 59);
			this.txtY.Name = "txtY";
			this.txtY.Size = new System.Drawing.Size(55, 20);
			this.txtY.TabIndex = 5;
			this.txtY.TextChanged += new System.EventHandler(this.txtY_TextChanged);
			this.txtY.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			this.txtY.MouseHover += new System.EventHandler(this.txtCoordinate_MouseHover);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(141, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Width";
			// 
			// txtWidth
			// 
			this.txtWidth.Location = new System.Drawing.Point(214, 85);
			this.txtWidth.Name = "txtWidth";
			this.txtWidth.Size = new System.Drawing.Size(55, 20);
			this.txtWidth.TabIndex = 7;
			this.txtWidth.TextChanged += new System.EventHandler(this.txtWidth_TextChanged);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 13);
			this.label4.TabIndex = 10;
			this.label4.Text = "Height";
			// 
			// txtHeight
			// 
			this.txtHeight.Location = new System.Drawing.Point(80, 85);
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.Size = new System.Drawing.Size(55, 20);
			this.txtHeight.TabIndex = 9;
			this.txtHeight.TextChanged += new System.EventHandler(this.txtHeight_TextChanged);
			// 
			// btnSaveProcessInfoToSettings
			// 
			this.btnSaveProcessInfoToSettings.AutoSize = true;
			this.btnSaveProcessInfoToSettings.Location = new System.Drawing.Point(6, 190);
			this.btnSaveProcessInfoToSettings.Name = "btnSaveProcessInfoToSettings";
			this.btnSaveProcessInfoToSettings.Size = new System.Drawing.Size(172, 23);
			this.btnSaveProcessInfoToSettings.TabIndex = 21;
			this.btnSaveProcessInfoToSettings.Text = "Save process to session";
			this.ttInfo.SetToolTip(this.btnSaveProcessInfoToSettings, "Save this information to the session");
			this.btnSaveProcessInfoToSettings.UseVisualStyleBackColor = true;
			this.btnSaveProcessInfoToSettings.Click += new System.EventHandler(this.btnSaveProcessInfo_Click);
			// 
			// txtProcess
			// 
			this.txtProcess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcess.Location = new System.Drawing.Point(6, 32);
			this.txtProcess.Name = "txtProcess";
			this.txtProcess.ReadOnly = true;
			this.txtProcess.Size = new System.Drawing.Size(361, 20);
			this.txtProcess.TabIndex = 11;
			// 
			// txtId
			// 
			this.txtId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtId.Location = new System.Drawing.Point(6, 71);
			this.txtId.Name = "txtId";
			this.txtId.ReadOnly = true;
			this.txtId.Size = new System.Drawing.Size(361, 20);
			this.txtId.TabIndex = 13;
			// 
			// comboboxWindowPlacement
			// 
			this.comboboxWindowPlacement.FormattingEnabled = true;
			this.comboboxWindowPlacement.Items.AddRange(new object[] {
			"Normal",
			"Minimized",
			"Maximized"});
			this.comboboxWindowPlacement.Location = new System.Drawing.Point(6, 32);
			this.comboboxWindowPlacement.Name = "comboboxWindowPlacement";
			this.comboboxWindowPlacement.Size = new System.Drawing.Size(262, 21);
			this.comboboxWindowPlacement.TabIndex = 17;
			this.comboboxWindowPlacement.SelectedIndexChanged += new System.EventHandler(this.comboboxWindowPlacement_SelectedIndexChanged);
			this.comboboxWindowPlacement.Click += new System.EventHandler(this.comboboxMinimized_Click);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(71, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Processname";
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 55);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(59, 13);
			this.label6.TabIndex = 19;
			this.label6.Text = "Process ID";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(51, 13);
			this.label7.TabIndex = 20;
			this.label7.Text = "Show as:";
			// 
			// conmenuProcessAction
			// 
			this.conmenuProcessAction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.conmenuDelete,
			this.conmenuExcludeApplication});
			this.conmenuProcessAction.Name = "cmSysTray";
			this.conmenuProcessAction.ShowImageMargin = false;
			this.conmenuProcessAction.Size = new System.Drawing.Size(152, 48);
			// 
			// conmenuDelete
			// 
			this.conmenuDelete.Name = "conmenuDelete";
			this.conmenuDelete.Size = new System.Drawing.Size(151, 22);
			this.conmenuDelete.Text = "Delete process";
			this.conmenuDelete.Click += new System.EventHandler(this.deleteProcess);
			// 
			// conmenuExcludeApplication
			// 
			this.conmenuExcludeApplication.Name = "conmenuExcludeApplication";
			this.conmenuExcludeApplication.Size = new System.Drawing.Size(151, 22);
			this.conmenuExcludeApplication.Text = "Exclude application";
			this.conmenuExcludeApplication.Click += new System.EventHandler(this.excludeApplication);
			// 
			// label8
			// 
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 39);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(112, 13);
			this.label8.TabIndex = 21;
			this.label8.Text = "Session when docked";
			// 
			// label9
			// 
			this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 79);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(124, 13);
			this.label9.TabIndex = 22;
			this.label9.Text = "Session when undocked";
			// 
			// comboboxDocked
			// 
			this.comboboxDocked.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.comboboxDocked.FormattingEnabled = true;
			this.comboboxDocked.Location = new System.Drawing.Point(6, 55);
			this.comboboxDocked.Name = "comboboxDocked";
			this.comboboxDocked.Size = new System.Drawing.Size(124, 21);
			this.comboboxDocked.TabIndex = 23;
			this.comboboxDocked.SelectedIndexChanged += new System.EventHandler(this.comboboxDocked_SelectedIndexChanged);
			// 
			// comboboxUndocked
			// 
			this.comboboxUndocked.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.comboboxUndocked.FormattingEnabled = true;
			this.comboboxUndocked.Location = new System.Drawing.Point(6, 95);
			this.comboboxUndocked.Name = "comboboxUndocked";
			this.comboboxUndocked.Size = new System.Drawing.Size(124, 21);
			this.comboboxUndocked.TabIndex = 24;
			this.comboboxUndocked.SelectedIndexChanged += new System.EventHandler(this.comboboxUndocked_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnRemoveProcess);
			this.groupBox1.Controls.Add(this.btnSaveProcessInfoToSettings);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.btnSetProcessInfoLayout);
			this.groupBox1.Controls.Add(this.txtX);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtY);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtWidth);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtHeight);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.comboboxWindowPlacement);
			this.groupBox1.Location = new System.Drawing.Point(6, 97);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(361, 219);
			this.groupBox1.TabIndex = 25;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Window layout settings";
			// 
			// btnRemoveProcess
			// 
			this.btnRemoveProcess.Location = new System.Drawing.Point(6, 161);
			this.btnRemoveProcess.Name = "btnRemoveProcess";
			this.btnRemoveProcess.Size = new System.Drawing.Size(172, 23);
			this.btnRemoveProcess.TabIndex = 22;
			this.btnRemoveProcess.Text = "Remove process from session";
			this.btnRemoveProcess.UseVisualStyleBackColor = true;
			this.btnRemoveProcess.Click += new System.EventHandler(this.btnRemoveProcess_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.btnClose);
			this.groupBox2.Controls.Add(this.checkboxStart);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.comboboxDocked);
			this.groupBox2.Controls.Add(this.comboboxUndocked);
			this.groupBox2.Location = new System.Drawing.Point(12, 215);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(138, 203);
			this.groupBox2.TabIndex = 26;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Settings";
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClose.Location = new System.Drawing.Point(6, 173);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(126, 23);
			this.btnClose.TabIndex = 30;
			this.btnClose.Text = "Exit application";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// checkboxStart
			// 
			this.checkboxStart.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.checkboxStart.AutoSize = true;
			this.checkboxStart.Location = new System.Drawing.Point(6, 19);
			this.checkboxStart.Name = "checkboxStart";
			this.checkboxStart.Size = new System.Drawing.Size(94, 17);
			this.checkboxStart.TabIndex = 0;
			this.checkboxStart.Text = "Start in systray";
			this.checkboxStart.UseVisualStyleBackColor = true;
			this.checkboxStart.CheckedChanged += new System.EventHandler(this.checkboxStart_CheckedChanged);
			// 
			// btnCreateNewSession
			// 
			this.btnCreateNewSession.Location = new System.Drawing.Point(12, 70);
			this.btnCreateNewSession.Name = "btnCreateNewSession";
			this.btnCreateNewSession.Size = new System.Drawing.Size(138, 23);
			this.btnCreateNewSession.TabIndex = 28;
			this.btnCreateNewSession.Text = "Create new session";
			this.btnCreateNewSession.UseVisualStyleBackColor = true;
			this.btnCreateNewSession.Click += new System.EventHandler(this.btnCreateNewSession_Click);
			// 
			// btnWinInfo
			// 
			this.btnWinInfo.Location = new System.Drawing.Point(12, 186);
			this.btnWinInfo.Name = "btnWinInfo";
			this.btnWinInfo.Size = new System.Drawing.Size(138, 23);
			this.btnWinInfo.TabIndex = 29;
			this.btnWinInfo.Text = "WinInfo";
			this.btnWinInfo.UseVisualStyleBackColor = true;
			this.btnWinInfo.Click += new System.EventHandler(this.btnWinInfo_Click);
			// 
			// conmsActiveWindows
			// 
			this.conmsActiveWindows.Name = "conMS";
			this.conmsActiveWindows.ShowImageMargin = false;
			this.conmsActiveWindows.Size = new System.Drawing.Size(36, 4);
			// 
			// lvProcesses
			// 
			this.lvProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left)));
			this.lvProcesses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.colID,
			this.colName});
			this.lvProcesses.FullRowSelect = true;
			this.lvProcesses.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvProcesses.HideSelection = false;
			this.lvProcesses.Location = new System.Drawing.Point(6, 19);
			this.lvProcesses.MultiSelect = false;
			this.lvProcesses.Name = "lvProcesses";
			this.lvProcesses.Size = new System.Drawing.Size(288, 351);
			this.lvProcesses.TabIndex = 30;
			this.lvProcesses.UseCompatibleStateImageBehavior = false;
			this.lvProcesses.View = System.Windows.Forms.View.Details;
			this.lvProcesses.SelectedIndexChanged += new System.EventHandler(this.lvProcesses_SelectedIndexChanged);
			this.lvProcesses.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvProcesses_MouseDown);
			// 
			// colID
			// 
			this.colID.Text = "PID";
			this.colID.Width = 70;
			// 
			// colName
			// 
			this.colName.Text = "PName";
			this.colName.Width = 190;
			// 
			// conmSessions
			// 
			this.conmSessions.Name = "conmSessions";
			this.conmSessions.ShowImageMargin = false;
			this.conmSessions.Size = new System.Drawing.Size(36, 4);
			// 
			// groupBoxProcesses
			// 
			this.groupBoxProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left)));
			this.groupBoxProcesses.Controls.Add(this.btnApplySession);
			this.groupBoxProcesses.Controls.Add(this.lvProcesses);
			this.groupBoxProcesses.Location = new System.Drawing.Point(156, 12);
			this.groupBoxProcesses.Name = "groupBoxProcesses";
			this.groupBoxProcesses.Size = new System.Drawing.Size(300, 406);
			this.groupBoxProcesses.TabIndex = 30;
			this.groupBoxProcesses.TabStop = false;
			this.groupBoxProcesses.Text = "Currently running processes";
			// 
			// btnApplySession
			// 
			this.btnApplySession.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnApplySession.AutoSize = true;
			this.btnApplySession.Location = new System.Drawing.Point(6, 376);
			this.btnApplySession.Name = "btnApplySession";
			this.btnApplySession.Size = new System.Drawing.Size(288, 23);
			this.btnApplySession.TabIndex = 31;
			this.btnApplySession.Text = "Apply this session";
			this.btnApplySession.UseVisualStyleBackColor = true;
			this.btnApplySession.Click += new System.EventHandler(this.btnApplySession_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label5);
			this.groupBox4.Controls.Add(this.txtProcess);
			this.groupBox4.Controls.Add(this.label6);
			this.groupBox4.Controls.Add(this.txtId);
			this.groupBox4.Controls.Add(this.groupBox1);
			this.groupBox4.Location = new System.Drawing.Point(6, 42);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(373, 322);
			this.groupBox4.TabIndex = 31;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Window information saved in session";
			// 
			// btnLoadSession
			// 
			this.btnLoadSession.Location = new System.Drawing.Point(12, 12);
			this.btnLoadSession.Name = "btnLoadSession";
			this.btnLoadSession.Size = new System.Drawing.Size(138, 23);
			this.btnLoadSession.TabIndex = 32;
			this.btnLoadSession.Tag = "load";
			this.btnLoadSession.Text = "Load session ->";
			this.btnLoadSession.UseVisualStyleBackColor = true;
			this.btnLoadSession.Click += new System.EventHandler(this.btnLoadSession_Click);
			// 
			// btnDeleteLoadedSession
			// 
			this.btnDeleteLoadedSession.Location = new System.Drawing.Point(12, 41);
			this.btnDeleteLoadedSession.Name = "btnDeleteLoadedSession";
			this.btnDeleteLoadedSession.Size = new System.Drawing.Size(138, 23);
			this.btnDeleteLoadedSession.TabIndex = 33;
			this.btnDeleteLoadedSession.Tag = "delete";
			this.btnDeleteLoadedSession.Text = "Delete loaded session";
			this.btnDeleteLoadedSession.UseVisualStyleBackColor = true;
			this.btnDeleteLoadedSession.Click += new System.EventHandler(this.btnDeleteLoadedSession_Click);
			// 
			// checkboxTaskbar
			// 
			this.checkboxTaskbar.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.checkboxTaskbar.AutoSize = true;
			this.checkboxTaskbar.Location = new System.Drawing.Point(6, 19);
			this.checkboxTaskbar.Name = "checkboxTaskbar";
			this.checkboxTaskbar.Size = new System.Drawing.Size(97, 17);
			this.checkboxTaskbar.TabIndex = 31;
			this.checkboxTaskbar.Text = "Taskbar visible";
			this.checkboxTaskbar.UseVisualStyleBackColor = true;
			this.checkboxTaskbar.CheckedChanged += new System.EventHandler(this.checkboxTaskbar_CheckedChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.checkboxTaskbar);
			this.groupBox3.Controls.Add(this.groupBox4);
			this.groupBox3.Location = new System.Drawing.Point(462, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(385, 370);
			this.groupBox3.TabIndex = 34;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Session settings";
			// 
			// btnExcludedApplications
			// 
			this.btnExcludedApplications.Location = new System.Drawing.Point(12, 128);
			this.btnExcludedApplications.Name = "btnExcludedApplications";
			this.btnExcludedApplications.Size = new System.Drawing.Size(138, 23);
			this.btnExcludedApplications.TabIndex = 35;
			this.btnExcludedApplications.Text = "Excluded Applications";
			this.btnExcludedApplications.UseVisualStyleBackColor = true;
			this.btnExcludedApplications.Click += new System.EventHandler(this.btnExcludedApplications_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(859, 426);
			this.Controls.Add(this.btnExcludedApplications);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.btnDeleteLoadedSession);
			this.Controls.Add(this.btnLoadSession);
			this.Controls.Add(this.groupBoxProcesses);
			this.Controls.Add(this.btnWinInfo);
			this.Controls.Add(this.btnCreateNewSession);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnGetRunningProcesses);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = global::Session_windows.Resources.ICON;
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(875, 460);
			this.Name = "MainForm";
			this.Text = "Session windows";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.conmenuProcessAction.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBoxProcesses.ResumeLayout(false);
			this.groupBoxProcesses.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
