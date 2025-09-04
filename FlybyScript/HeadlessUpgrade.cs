using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Flyby11
{
    public static class HeadlessUpgrade
    {
        public static int Run_Backup(string isoPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(isoPath))
                {
                    Console.Error.WriteLine("Missing --iso=\\\\server\\share\\Win11.iso");
                    return 2;
                }

                if (!File.Exists(isoPath))
                {
                    Console.Error.WriteLine($"ISO not found: {isoPath}");
                    return 3;
                }

                // Mount ISO (works with UNC too)
                var ps = new ProcessStartInfo("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"Mount-DiskImage -ImagePath '{isoPath.Replace("'", "''")}' -PassThru | Get-Volume | Select -ExpandProperty DriveLetter\"")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };
                var p = Process.Start(ps);
                p.WaitForExit();
                var driveLetter = p.StandardOutput.ReadToEnd().Trim();
                if (string.IsNullOrEmpty(driveLetter))
                {
                    Console.Error.WriteLine("Failed to mount ISO.");
                    return 4;
                }

                var setupExe = $"{driveLetter}:\\setup.exe";
                if (!File.Exists(setupExe))
                {
                    Console.Error.WriteLine("setup.exe not found in ISO.");
                    return 5;
                }

                // Quiet in-place upgrade
                // /noreboot = don’t auto-restart; remove it if you want auto reboot
                var args =
                    "/auto upgrade /quiet /noreboot /compat IgnoreWarning /CopyLogs C:\\Windows\\Temp\\Win11UpgradeLogs " +
                    "/DynamicUpdate Enable /Telemetry Disable";

                var setup = Process.Start(new ProcessStartInfo(setupExe, args) { UseShellExecute = false });
                setup.WaitForExit();

                // Unmount ISO
                Process.Start(new ProcessStartInfo("powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"Dismount-DiskImage -ImagePath '{isoPath.Replace("'", "''")}'\"")
                { UseShellExecute = false }).WaitForExit();

                return setup.ExitCode; // 0 = success
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return 1;
            }
        }

        public static  Run(string isoPath) {
            HandleIso(isoPath);
        }
    }
}
