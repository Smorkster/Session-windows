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

			appList.Sort();
			foreach (string app in appList)
			{
				var t = new ListViewItem(new [] { app });
				items.Add(t);
			}

			ListViewItem[] list = items.ToArray();
			lvAppList.Items.AddRange(list);
			this.appList.AddRange(appList);
		}

		#region Operational methods
		/// <summary>
		/// Get applist
		/// </summary>
		/// <returns>The list of applications to exclude</returns>
		internal List<string> GetExcludedApplications()
		{
			return appList;
		}

		#endregion

		#region Event methods
		/// <summary>
		/// User wants to cancel editing the applist
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Remove the application marked in lvAppList
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnRemove_Click(object sender, EventArgs e)
		{
			var index = lvAppList.SelectedItems[0].Index;
			lvAppList.Items[index].Remove();
			btnSave.Enabled = true;
		}

		/// <summary>
		/// User wants to save the list
		/// Update the applist and close the form
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic EventArgs</param>
		void BtnSave_Click(object sender, EventArgs e)
		{
			appList.Clear();
			foreach (ListViewItem i in lvAppList.Items)
				appList.Add(i.Text);
			Close();
		}

		/// <summary>
		/// A click has occured on the listview
		/// Check if item is located at click
		/// If yes, enable button for removal
		/// If no, disable button for removal
		/// </summary>
		/// <param name="sender">Generic object</param>
		/// <param name="e">Generic MouseEventArgs</param>
		void LvAppList_MouseDown(object sender, MouseEventArgs e)
		{
			ListViewItem item = lvAppList.GetItemAt(e.X, e.Y);

			if (item == null)
			{
				btnRemove.Enabled = false;
			}
			else
			{
				btnRemove.Enabled = true;
			}
		}
		#endregion
	}
}
