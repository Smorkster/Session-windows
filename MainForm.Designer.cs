
namespace Session_windows
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button btnGetProcesses;
		private System.Windows.Forms.Button btnSetProcessInfo;
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
		private System.Windows.Forms.Button btnSaveSession;
		private System.Windows.Forms.TextBox txtId;
		private System.Windows.Forms.ListBox lbSessions;
		private System.Windows.Forms.ComboBox comboboxMinimized;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ContextMenuStrip conmenuDeleteProcess;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox comboboxDocked;
		private System.Windows.Forms.ComboBox comboboxUndocked;
		private System.Windows.Forms.ToolStripMenuItem conmenuDelete;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkboxStart;
		private System.Windows.Forms.Button btnCreateNew;
		private System.Windows.Forms.Button btnWinInfo;
		private System.Windows.Forms.ContextMenuStrip conmsActiveWindows;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ListView lvProcesses;
		private System.Windows.Forms.ColumnHeader colID;
		private System.Windows.Forms.ColumnHeader colName;
		
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
			this.btnGetProcesses = new System.Windows.Forms.Button();
			this.btnSetProcessInfo = new System.Windows.Forms.Button();
			this.txtX = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtY = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtWidth = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtHeight = new System.Windows.Forms.TextBox();
			this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
			this.txtProcess = new System.Windows.Forms.TextBox();
			this.btnSaveSession = new System.Windows.Forms.Button();
			this.txtId = new System.Windows.Forms.TextBox();
			this.lbSessions = new System.Windows.Forms.ListBox();
			this.comboboxMinimized = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.conmenuDeleteProcess = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.conmenuDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.comboboxDocked = new System.Windows.Forms.ComboBox();
			this.comboboxUndocked = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.checkboxStart = new System.Windows.Forms.CheckBox();
			this.btnCreateNew = new System.Windows.Forms.Button();
			this.btnWinInfo = new System.Windows.Forms.Button();
			this.conmsActiveWindows = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.lvProcesses = new System.Windows.Forms.ListView();
			this.colID = new System.Windows.Forms.ColumnHeader();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.conmenuDeleteProcess.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnGetProcesses
			// 
			this.btnGetProcesses.AutoSize = true;
			this.btnGetProcesses.Location = new System.Drawing.Point(179, 12);
			this.btnGetProcesses.Name = "btnGetProcesses";
			this.btnGetProcesses.Size = new System.Drawing.Size(86, 23);
			this.btnGetProcesses.TabIndex = 1;
			this.btnGetProcesses.Text = "Get Processes";
			this.btnGetProcesses.UseVisualStyleBackColor = true;
			this.btnGetProcesses.Click += new System.EventHandler(this.btnGetProcesses_Click);
			this.btnGetProcesses.MouseLeave += new System.EventHandler(this.control_MouseLeave);
			this.btnGetProcesses.MouseHover += new System.EventHandler(this.btnGetProcesses_MouseHover);
			// 
			// btnSetProcessInfo
			// 
			this.btnSetProcessInfo.Enabled = false;
			this.btnSetProcessInfo.Location = new System.Drawing.Point(191, 144);
			this.btnSetProcessInfo.Name = "btnSetProcessInfo";
			this.btnSetProcessInfo.Size = new System.Drawing.Size(46, 23);
			this.btnSetProcessInfo.TabIndex = 2;
			this.btnSetProcessInfo.Text = "Set";
			this.btnSetProcessInfo.UseVisualStyleBackColor = true;
			this.btnSetProcessInfo.Click += new System.EventHandler(this.btnSetProcessInfo_Click);
			// 
			// txtX
			// 
			this.txtX.Location = new System.Drawing.Point(79, 68);
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
			this.label1.Location = new System.Drawing.Point(6, 71);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "X coordinate";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 97);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Y coordinate";
			// 
			// txtY
			// 
			this.txtY.Location = new System.Drawing.Point(79, 94);
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
			this.label3.Location = new System.Drawing.Point(6, 123);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Width";
			// 
			// txtWidth
			// 
			this.txtWidth.Location = new System.Drawing.Point(79, 120);
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
			this.label4.Location = new System.Drawing.Point(6, 149);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 13);
			this.label4.TabIndex = 10;
			this.label4.Text = "Height";
			// 
			// txtHeight
			// 
			this.txtHeight.Location = new System.Drawing.Point(79, 146);
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.Size = new System.Drawing.Size(55, 20);
			this.txtHeight.TabIndex = 9;
			this.txtHeight.TextChanged += new System.EventHandler(this.txtHeight_TextChanged);
			// 
			// txtProcess
			// 
			this.txtProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcess.Location = new System.Drawing.Point(443, 57);
			this.txtProcess.Name = "txtProcess";
			this.txtProcess.ReadOnly = true;
			this.txtProcess.Size = new System.Drawing.Size(243, 20);
			this.txtProcess.TabIndex = 11;
			// 
			// btnSaveSession
			// 
			this.btnSaveSession.AutoSize = true;
			this.btnSaveSession.Location = new System.Drawing.Point(12, 12);
			this.btnSaveSession.Name = "btnSaveSession";
			this.btnSaveSession.Size = new System.Drawing.Size(80, 23);
			this.btnSaveSession.TabIndex = 12;
			this.btnSaveSession.Text = "Save session";
			this.btnSaveSession.UseVisualStyleBackColor = true;
			this.btnSaveSession.Click += new System.EventHandler(this.btnSaveSession_Click);
			// 
			// txtId
			// 
			this.txtId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtId.Location = new System.Drawing.Point(443, 96);
			this.txtId.Name = "txtId";
			this.txtId.ReadOnly = true;
			this.txtId.Size = new System.Drawing.Size(243, 20);
			this.txtId.TabIndex = 13;
			// 
			// lbSessions
			// 
			this.lbSessions.FormattingEnabled = true;
			this.lbSessions.Location = new System.Drawing.Point(12, 41);
			this.lbSessions.Name = "lbSessions";
			this.lbSessions.Size = new System.Drawing.Size(161, 251);
			this.lbSessions.TabIndex = 15;
			this.lbSessions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbSessions_MouseDown);
			// 
			// comboboxMinimized
			// 
			this.comboboxMinimized.FormattingEnabled = true;
			this.comboboxMinimized.Items.AddRange(new object[] {
			"Normal",
			"Minimized",
			"Maximized"});
			this.comboboxMinimized.Location = new System.Drawing.Point(6, 41);
			this.comboboxMinimized.Name = "comboboxMinimized";
			this.comboboxMinimized.Size = new System.Drawing.Size(231, 21);
			this.comboboxMinimized.TabIndex = 17;
			this.comboboxMinimized.SelectedIndexChanged += new System.EventHandler(this.comboboxMinimized_SelectedIndexChanged);
			this.comboboxMinimized.Click += new System.EventHandler(this.comboboxMinimized_Click);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(443, 41);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(45, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Process";
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(443, 80);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(59, 13);
			this.label6.TabIndex = 19;
			this.label6.Text = "Process ID";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 25);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(51, 13);
			this.label7.TabIndex = 20;
			this.label7.Text = "Show as:";
			// 
			// conmenuDeleteProcess
			// 
			this.conmenuDeleteProcess.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.conmenuDelete});
			this.conmenuDeleteProcess.Name = "cmSysTray";
			this.conmenuDeleteProcess.ShowImageMargin = false;
			this.conmenuDeleteProcess.Size = new System.Drawing.Size(126, 26);
			// 
			// conmenuDelete
			// 
			this.conmenuDelete.Name = "conmenuDelete";
			this.conmenuDelete.Size = new System.Drawing.Size(125, 22);
			this.conmenuDelete.Text = "Delete process";
			this.conmenuDelete.Click += new System.EventHandler(this.contextmenuDeleteProcess_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(12, 301);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(112, 13);
			this.label8.TabIndex = 21;
			this.label8.Text = "Session when docked";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(12, 341);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(124, 13);
			this.label9.TabIndex = 22;
			this.label9.Text = "Session when undocked";
			// 
			// comboboxDocked
			// 
			this.comboboxDocked.FormattingEnabled = true;
			this.comboboxDocked.Location = new System.Drawing.Point(12, 317);
			this.comboboxDocked.Name = "comboboxDocked";
			this.comboboxDocked.Size = new System.Drawing.Size(161, 21);
			this.comboboxDocked.TabIndex = 23;
			this.comboboxDocked.SelectedIndexChanged += new System.EventHandler(this.comboboxDocked_SelectedIndexChanged);
			// 
			// comboboxUndocked
			// 
			this.comboboxUndocked.FormattingEnabled = true;
			this.comboboxUndocked.Location = new System.Drawing.Point(12, 357);
			this.comboboxUndocked.Name = "comboboxUndocked";
			this.comboboxUndocked.Size = new System.Drawing.Size(161, 21);
			this.comboboxUndocked.TabIndex = 24;
			this.comboboxUndocked.SelectedIndexChanged += new System.EventHandler(this.comboboxUndocked_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.btnSetProcessInfo);
			this.groupBox1.Controls.Add(this.txtX);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtY);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtWidth);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtHeight);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.comboboxMinimized);
			this.groupBox1.Location = new System.Drawing.Point(443, 122);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(243, 170);
			this.groupBox1.TabIndex = 25;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Window layout (not current)";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.btnClose);
			this.groupBox2.Controls.Add(this.checkboxStart);
			this.groupBox2.Location = new System.Drawing.Point(443, 298);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(243, 85);
			this.groupBox2.TabIndex = 26;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Applicationsettings";
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(149, 56);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(88, 23);
			this.btnClose.TabIndex = 30;
			this.btnClose.Text = "Exit application";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// checkboxStart
			// 
			this.checkboxStart.AutoSize = true;
			this.checkboxStart.Location = new System.Drawing.Point(6, 16);
			this.checkboxStart.Name = "checkboxStart";
			this.checkboxStart.Size = new System.Drawing.Size(94, 17);
			this.checkboxStart.TabIndex = 0;
			this.checkboxStart.Text = "Start in systray";
			this.checkboxStart.UseVisualStyleBackColor = true;
			this.checkboxStart.CheckedChanged += new System.EventHandler(this.checkboxStart_CheckedChanged);
			// 
			// btnCreateNew
			// 
			this.btnCreateNew.Enabled = false;
			this.btnCreateNew.Location = new System.Drawing.Point(98, 12);
			this.btnCreateNew.Name = "btnCreateNew";
			this.btnCreateNew.Size = new System.Drawing.Size(75, 23);
			this.btnCreateNew.TabIndex = 28;
			this.btnCreateNew.Text = "Create new";
			this.btnCreateNew.UseVisualStyleBackColor = true;
			this.btnCreateNew.Click += new System.EventHandler(this.btnCreateNew_Click);
			// 
			// btnWinInfo
			// 
			this.btnWinInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWinInfo.Location = new System.Drawing.Point(362, 12);
			this.btnWinInfo.Name = "btnWinInfo";
			this.btnWinInfo.Size = new System.Drawing.Size(75, 23);
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
			this.lvProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.lvProcesses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.colID,
			this.colName});
			this.lvProcesses.FullRowSelect = true;
			this.lvProcesses.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvProcesses.Location = new System.Drawing.Point(179, 41);
			this.lvProcesses.MultiSelect = false;
			this.lvProcesses.Name = "lvProcesses";
			this.lvProcesses.Size = new System.Drawing.Size(258, 342);
			this.lvProcesses.TabIndex = 30;
			this.lvProcesses.UseCompatibleStateImageBehavior = false;
			this.lvProcesses.View = System.Windows.Forms.View.Details;
			this.lvProcesses.SelectedIndexChanged += new System.EventHandler(this.lvProcesses_SelectedIndexChanged);
			this.lvProcesses.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvProcesses_MouseDown);
			// 
			// colID
			// 
			this.colID.Text = "PID";
			// 
			// colName
			// 
			this.colName.Text = "PName";
			this.colName.Width = 190;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(694, 390);
			this.Controls.Add(this.lvProcesses);
			this.Controls.Add(this.btnWinInfo);
			this.Controls.Add(this.btnCreateNew);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.comboboxUndocked);
			this.Controls.Add(this.comboboxDocked);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lbSessions);
			this.Controls.Add(this.txtId);
			this.Controls.Add(this.btnSaveSession);
			this.Controls.Add(this.txtProcess);
			this.Controls.Add(this.btnGetProcesses);
			this.Icon = global::Session_windows.Resources.ICON;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(706, 422);
			this.Name = "MainForm";
			this.Text = "Session windows";
			this.conmenuDeleteProcess.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
