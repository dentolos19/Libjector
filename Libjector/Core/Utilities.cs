using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Libjector.Core;

public static class Utilities
{

    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWow64Process([In] IntPtr processHandle, [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

    public static string GetProcessArchitecture(Process process)
    {
        if (!Environment.Is64BitOperatingSystem)
            return "32-bit";
        if (!IsWow64Process(process.Handle, out var result))
            return "Unknown Architecture";
        return result ? "32-bit" : "64-bit";
    }

    public static string GetDllArchitecture(string libraryPath)
    {
        using var stream = new FileStream(libraryPath, FileMode.Open, FileAccess.Read);
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
            _ => "Unknown Architecture"
        };
    }

    public static bool IsRunningAsAdministrator()
    {
        return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    }

}