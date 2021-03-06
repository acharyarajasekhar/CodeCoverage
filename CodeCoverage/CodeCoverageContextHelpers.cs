﻿using CodeCoverage.Properties;
using Microsoft.VisualStudio.Coverage.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeCoverage
{
    internal partial class CodeCoverageContext
    {
        private Settings Config { get { return Properties.Settings.Default; } }
        private bool CanProceed { get { return ErrorLog.Length == 0; } }

        private void ToCoverageXml(string coverageFile, out string coverageXmlFile)
        {
            try
            {
                using (CoverageInfo info = CoverageInfo.CreateFromFile(coverageFile))
                {
                    CoverageDS data = info.BuildDataSet();
                    coverageXmlFile = Path.GetTempFileName();
                    data.WriteXml(coverageXmlFile);
                }
            }
            catch (Exception exc)
            {
                coverageXmlFile = string.Empty;
                ErrorLog.Append(exc.Message);
            }
        }

        private void HandleResult()
        {
            if (ErrorLog.Length != 0)
            {
                //TrayIcon.ShowBalloonTip(10000, "Error", ErrorLog.ToString(), ToolTipIcon.Error);
                MessageBox.Show(ErrorLog.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLog.Clear();
            }
        }

        private void OpenCoverageReport(string coverageFile)
        {
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
    }
}
