using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
