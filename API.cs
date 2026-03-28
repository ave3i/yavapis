using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;

namespace yourAPI
{
    public static class API
    {
        private const string DllName = "yav-module.dll";
        private static string discord_server = "https://discord.gg/yourserver";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Connect();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void ExecuteScript(string input, string pid);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ReturnConnected(out int count);

        public static void Execute(string script)
        {
            if (string.IsNullOrWhiteSpace(script))
                return;

            var pids = GetConnectedPIDs();

            if (pids.Count == 0)
                return;

            string pid_str = string.Join(",", pids);
            ExecuteScript(script, pid_str);
        }

        public static bool IsInjected()
        {
            return GetConnectedPIDs().Count > 0;
        }

        private static async Task checkInjectStatus()
        {
            while (true)
            {
                await Task.Delay(5000);
                if (isInjected())
                {
                    Execute("print('yourAPI Injected!')");
                    break;
                }
            }
        }
        
        public static void InjectAPI()
        {
            Process.Start(discord_server);
            Connect();
            checkInjectStatus();
        }

        public static List<int> GetConnectedPIDs()
        {
            List<int> result = new List<int>();

            IntPtr ptr = ReturnConnected(out int count);

            if (ptr == IntPtr.Zero || count <= 0)
                return result;

            int[] pids = new int[count];
            Marshal.Copy(ptr, pids, 0, count);

            result.AddRange(pids);
            return result;
        }

        public static List<int> GetRobloxPIDs()
        {
            return Process.GetProcessesByName("RobloxPlayerBeta").Select(p => p.Id).ToList();
        }

        public static bool IsRobloxOpen()
        {
            return Process.GetProcessesByName("RobloxPlayerBeta").Length > 0;
        }

        public static void KillRoblox()
        {
            foreach (var process in Process.GetProcessesByName("RobloxPlayerBeta"))
                process.Kill();
        }

        public static void AutoInject()
        {
            if (Process.GetProcessesByName("RobloxPlayerBeta").Length > 0)
            {
                try
                {
                    Process.Start(discord_server);
                    Connect();
                }
                catch
                {
                }
            }
        }
    }
}
