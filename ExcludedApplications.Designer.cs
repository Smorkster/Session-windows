
namespace Session_windows
{
	partial class ExcludedApplications
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.ListView lvAppList;
		
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.lvAppList = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(174, 217);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(93, 217);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(12, 217);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(75, 23);
			this.btnRemove.TabIndex = 3;
			this.btnRemove.Text = "Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// lvAppList
			// 
			this.lvAppList.Location = new System.Drawing.Point(12, 12);
			this.lvAppList.MultiSelect = false;
			this.lvAppList.Name = "lvAppList";
			this.lvAppList.Size = new System.Drawing.Size(237, 199);
			this.lvAppList.TabIndex = 4;
			this.lvAppList.UseCompatibleStateImageBehavior = false;
			this.lvAppList.View = System.Windows.Forms.View.List;
			// 
			// ExcludedApplications
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(261, 250);
			this.ControlBox = false;
			this.Controls.Add(this.lvAppList);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnCancel);
			this.Name = "ExcludedApplications";
			this.Text = "ExcludedApplications";
			this.ResumeLayout(false);

		}
	}
}
