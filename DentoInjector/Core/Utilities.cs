using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows;

namespace DentoInjector.Core
{

    public static class Utilities
    {

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        public static string GetDllArchitecture(string dllPath)
        {
            using var stream = new FileStream(dllPath, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(stream);
            stream.Seek(0x3c, SeekOrigin.Begin);
            var offset = reader.ReadInt32();
            stream.Seek(offset, SeekOrigin.Begin);
            var head = reader.ReadUInt32();
            if (head != 0x00004550)
                return "Unknown";
            return (ushort)reader.ReadInt16() switch
            {
                0x8664 => "64-bit",
                0x200 => "64-bit",
                0x14c => "32-bit",
                _ => "Unidentified"
            };
        }

        public static bool IsRunningAsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void RestartApp(string? args = null)
        {
            var location = Assembly.GetExecutingAssembly().Location;
            if (location.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
                location = Path.Combine(Path.GetDirectoryName(location)!, Path.GetFileNameWithoutExtension(location) + ".exe");
            Process.Start(location, args ?? string.Empty);
            Application.Current.Shutdown();
        }

        public static string GetProcessArchitecture(Process process)
        {
            if (!Environment.Is64BitOperatingSystem)
                return "32-bit";
            if (!IsWow64Process(process.Handle, out var result))
                return "Unknown";
            return result ? "32-bit" : "64-bit";
        }

    }

}