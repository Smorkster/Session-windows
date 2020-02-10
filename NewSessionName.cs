using System;
using System.Windows.Forms;

namespace Session_windows
{
	public partial class NewSessionName : Form
	{
		public NewSessionName(AutoCompleteStringCollection sessionNames)
		{
			InitializeComponent();
			ActiveControl = txtSessionName;
			txtSessionName.AutoCompleteCustomSource = sessionNames;
		}

		/// <summary>
		/// Name is given, button to save session is pressed
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnSave_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// Button to cancel naming the session was pressed
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnCancel_Click(object sender, EventArgs e)
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
			if (DialogResult == DialogResult.OK)
			{
				if (txtSessionName.Text.Length == 0)
				{
					DialogResult a = MessageBox.Show("No name of session.\nDo you want to cancel saving?", "No name", MessageBoxButtons.YesNo);
					if (a == DialogResult.Yes)
					{
						DialogResult = DialogResult.Cancel;
						e.Cancel = false;
					}
				}
			}
		}

		/// <summary>
		/// Used for getting the name the user have entered
		/// </summary>
		/// <returns>The name of the session</returns>
		public string GetName()
		{
			return txtSessionName.Text;
		}

		/// <summary>
		/// Controls if name is already existing
		/// If so, disable button for save
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void TxtSessionName_TextChanged(object sender, EventArgs e)
		{
			if (txtSessionName.AutoCompleteCustomSource.Contains(txtSessionName.Text))
			{
				btnSave.Enabled = false;
				toolTip1.Show("Illegal name. Can't be same as other session.", txtSessionName);
			}
			else if (txtSessionName.Text.Contains("Current"))
			{
				btnSave.Enabled = false;
				toolTip1.Show("Illegal name. 'Current' is reserved for application.", txtSessionName);
			}
		}

		void TxtSessionName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter)
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}
	}
}