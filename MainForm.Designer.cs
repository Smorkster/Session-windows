
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
		private System.Windows.Forms.Button btnSetSettings;
		private System.Windows.Forms.ContextMenuStrip cmenuSave;
		private System.Windows.Forms.ToolStripMenuItem cmenuNew;
		private System.Windows.Forms.ToolStripMenuItem cmenuMarked;
		private System.Windows.Forms.ComboBox cbMinimized;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ContextMenuStrip cmSysTray;
		
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
			this.cmenuSave = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmenuNew = new System.Windows.Forms.ToolStripMenuItem();
			this.cmenuMarked = new System.Windows.Forms.ToolStripMenuItem();
			this.txtId = new System.Windows.Forms.TextBox();
			this.lbSessions = new System.Windows.Forms.ListBox();
			this.btnSetSettings = new System.Windows.Forms.Button();
			this.cbMinimized = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.cmSysTray = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmenuSave.SuspendLayout();
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
			// 
			// btnSetProcessInfo
			// 
			this.btnSetProcessInfo.Location = new System.Drawing.Point(600, 214);
			this.btnSetProcessInfo.Name = "btnSetProcessInfo";
			this.btnSetProcessInfo.Size = new System.Drawing.Size(86, 23);
			this.btnSetProcessInfo.TabIndex = 2;
			this.btnSetProcessInfo.Text = "Set";
			this.btnSetProcessInfo.UseVisualStyleBackColor = true;
			this.btnSetProcessInfo.Click += new System.EventHandler(this.btnSetProcessInfo_Click);
			// 
			// txtX
			// 
			this.txtX.Location = new System.Drawing.Point(516, 162);
			this.txtX.Name = "txtX";
			this.txtX.Size = new System.Drawing.Size(55, 20);
			this.txtX.TabIndex = 3;
			this.txtX.TextChanged += new System.EventHandler(this.txtX_TextChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(443, 165);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "X coordinate";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(443, 191);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Y coordinate";
			// 
			// txtY
			// 
			this.txtY.Location = new System.Drawing.Point(516, 188);
			this.txtY.Name = "txtY";
			this.txtY.Size = new System.Drawing.Size(55, 20);
			this.txtY.TabIndex = 5;
			this.txtY.TextChanged += new System.EventHandler(this.txtY_TextChanged);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(577, 165);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Width";
			// 
			// txtWidth
			// 
			this.txtWidth.Location = new System.Drawing.Point(631, 162);
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
			this.label4.Location = new System.Drawing.Point(577, 191);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 13);
			this.label4.TabIndex = 10;
			this.label4.Text = "Height";
			// 
			// txtHeight
			// 
			this.txtHeight.Location = new System.Drawing.Point(631, 188);
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.Size = new System.Drawing.Size(55, 20);
			this.txtHeight.TabIndex = 9;
			this.txtHeight.TextChanged += new System.EventHandler(this.txtHeight_TextChanged);
			// 
			// txtProcess
			// 
			this.txtProcess.Location = new System.Drawing.Point(443, 57);
			this.txtProcess.Name = "txtProcess";
			this.txtProcess.Size = new System.Drawing.Size(243, 20);
			this.txtProcess.TabIndex = 11;
			// 
			// btnSave
			// 
			this.btnSave.AutoSize = true;
			this.btnSave.ContextMenuStrip = this.cmenuSave;
			this.btnSave.Location = new System.Drawing.Point(12, 12);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(80, 23);
			this.btnSave.TabIndex = 12;
			this.btnSave.Text = "Save session";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// cmenuSave
			// 
			this.cmenuSave.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.cmenuNew,
			this.cmenuMarked});
			this.cmenuSave.Name = "cmenuSave";
			this.cmenuSave.Size = new System.Drawing.Size(115, 48);
			// 
			// cmenuNew
			// 
			this.cmenuNew.Name = "cmenuNew";
			this.cmenuNew.Size = new System.Drawing.Size(114, 22);
			this.cmenuNew.Text = "New";
			this.cmenuNew.Click += new System.EventHandler(this.cmenuNew_Click);
			// 
			// cmenuMarked
			// 
			this.cmenuMarked.Name = "cmenuMarked";
			this.cmenuMarked.Size = new System.Drawing.Size(114, 22);
			this.cmenuMarked.Text = "Marked";
			this.cmenuMarked.Click += new System.EventHandler(this.cmenuMarked_Click);
			// 
			// txtId
			// 
			this.txtId.Location = new System.Drawing.Point(443, 96);
			this.txtId.Name = "txtId";
			this.txtId.Size = new System.Drawing.Size(243, 20);
			this.txtId.TabIndex = 13;
			// 
			// lbSessions
			// 
			this.lbSessions.FormattingEnabled = true;
			this.lbSessions.Location = new System.Drawing.Point(12, 41);
			this.lbSessions.Name = "lbSessions";
			this.lbSessions.Size = new System.Drawing.Size(161, 342);
			this.lbSessions.TabIndex = 15;
			this.lbSessions.SelectedIndexChanged += new System.EventHandler(this.lbSessions_SelectedIndexChanged);
			// 
			// btnSetSettings
			// 
			this.btnSetSettings.Location = new System.Drawing.Point(362, 12);
			this.btnSetSettings.Name = "btnSetSettings";
			this.btnSetSettings.Size = new System.Drawing.Size(75, 23);
			this.btnSetSettings.TabIndex = 16;
			this.btnSetSettings.Text = "Set Settings";
			this.btnSetSettings.UseVisualStyleBackColor = true;
			this.btnSetSettings.Click += new System.EventHandler(this.btnSetSettings_Click);
			// 
			// cbMinimized
			// 
			this.cbMinimized.FormattingEnabled = true;
			this.cbMinimized.Items.AddRange(new object[] {
			"Normal",
			"Minimized",
			"Maximized"});
			this.cbMinimized.Location = new System.Drawing.Point(443, 135);
			this.cbMinimized.Name = "cbMinimized";
			this.cbMinimized.Size = new System.Drawing.Size(243, 21);
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
			this.label7.Location = new System.Drawing.Point(443, 119);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(77, 13);
			this.label7.TabIndex = 20;
			this.label7.Text = "Window layout";
			// 
			// cmSysTray
			// 
			this.cmSysTray.Name = "cmSysTray";
			this.cmSysTray.Size = new System.Drawing.Size(153, 26);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(694, 390);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cbMinimized);
			this.Controls.Add(this.btnSetSettings);
			this.Controls.Add(this.lbSessions);
			this.Controls.Add(this.txtId);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.txtProcess);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtHeight);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtWidth);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtY);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtX);
			this.Controls.Add(this.btnSetProcessInfo);
			this.Controls.Add(this.btnGetProcesses);
			this.Controls.Add(this.lbProcesses);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "Session windows";
			this.cmenuSave.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
