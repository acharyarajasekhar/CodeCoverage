using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeCoverage
{
    public partial class ListOfAssemblies : Form
    {
        public ListOfAssemblies()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using(var dialog = new OpenFileDialog())
            {
                dialog.Filter = ".NET Assembly File|*.dll;*.exe";
                dialog.Title = "Select Assembly Files";
                dialog.Multiselect = true;
                
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    lbAssemblies.Items.AddRange(dialog.FileNames);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (int Index in lbAssemblies.SelectedIndices.Cast<int>().Select(x => x).Reverse())
                lbAssemblies.Items.RemoveAt(Index);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ListOfAssemblies.Clear();
            Properties.Settings.Default.ListOfAssemblies.AddRange(lbAssemblies.Items.Cast<string>().Distinct());
            Properties.Settings.Default.Save();
        }

        private void ListOfAssemblies_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.ListOfAssemblies == null)
            {
                Properties.Settings.Default.ListOfAssemblies = new List<string>();
                Properties.Settings.Default.Save();
            }
            lbAssemblies.Items.AddRange(Properties.Settings.Default.ListOfAssemblies.ToArray());
        }
    }
}
