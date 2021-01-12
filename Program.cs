using System;
using System.Diagnostics;
using System.Numerics;

namespace Yato.DirectXOverlay
{
    class Program
    {
        public struct LocalPlayer {
            public static uint Alive , Team;
            public static Vector3 Pos;
        };
        public struct PlayerList {
            public uint HP, Armor, Team, Alive, Dormant , Spoted;
            public float distance;
            public Vector3 Pos , HeadPos;
        };
        public struct AimPos
        {
            public float Distance;
            public Vector3 HeadPos;
        };
        static void Main(string[] args)
        {
            while (true)
            {
                if (GetProcessesByName("csgo", out Process process))
                {
                    Offsets.ProcessHandle = process.Handle;
                    ProcessModule mod = null;
                    for (int i = 0; i < process.Modules.Count; i++)
                    {
                        mod = process.Modules[i];
                        if (mod.ModuleName.Equals("client.dll"))
                        {
                            Offsets.ClientDll = (uint)mod.BaseAddress;
                            Console.WriteLine("module found. " + mod.ModuleName + " Adress: " + mod.BaseAddress);
                        }
                        else if (mod.ModuleName.Equals("engine.dll"))
                        {
                            Offsets.EngineDll = (uint)mod.BaseAddress;
                            Console.WriteLine("module found. " + mod.ModuleName + " Adress: " + mod.BaseAddress);
                        }
                        else if (mod.ModuleName.Equals("server.dll"))
                        {
                            Offsets.ServerDll = (uint)mod.BaseAddress;
                            Console.WriteLine("module found. " + mod.ModuleName + " Adress: " + mod.BaseAddress);
                        }
                        else if (mod.ModuleName.Equals("materialsystem.dll"))
                        {
                            Offsets.MaterialsystemDll = (uint)mod.BaseAddress;
                            Console.WriteLine("module found. " + mod.ModuleName + " Adress: " + mod.BaseAddress);
                        }
                    }
                    
                    Console.Write("Process found. " + process.Handle);
                    ESP.Draw(process.MainWindowHandle);
                    break;
                }
            }
            Environment.Exit(0);
        }

        public static bool GetProcessesByName(string pName, out Process process)
        {
            Process[] pList = Process.GetProcessesByName(pName);
            process = pList.Length > 0 ? pList[0] : null;
            return process != null;
        }
        public static Process GetProcessesByName(string processName)
        {
            if (processName.IndexOf(".exe") != -1)
                processName = processName.Remove(processName.Length - 4);

            Process[] processesByName = Process.GetProcessesByName(processName);
            if (processesByName.Length == 0) return null;

            Process[] numArray = new Process[processesByName.Length];
            for (int i = 0; i < processesByName.Length; i++)
                numArray[i] = processesByName[i];

            return numArray[0];
        }
    }
}
