
namespace Session_windows
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ListBox lbProcesses;
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
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtId;
		private System.Windows.Forms.ListBox lbSessions;
		private System.Windows.Forms.ContextMenuStrip conmenuSave;
		private System.Windows.Forms.ToolStripMenuItem conmenuNew;
		private System.Windows.Forms.ToolStripMenuItem conmenuMarked;
		private System.Windows.Forms.ComboBox cbMinimized;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ContextMenuStrip conmenuDeleteProcess;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cbDocked;
		private System.Windows.Forms.ComboBox cbUndocked;
		private System.Windows.Forms.ToolStripMenuItem conmenuDelete;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkBox1;
		
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
			this.lbProcesses = new System.Windows.Forms.ListBox();
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
			this.btnSave = new System.Windows.Forms.Button();
			this.conmenuSave = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.conmenuNew = new System.Windows.Forms.ToolStripMenuItem();
			this.conmenuMarked = new System.Windows.Forms.ToolStripMenuItem();
			this.txtId = new System.Windows.Forms.TextBox();
			this.lbSessions = new System.Windows.Forms.ListBox();
			this.cbMinimized = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.conmenuDeleteProcess = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.conmenuDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.cbDocked = new System.Windows.Forms.ComboBox();
			this.cbUndocked = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.conmenuSave.SuspendLayout();
			this.conmenuDeleteProcess.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbProcesses
			// 
			this.lbProcesses.FormattingEnabled = true;
			this.lbProcesses.Location = new System.Drawing.Point(179, 41);
			this.lbProcesses.MultiColumn = true;
			this.lbProcesses.Name = "lbProcesses";
			this.lbProcesses.Size = new System.Drawing.Size(258, 342);
			this.lbProcesses.TabIndex = 0;
			this.lbProcesses.SelectedIndexChanged += new System.EventHandler(this.lbProcesses_SelectedIndexChanged);
			this.lbProcesses.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbProcesses_MouseDown);
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
			this.btnGetProcesses.MouseHover += new System.EventHandler(this.btnGetProcesses_MouseHover);
			// 
			// btnSetProcessInfo
			// 
			this.btnSetProcessInfo.Location = new System.Drawing.Point(151, 144);
			this.btnSetProcessInfo.Name = "btnSetProcessInfo";
			this.btnSetProcessInfo.Size = new System.Drawing.Size(86, 23);
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
			this.txtProcess.Location = new System.Drawing.Point(443, 57);
			this.txtProcess.Name = "txtProcess";
			this.txtProcess.ReadOnly = true;
			this.txtProcess.Size = new System.Drawing.Size(243, 20);
			this.txtProcess.TabIndex = 11;
			// 
			// btnSave
			// 
			this.btnSave.AutoSize = true;
			this.btnSave.ContextMenuStrip = this.conmenuSave;
			this.btnSave.Location = new System.Drawing.Point(12, 12);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(80, 23);
			this.btnSave.TabIndex = 12;
			this.btnSave.Text = "Save session";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnSave.MouseHover += new System.EventHandler(this.btnSave_MouseHover);
			// 
			// conmenuSave
			// 
			this.conmenuSave.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.conmenuNew,
			this.conmenuMarked});
			this.conmenuSave.Name = "cmenuSave";
			this.conmenuSave.Size = new System.Drawing.Size(115, 48);
			this.conmenuSave.Opening += new System.ComponentModel.CancelEventHandler(this.conmenuSave_Opening);
			// 
			// conmenuNew
			// 
			this.conmenuNew.Name = "conmenuNew";
			this.conmenuNew.Size = new System.Drawing.Size(114, 22);
			this.conmenuNew.Text = "New";
			this.conmenuNew.Click += new System.EventHandler(this.conmenuNew_Click);
			// 
			// conmenuMarked
			// 
			this.conmenuMarked.Name = "conmenuMarked";
			this.conmenuMarked.Size = new System.Drawing.Size(114, 22);
			this.conmenuMarked.Text = "Marked";
			this.conmenuMarked.Click += new System.EventHandler(this.conmenuMarked_Click);
			// 
			// txtId
			// 
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
			this.lbSessions.SelectedIndexChanged += new System.EventHandler(this.lbSessions_SelectedIndexChanged);
			// 
			// cbMinimized
			// 
			this.cbMinimized.FormattingEnabled = true;
			this.cbMinimized.Items.AddRange(new object[] {
			"Normal",
			"Minimized",
			"Maximized"});
			this.cbMinimized.Location = new System.Drawing.Point(6, 41);
			this.cbMinimized.Name = "cbMinimized";
			this.cbMinimized.Size = new System.Drawing.Size(231, 21);
			this.cbMinimized.TabIndex = 17;
			this.cbMinimized.SelectedIndexChanged += new System.EventHandler(this.cbMinimized_SelectedIndexChanged);
			this.cbMinimized.Click += new System.EventHandler(this.cbMinimized_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(443, 41);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(45, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Process";
			// 
			// label6
			// 
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
			this.conmenuDeleteProcess.Size = new System.Drawing.Size(151, 26);
			// 
			// conmenuDelete
			// 
			this.conmenuDelete.Name = "conmenuDelete";
			this.conmenuDelete.Size = new System.Drawing.Size(150, 22);
			this.conmenuDelete.Text = "Delete process";
			this.conmenuDelete.Click += new System.EventHandler(this.conmenuDelete_Click);
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
			// cbDocked
			// 
			this.cbDocked.FormattingEnabled = true;
			this.cbDocked.Location = new System.Drawing.Point(12, 317);
			this.cbDocked.Name = "cbDocked";
			this.cbDocked.Size = new System.Drawing.Size(161, 21);
			this.cbDocked.TabIndex = 23;
			this.cbDocked.SelectedIndexChanged += new System.EventHandler(this.cbDocked_SelectedIndexChanged);
			// 
			// cbUndocked
			// 
			this.cbUndocked.FormattingEnabled = true;
			this.cbUndocked.Location = new System.Drawing.Point(12, 357);
			this.cbUndocked.Name = "cbUndocked";
			this.cbUndocked.Size = new System.Drawing.Size(161, 21);
			this.cbUndocked.TabIndex = 24;
			this.cbUndocked.SelectedIndexChanged += new System.EventHandler(this.cbUndocked_SelectedIndexChanged);
			// 
			// groupBox1
			// 
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
			this.groupBox1.Controls.Add(this.cbMinimized);
			this.groupBox1.Location = new System.Drawing.Point(443, 122);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(243, 170);
			this.groupBox1.TabIndex = 25;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Window layout (not current)";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkBox1);
			this.groupBox2.Location = new System.Drawing.Point(443, 298);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(243, 85);
			this.groupBox2.TabIndex = 26;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Settings";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(6, 16);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(94, 17);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Start in systray";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(694, 390);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cbUndocked);
			this.Controls.Add(this.cbDocked);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lbSessions);
			this.Controls.Add(this.txtId);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.txtProcess);
			this.Controls.Add(this.btnGetProcesses);
			this.Controls.Add(this.lbProcesses);
			this.Icon = global::Session_windows.Resources.ICON;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Text = "Session windows";
			this.conmenuSave.ResumeLayout(false);
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
