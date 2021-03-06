﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RT
{
    class Win32
    {
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);
        [DllImport("User32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        public const int WM_USER = 0x0400;
        public const int WM_COPYDATA = 0x004a;

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            [MarshalAs(UnmanagedType.I4)]
            public int dwData;
            [MarshalAs(UnmanagedType.I4)]
            public int cbData;
            [MarshalAs(UnmanagedType.SysInt)]
            public IntPtr lpData;
        }

        public static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == Win32.WM_COPYDATA)
            {
                Win32.COPYDATASTRUCT data = (Win32.COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(Win32.COPYDATASTRUCT));

                string str = Marshal.PtrToStringUni(data.lpData);
                // Write Log
                //WriteToMessages(str);
            }
            return IntPtr.Zero;
        }

        private void WriteToMessages(string str)
        {
            //MessagesTextBox.Text += Environment.NewLine + "WinForms Says: " + str;
        }
    }

    class Win32Helper
    {
        private const int MULTIPLICATION_FACTOR_UNICODE = 2;
        private const int LENGTH_NULL_CHAR = 1;

        public void SendStringViaCopyData(IntPtr handle,string message)
        {
            IntPtr lpData = Marshal.StringToHGlobalUni(message);

            Win32.COPYDATASTRUCT data = new Win32.COPYDATASTRUCT();
            data.dwData = 0;
            data.cbData = message.Length * MULTIPLICATION_FACTOR_UNICODE + LENGTH_NULL_CHAR;
            data.lpData = lpData;

            IntPtr lpStruct = Marshal.AllocHGlobal(
                Marshal.SizeOf(data));

            Marshal.StructureToPtr(data, lpStruct, false);


            Win32.SendMessage(handle, Win32.WM_COPYDATA,
                IntPtr.Zero, lpStruct);
        }
    }

    class DataStruct
    {
        public int ModuleID;
        public int FunctionID;
        public int SubFunctionID;
        public string[] DataArray; 
    }
}
