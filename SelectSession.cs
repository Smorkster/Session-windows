
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Session_windows
{
	public partial class SelectSession : Form
	{
		List<Session> sl = new List<Session>();
		public SelectSession(List<Session> s)
		{
			InitializeComponent();
			sl = s;
			foreach(Session si in s)
			{
				cbSessionSelect.Items.Add(si.SessionName);
			}
		}
		/// <summary>
		/// Get selected combobox item
		/// </summary>
		/// <returns>Item text as string</returns>
		public Session getSession()
		{
			return sl.Find(x => x.SessionName.Equals(cbSessionSelect.SelectedItem.ToString()));
		}
		void btnChoose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
