
using Microsoft.VisualStudio.Coverage.Analysis;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Config = CodeCoverage.Properties.Settings;

namespace CodeCoverage
{
    public partial class TrayAppContextMenu
    {
        private MenuItem openReportMenu;
        private MenuItem exitMenu;
        private MenuItem startSessionMenu;
        private MenuItem stopSessionMenu;

        private void OnExit(object sender, System.EventArgs e)
        {
            Application.Exit();
        }        

        private void OnSessionStart(object sender, System.EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Coverage File|*.coverage";
                dialog.Title = "Save Coverage File";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var errorLog = new StringBuilder();
                    var bResult = ExternalProgramManager.Run(Config.Default.VsPerfCmdExe, string.Format(Config.Default.StartVsPerfCmdExe, dialog.FileName), null, errorLog);
                    HandleErrorIfAny(errorLog, bResult);
                }
            }
        }

        private static void HandleErrorIfAny(StringBuilder errorLog, bool bResult)
        {
            if (!bResult)
            {
                MessageBox.Show(errorLog.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnSessionStop(object sender, System.EventArgs e)
        {
            var errorLog = new StringBuilder();
            var bResult = ExternalProgramManager.Run(Config.Default.VsPerfCmdExe, Config.Default.StopVsPerfCmdExe, null, errorLog);
            HandleErrorIfAny(errorLog, bResult);
        }

        private void OnOpenReport(object sender, System.EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Coverage File|*.coverage";
                dialog.Title = "Select Coverage File";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var errorLog = new StringBuilder();
                    string coverageXmlFile;
                    var isSuccess = ToCoverageXml(dialog.FileName, errorLog, out coverageXmlFile);
                    HandleErrorIfAny(errorLog, isSuccess);
                    if (isSuccess)
                    {
                        var targetDir = Path.GetDirectoryName(dialog.FileName) + "\\" + Path.GetFileNameWithoutExtension(dialog.FileName);
                        isSuccess = ExternalProgramManager.Run(Config.Default.ReportGeneratorExe, string.Format(Config.Default.ReportGeneratorExeArgs, coverageXmlFile, targetDir), null, errorLog);
                        HandleErrorIfAny(errorLog, isSuccess);
                        if(isSuccess)
                        {
                            isSuccess = ExternalProgramManager.Run(Config.Default.BrowserExe, targetDir + "\\index.htm", null, errorLog); 
                            HandleErrorIfAny(errorLog, isSuccess);
                        }
                    }
                }
            }
        }

        private void WireUpEvents()
        {
            startSessionMenu = new MenuItem("Start", OnSessionStart);
            stopSessionMenu = new MenuItem("Stop", OnSessionStop);
            openReportMenu = new MenuItem("Open Report", OnOpenReport);
            exitMenu = new MenuItem("Exit", OnExit);

            trayMenu.MenuItems.Add("New Session", new MenuItem[] { startSessionMenu, stopSessionMenu });
            trayMenu.MenuItems.Add(openReportMenu);
            trayMenu.MenuItems.Add(exitMenu);
        }

        private bool ToCoverageXml(string coverageFilePath, StringBuilder errorLog, out string coverageXmlFilePath)
        {
            try
            {
                using (CoverageInfo info = CoverageInfo.CreateFromFile(coverageFilePath))
                {
                    CoverageDS data = info.BuildDataSet();
                    coverageXmlFilePath = Path.GetTempFileName();
                    data.WriteXml(coverageXmlFilePath);
                }
            }
            catch(Exception exc)
            {
                coverageXmlFilePath = string.Empty;
                errorLog.Append(exc.Message);
            }
            return errorLog.Length == 0;
        }

    }
}
