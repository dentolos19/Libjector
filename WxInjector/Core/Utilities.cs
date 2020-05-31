using System;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;

namespace WxInjector.Core
{

    public static class Utilities
    {

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetConnectedState(out int flags, int reserved);

        public static bool IsUserOnline()
        {
            return InternetGetConnectedState(out _, 0);
        }

        public static bool IsUpdateAvailable()
        {
            var client = new WebClient();
            var data = client.DownloadString("https://raw.githubusercontent.com/dentolos19/WxInjector/master/VERSION");
            client.Dispose();
            return Version.Parse(data) > Assembly.GetExecutingAssembly().GetName().Version;
        }

    }

}