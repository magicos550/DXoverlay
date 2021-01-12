using System;

namespace Yato.DirectXOverlay
{
    class Offsets
    {
        public static IntPtr ProcessHandle;
        public static uint ClientDll;
        public static uint EngineDll;
        public static uint ServerDll;
        public static uint MaterialsystemDll;
        public const uint ClientState = 0x57B7EC;

        public const uint LocalPlayer = 0xA9BDDC;
        public const uint EntityBase = 0x4A78BA4;
        public const uint EntityBaseOffset = 0x10;
        public const uint vMatricOffset = 0x4A6A614; // ??? client.dll with angles

        public const uint T_Offset = 0x4D3B24;
        public const uint CT_Offset = 0x4E3A4C;

        public const uint Player_HP = 0xFC;
        public const uint Player_Armor = 0xB238;
        public const uint Player_State = 0x25B;
        public const uint Player_Team = 0xF0;
        public const uint Player_BoneMatrix = 0x2698;
        public const uint m_bSpotted = 0x939;
        public const uint m_bSpottedByMask = 0x97C;
        public const uint m_bDormant = 0xE9;

        public const uint Player_X = 0x134;
        public const uint Player_Y = 0x138;
        public const uint Player_Z = 0x13C;

        public const uint Player_vec_pitch = 0x26C;
        public const uint Player_vec_yaw = 0x270;
        public const uint Player_vec_roll = 0x274;

        public const uint PlayerTotal = 0x4EFFC0;

        public const uint RadarBase = 0x4EAD89C;
        public const uint RadarOffset = 0x140;
        public const uint RadarName = 0x38;
        public const uint RadarTeam = 0x58;
        public const uint RadarHP = 0x5C;
        public const uint RadarX = 0x60;
        public const uint RadarY = 0x64;
        public const uint RadarZ = 0x68;
    }
}
