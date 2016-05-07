using System.IO;
using System.Text;

namespace CodeCoverage
{
    public static class CoverageMonitor
    {
        public static void Run(string coverageFile)
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine("echo off");
            script.AppendLine("cls");
            script.AppendCommand(Properties.Settings.Default.StopIIS);
            foreach (var assembly in Properties.Settings.Default.ListOfAssemblies)
            {
                script.AppendCommand("echo Instrumenting " + assembly);
                script.AppendLine(Properties.Settings.Default.VsInstrExePath + " " + string.Format(Properties.Settings.Default.VsInstrExeArgs, assembly));
            }
            script.AppendCommand(Properties.Settings.Default.VsPerfCmdExePath + " " + string.Format(Properties.Settings.Default.StartVsPerfCmdExeArgs, coverageFile, Properties.Settings.Default.AppPoolIdentity));
            script.AppendCommand(Properties.Settings.Default.StartIIS);
            script.AppendCommand("echo Code coverage monitor started successfully. You can start testing your application and press any key when you are done with your testing.");            
            script.AppendLine("pause");
            script.AppendCommand(Properties.Settings.Default.StopIIS);
            script.AppendCommand(Properties.Settings.Default.VsPerfCmdExePath + " " + Properties.Settings.Default.StopVsPerfCmdExeArgs);
            script.AppendCommand(Properties.Settings.Default.StartIIS);

            //foreach (var assembly in Properties.Settings.Default.ListOfAssemblies)
            //{
            //    script.AppendCommand("echo DeInstrumenting " + assembly);
            //    script.AppendLine("del \"" + assembly + "\"");
            //    script.AppendLine("del \"" + Path.GetDirectoryName(assembly.Value) + "\\" + Path.GetFileNameWithoutExtension(assembly.Value) + ".instr.pdb" + "\"");
            //    script.AppendLine("rename \"" + Path.GetDirectoryName(assembly.Value) + "\\" + Path.GetFileNameWithoutExtension(assembly.Value) + Path.GetExtension(assembly.Value) + ".orig" + "\" " + Path.GetFileName(assembly.Value));
            //}

            script.AppendCommand("echo Code coverage monitor stopped successfully. Code coverage report is ready.");
            script.AppendLine("pause");
            
            var scriptFile = Path.GetTempPath() + "\\temp.bat";
            File.WriteAllText(scriptFile, script.ToString());
            ExternalProgramManager.Run(scriptFile, null, null, null);
        }

        public static void AppendCommand(this StringBuilder script, string cmd)
        {
            script.AppendLine("echo.");
            script.AppendLine("echo.");
            //script.AppendLine("echo " + cmd);
            script.AppendLine(cmd);
        }
    }
}
