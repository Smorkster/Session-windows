using System;
using System.Windows.Forms;

namespace Session_windows
{

	public partial class SessionName : Form
	{
		public SessionName()
		{
			InitializeComponent();
		}
		void btnSave_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}
		void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
		void SessionName_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(txtSessionName.Text.Length == 0)
			{
				DialogResult a = MessageBox.Show("No name of session.\nDo you want to cancel saving?","No name",MessageBoxButtons.YesNo);
				if(a == DialogResult.Yes)
				{
					DialogResult = DialogResult.Cancel;
					e.Cancel = false;
				}
			}
		}
		public string getName()
		{
			return txtSessionName.Text;
		}
	}
}
