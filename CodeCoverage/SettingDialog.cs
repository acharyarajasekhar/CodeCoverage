using System.Windows.Forms;

namespace CodeCoverage
{
    public partial class SettingDialog : Form
    {
        public SettingDialog()
        {
            InitializeComponent();
            pgCodeCoverageConfig.SelectedObject = Properties.Settings.Default;
        }
    }
}
