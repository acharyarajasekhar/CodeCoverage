﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeCoverage
{
    public class ExternalProgramManager
    {
        public string ProgramFile { get; private set; }
        public string Args { get; private set; }
        public bool IsSuccess { get { return string.IsNullOrEmpty(sbErrorLog.ToString()); } }
        private StringBuilder sbErrorLog;
        private StringBuilder sbOutputLog;

        public static bool Run(string fileName, string args, StringBuilder outputLog, StringBuilder errorLog)
        {
            var instance = new ExternalProgramManager(fileName, args, outputLog, errorLog);
            try
            {                
                var iRetVal = instance.RunExternalProgram();
                if(iRetVal != 0 && errorLog.Length == 0)
                {
                    errorLog.Append("Error Code Recieved: " + iRetVal);
                }
            }
            catch(Exception exc)
            {
                errorLog.Append(exc.Message);
            }
            return instance.IsSuccess;
        }

        private ExternalProgramManager(string fileName, string args, StringBuilder outputLog, StringBuilder errorLog)
        {
            this.ProgramFile = fileName;
            this.Args = args;
            this.sbErrorLog = errorLog;
            this.sbOutputLog = outputLog;
        }

        private int RunExternalProgram()
        {
            var p = new Process();
            p.StartInfo.FileName = ProgramFile;
            p.StartInfo.Arguments = Args;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            p.WaitForExit();
            return p.ExitCode;
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (sbOutputLog != null)
                sbOutputLog.Append(e.Data);
        }

        private void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (sbErrorLog != null)
                sbErrorLog.Append(e.Data);
        }
    }
}