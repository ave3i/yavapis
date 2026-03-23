using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace yourAPI
{
    public class API
    {
        public const string DllName = "yav-module.dll";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Attach();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern void Execute([MarshalAs(UnmanagedType.LPWStr)] string input);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool IsAttached();

        public void SetAutoAttach(bool enabled)
        {
            if (enabled)
            {
                Process[] processes = Process.GetProcessesByName("RobloxPlayerBeta");

                if (processes.Length > 0)
                {
                    Attach();
                }
            }
        }

        public void KillRoblox()
        {
            Process[] processes = Process.GetProcessesByName("RobloxPlayerBeta");
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }
    }
}
