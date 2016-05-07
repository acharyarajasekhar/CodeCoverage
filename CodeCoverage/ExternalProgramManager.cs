using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeCoverage
{
    public class ExternalProgramManager
    {
        public string ProgramFile { get; private set; }
        public string Args { get; private set; }
        private StringBuilder sbErrorLog;
        private StringBuilder sbOutputLog;
        
        public static void Run(string fileName, string args, StringBuilder outputLog, StringBuilder errorLog)
        {
            var instance = new ExternalProgramManager(fileName, args, outputLog, errorLog);
            try
            {
                instance.RunExternalProgram();
            }
            catch (Exception exc)
            {
                errorLog.Append(exc.Message);
            }
        }

        private ExternalProgramManager(string fileName, string args, StringBuilder outputLog, StringBuilder errorLog)
        {
            this.ProgramFile = fileName;
            this.Args = args;
            this.sbErrorLog = errorLog;
            this.sbOutputLog = outputLog;
        }

        private void RunExternalProgram()
        {
            var p = new Process();
            p.StartInfo.FileName = ProgramFile;
            p.StartInfo.Arguments = Args;
            p.StartInfo.CreateNoWindow = false;
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardError = true;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            //p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            p.Start();
            //p.BeginErrorReadLine();
            //p.BeginOutputReadLine();
            p.WaitForExit();
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (sbOutputLog != null)
            {
                Console.WriteLine(e.Data);
                sbOutputLog.Append(e.Data);
            }
        }

        private void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (sbErrorLog != null)
            {
                Console.WriteLine(e.Data);
                sbErrorLog.Append(e.Data);
            }
        }
    }
}
