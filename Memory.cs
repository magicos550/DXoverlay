using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Yato.DirectXOverlay
{
    class Memory
    {
        #region Kernel32
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwAccess, bool inherit, int pid);

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] IntPtr lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesRead);
        public static bool ReadMemory(Int64 Address, ref byte[] buffer)
        {
            return ReadProcessMemory(Offsets.ProcessHandle, (UIntPtr)Address, buffer, (UIntPtr)buffer.Length, IntPtr.Zero);
        }
        public static bool ReadMemory(Int64 Address, ref byte[] buffer, int size)
        {
            return ReadProcessMemory(Offsets.ProcessHandle, (UIntPtr)Address, buffer, (UIntPtr)size, IntPtr.Zero);
        }
        public static bool ReadMemory(long Address, [Out] IntPtr lpBuffer, int size)
        {
            return ReadProcessMemory(Offsets.ProcessHandle, (UIntPtr)Address, lpBuffer, (UIntPtr)size, IntPtr.Zero);
        }
        #endregion

        public static byte ReadByte(long Address)
        {
            byte[] buffer = new byte[1];
            if (ReadMemory(Address, ref buffer, 1))
                return buffer[0];
            return 0xff;
        }
        public static float ReadFloat(long Address)
        {
            byte[] buffer = new byte[4];
            if (ReadMemory(Address, ref buffer, 4))
                return BitConverter.ToSingle(buffer, 0);
            return float.MaxValue;
        }


        public static object ReadObject(long Address, Type type)
        {
            int cb = Marshal.SizeOf(type);
            IntPtr lpBuffer = Marshal.AllocHGlobal(cb);
            if (!ReadMemory(Address, lpBuffer, cb))
            {
                return null;
            }
            object obj2 = Marshal.PtrToStructure(lpBuffer, type);
            Marshal.FreeHGlobal(lpBuffer);
            return obj2;
        }
        public static string ReadString(long Address, int len)
        {
            byte[] buffer = new byte[len];
            if (ReadMemory(Address, ref buffer, len))
            {
                string str = Encoding.UTF8.GetString(buffer);
                if (str.IndexOf('\0') != -1)
                    return str.Remove(str.IndexOf('\0'));
                return str;
            }
            return string.Empty;
        }
        public static string ReadUnicode(long Address, int len)
        {
            byte[] buffer = new byte[len];
            if (ReadMemory(Address, ref buffer, len))
            {
                string str = Encoding.Unicode.GetString(buffer);
                if (str.IndexOf('\0') != -1)
                    return str.Remove(str.IndexOf('\0'));
                return str;
            }
            return string.Empty;
        }
        public static uint ReadUInt(long Address)
        {
            byte[] buffer = new byte[4];
            if (ReadMemory(Address, ref buffer, 4))
                return BitConverter.ToUInt32(buffer, 0);
            return uint.MaxValue;
        }
        public static UInt64 ReadUInt64(Int64 Address)
        {
            byte[] buffer = new byte[8];
            if (ReadMemory(Address, ref buffer, 8))
                return BitConverter.ToUInt64(buffer, 0);
            return UInt64.MaxValue;
        }
    }
}
