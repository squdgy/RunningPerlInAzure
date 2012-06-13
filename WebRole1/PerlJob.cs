using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Diagnostics;
using System.IO;

namespace WebRole1
{
    public class PerlJob : IJob
    {
        const string perlfolder = @"p";

        void IJob.Execute(IJobExecutionContext context)
        {
            var perlcommandline = "example.pl";

            ProcessStartInfo psi = CreateProcessStartInfo(perlcommandline, "perl.exe");
            Process process = Process.Start(psi);
            string stdout = process.StandardOutput.ReadToEnd();
            string stderr = process.StandardError.ReadToEnd();
            Trace.TraceInformation("Script output: " + stdout);
            Trace.TraceError("Script error output: " + stderr);
        }

        private static ProcessStartInfo CreateProcessStartInfo(string perlcommandline, string exeName)
        {
            var psi = new ProcessStartInfo()
            {
                Arguments = perlcommandline,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            AddPerlFoldersToPath(psi, perlfolder);
            LocatePerlBinary(psi, perlfolder, exeName);
            return psi;
        }

        private static void AddPerlFoldersToPath(ProcessStartInfo psi, string perlfolder)
        {
            var currentDir = (new DirectoryInfo(".")).FullName;

            var relPathExtensions = new[]
            {
                @"perl\site\bin",
                @"perl\bin",
                @"c\bin"
            };
            var pathExtensions = relPathExtensions
                .Select(p => Path.Combine(currentDir, perlfolder, p));

            var path = psi.EnvironmentVariables["Path"];
            var pathSegments = new List<string>();
            pathSegments.AddRange(path.Split(';'));
            pathSegments.AddRange(pathExtensions);

            pathSegments
                .Where(p => !Directory.Exists(p)).ToList()
                .ForEach(p => Trace.TraceWarning(
                    string.Format("Path contains non-existent dir \"{0}\"", p)));

            path = string.Join(";", pathSegments);
            psi.EnvironmentVariables["Path"] = path;
        }

        private static void LocatePerlBinary(ProcessStartInfo psi, string perlfolder, string name)
        {
            var currentDir = (new DirectoryInfo(".")).FullName;

            var pathToPerlExecutable = Path.Combine(currentDir, perlfolder, @"perl\bin\" + name);
            pathToPerlExecutable = (new FileInfo(pathToPerlExecutable)).FullName;
            psi.FileName = pathToPerlExecutable;
        }

    }
}