using CodeCoverage.Properties;
using Microsoft.VisualStudio.Coverage.Analysis;
using System;
using System.IO;
using System.Windows.Forms;

namespace CodeCoverage
{
    internal partial class CodeCoverageContext
    {
        /// <summary>
        /// Settings File Changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new SettingDialog())
            {
                switch (dialog.ShowDialog())
                {
                    case DialogResult.OK:
                        Properties.Settings.Default.Save();
                        break;
                    case DialogResult.Ignore:
                        Properties.Settings.Default.Reset();
                        break;
                    case DialogResult.Cancel:
                        Properties.Settings.Default.Reload();
                        break;
                }
            }
        }

        /// <summary>
        /// Opens report in browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewCoverageReportMenuItem_Click(object sender, EventArgs e)
        {
            string coverageFile = string.Empty;

            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Coverage File|*.coverage";
                dialog.Title = "Select Coverage File";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    coverageFile = dialog.FileName;
                }
            }

            if (!string.IsNullOrEmpty(coverageFile))
            {
                string coverageXmlFile;
                ToCoverageXml(coverageFile, out coverageXmlFile);
                HandleResult();
                if (CanProceed)
                {
                    var targetDir = Path.GetDirectoryName(coverageFile) + "\\" + Path.GetFileNameWithoutExtension(coverageFile);
                    ExternalProgramManager.Run(Config.ReportGeneratorExePath, string.Format(Config.ReportGeneratorExeArgs, coverageXmlFile, targetDir), null, ErrorLog);
                    HandleResult();
                    if (CanProceed)
                    {
                        ExternalProgramManager.Run(Config.BrowserExePath, targetDir + "\\index.htm", null, ErrorLog);
                        HandleResult();
                    }
                }
            }
        }

        /// <summary>
        /// Start Session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSessionMenuItem_Click(object sender, EventArgs e)
        {
            string coverageFile = string.Empty;

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Coverage File|*.coverage";
                dialog.Title = "Select Coverage File";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    coverageFile = dialog.FileName;
                }
            }

            if (!string.IsNullOrEmpty(coverageFile))
            {
                ExternalProgramManager.Run(Properties.Settings.Default.VsPerfCmdExePath, string.Format(Properties.Settings.Default.StartVsPerfCmdExeArgs, coverageFile, Properties.Settings.Default.AppPoolIdentity), null, ErrorLog);
                HandleResult();
            }
        }

        /// <summary>
        /// Stop Session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopSessionMenuItem_Click(object sender, EventArgs e)
        {
            ExternalProgramManager.Run(Properties.Settings.Default.VsPerfCmdExePath, Properties.Settings.Default.StopVsPerfCmdExeArgs, null, ErrorLog);
            HandleResult();
        }        

        /// <summary>
        /// Exit Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to close me?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
