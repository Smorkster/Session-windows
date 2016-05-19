using System;
using System.Windows.Forms;

namespace Session_windows
{

	public partial class NewSessionName : Form
	{
		public NewSessionName()
		{
			InitializeComponent();
			ActiveControl = txtSessionName;
		}

		/// <summary>
		/// User have entered a name and wants to use it for the session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSave_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// User wants to cancel naming the session
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		/// <summary>
		/// Form is closing, if textbox is empty, notify user
		/// If user wants to cancel saving, set DialogResult to Cancel
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic FormClosingEvent</param>
		void SessionName_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (txtSessionName.Text.Length == 0) {
				DialogResult a = MessageBox.Show("No name of session.\nDo you want to cancel saving?", "No name", MessageBoxButtons.YesNo);
				if (a == DialogResult.Yes) {
					DialogResult = DialogResult.Cancel;
					e.Cancel = false;
				}
			}
		}

		/// <summary>
		/// Used for getting the name the user have entered
		/// </summary>
		/// <returns>The name of the session</returns>
		public string getName()
		{
			return txtSessionName.Text;
		}
	}
}
