
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Session_windows
{
	public partial class SelectSession : Form
	{
		public SelectSession(string[] sessionList)
		{
			InitializeComponent();

			foreach (string session in sessionList) {
				cbSessionSelect.Items.Add(session);
			}
		}
		/// <summary>
		/// Get selected combobox item
		/// </summary>
		/// <returns>Item text as string</returns>
		public string getName()
		{
			return cbSessionSelect.SelectedItem.ToString();
		}

		/// <summary>
		/// Button is clicked
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnChoose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
