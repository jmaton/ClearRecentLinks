﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClearRecentLinks
{
	public partial class AddFilenameDlg : Form
	{
		public AddFilenameDlg()
		{
			InitializeComponent();
		}

		public string PartialFilename = string.Empty;

		private void btnOk_Click(object sender, EventArgs e)
		{
			PartialFilename = textBox1.Text;

			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
