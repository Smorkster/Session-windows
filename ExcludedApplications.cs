using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Session_windows
{
	public partial class ExcludedApplications : Form
	{
		List<string> appList = new List<string>();

		public ExcludedApplications(List<string> appList)
		{
			InitializeComponent();
			List<ListViewItem> items = new List<ListViewItem>();

			foreach (string app in appList)
			{
				var t = new ListViewItem(new [] { app });
				items.Add(t);
			}
			ListViewItem[] list = items.ToArray();
			lvAppList.Items.AddRange(list);

			this.appList.AddRange(appList);
		}

		/// <summary>
		/// Remove the application marked in lvAppList
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnRemove_Click(object sender, EventArgs e)
		{
			if (lvAppList.SelectedItems.Count == 1)
			{
				var index = lvAppList.SelectedItems[0].Index;
				lvAppList.Items[index].Remove();
			}
		}

		/// <summary>
		/// User wants to save the list
		/// Update the applist and close the form
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void btnSave_Click(object sender, EventArgs e)
		{
			appList.Clear();
			foreach (ListViewItem i in lvAppList.Items)
				appList.Add(i.Text);
			Close();
		}

		/// <summary>
		/// User wants to cancel editing the applist
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Get applist
		/// </summary>
		/// <returns>The list of applications to exclude</returns>
		public List<string> getExcludedApplications()
		{
			return appList;
		}
	}
}
