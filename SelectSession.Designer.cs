
namespace Session_windows
{
	partial class SelectSession
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ComboBox cbSessionSelect;
		private System.Windows.Forms.Button btnChoose;
		
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
			this.cbSessionSelect = new System.Windows.Forms.ComboBox();
			this.btnChoose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cbSessionSelect
			// 
			this.cbSessionSelect.FormattingEnabled = true;
			this.cbSessionSelect.Location = new System.Drawing.Point(12, 12);
			this.cbSessionSelect.Name = "cbSessionSelect";
			this.cbSessionSelect.Size = new System.Drawing.Size(249, 21);
			this.cbSessionSelect.TabIndex = 0;
			// 
			// btnChoose
			// 
			this.btnChoose.Location = new System.Drawing.Point(267, 10);
			this.btnChoose.Name = "btnChoose";
			this.btnChoose.Size = new System.Drawing.Size(75, 23);
			this.btnChoose.TabIndex = 1;
			this.btnChoose.Text = "Choose";
			this.btnChoose.UseVisualStyleBackColor = true;
			this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
			// 
			// SelectSession
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(419, 262);
			this.Controls.Add(this.btnChoose);
			this.Controls.Add(this.cbSessionSelect);
			this.Name = "SelectSession";
			this.Text = "SelectSession";
			this.ResumeLayout(false);

		}
	}
}
