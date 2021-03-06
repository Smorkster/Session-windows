﻿
namespace Session_windows
{
	partial class NewSessionName
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtSessionName;
		
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
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtSessionName = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(267, 10);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 0;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(348, 10);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// txtSessionName
			// 
			this.txtSessionName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.txtSessionName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txtSessionName.Location = new System.Drawing.Point(12, 12);
			this.txtSessionName.Name = "txtSessionName";
			this.txtSessionName.Size = new System.Drawing.Size(249, 20);
			this.txtSessionName.TabIndex = 2;
			this.txtSessionName.TextChanged += new System.EventHandler(this.TxtSessionName_TextChanged);
			this.txtSessionName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtSessionName_KeyDown);
			// 
			// NewSessionName
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(431, 42);
			this.ControlBox = false;
			this.Controls.Add(this.txtSessionName);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Name = "NewSessionName";
			this.Text = "SessionName";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SessionName_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.ToolTip toolTip1;
	}
}
