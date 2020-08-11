using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows;
using ControlzEx.Theming;

namespace WxInjector.Core
{

    internal static class Utilities
    {

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        public static void SetAppTheme(string colorScheme, bool darkMode, bool afterInit = true)
        {
            if (afterInit)
            {
                ThemeManager.Current.ChangeThemeColorScheme(Application.Current, colorScheme);
                ThemeManager.Current.ChangeThemeBaseColor(Application.Current, darkMode 
                    ? ThemeManager.BaseColorDarkConst 
                    : ThemeManager.BaseColorLightConst);
            }
            else
            {
                var dictionary = new ResourceDictionary
                {
                    Source = darkMode
                        ? new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.{colorScheme}.xaml")
                        : new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/Light.{colorScheme}.xaml")
                };
                Application.Current.Resources.MergedDictionaries.Add(dictionary);
            }
        }

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
            switch ((ushort)reader.ReadInt16())
            {
                case 0x8664:
                case 0x200:
                    return "64-bit";
                case 0x14c:
                    return "32-bit";
                default:
                    return "Unspecified";
            }
        }

        public static bool IsRunningAsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
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