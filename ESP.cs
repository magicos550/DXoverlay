using System;
using System.Numerics;
using System.Windows.Forms;

namespace Yato.DirectXOverlay
{
    class ESP
    {
        public static void Draw(IntPtr parentWindowHandle)
        {
            var rendererOptions = new Direct2DRendererOptions()
            {
                AntiAliasing = true,
                Hwnd = IntPtr.Zero,
                MeasureFps = true,
                VSync = false
            };
            OverlayManager manager = new OverlayManager(parentWindowHandle, rendererOptions);
            int[] resolution = new int[2];
            float[] vMatrix = new float[16];
            var overlay = manager.Window;
            var d2d = manager.Graphics;
            var bg = d2d.CreateBrush(0, 0, 0, 100);

            var blackBrush = d2d.CreateBrush(0, 0, 0, 255);
            var redBrush = d2d.CreateBrush(255, 0, 0, 255);
            var greenBrush = d2d.CreateBrush(0, 255, 0, 255);
            var blueBrush = d2d.CreateBrush(0, 0, 255, 255);

            var font = d2d.CreateFont("Consolas", 22);
            uint LocalPlayer = Memory.ReadUInt((Offsets.ClientDll + Offsets.LocalPlayer));
            uint EntityBase = Memory.ReadUInt((Offsets.ClientDll + Offsets.EntityBase));
            float MinDist = float.MaxValue;
            int ClosestEnemy = 0;
            Program.PlayerList[] PlayerList = new Program.PlayerList[64];
            Program.AimPos AimPlayer = new Program.AimPos();
            while (true)
            {
                Program.LocalPlayer.Alive = Memory.ReadUInt(LocalPlayer + Offsets.Player_State);
                Program.LocalPlayer.Team = Memory.ReadUInt(LocalPlayer + Offsets.Player_Team);

                d2d.BeginScene();
                d2d.ClearScene();

                resolution[0] = d2d.Width;
                resolution[1] = d2d.Height;

                if (PInvoke.WinApi.GetAsyncKeyState(Keys.LMenu) != 0)
                {
                    AimAssist.Begin(WorldToScreen.WTS(PlayerList[ClosestEnemy].HeadPos, resolution, vMatrix).X, WorldToScreen.WTS(PlayerList[ClosestEnemy].HeadPos, resolution, vMatrix).Y, resolution);
                    MinDist = float.MaxValue;
                }

                if (Program.LocalPlayer.Alive == 0)
                {
                    Vector3 PlayerPos = new Vector3(
                    Memory.ReadFloat(LocalPlayer + Offsets.Player_X),
                    Memory.ReadFloat(LocalPlayer + Offsets.Player_Y),
                    Memory.ReadFloat(LocalPlayer + Offsets.Player_Z)
                    );

                    for (int i = 0; i < vMatrix.Length; i++)
                    {
                        vMatrix[i] = Memory.ReadFloat(Offsets.ClientDll + Offsets.vMatricOffset + (i * 0x4));
                    }
                    
                    for (int i = 0; i < PlayerList.Length; i++)
                    {
                        uint EnemyPlayerEntity = Memory.ReadUInt((Offsets.ClientDll + Offsets.EntityBase + (i * 0x10)));

                        PlayerList[i].HP = Memory.ReadUInt(EnemyPlayerEntity + Offsets.Player_HP);
                        PlayerList[i].Armor = Memory.ReadUInt(EnemyPlayerEntity + Offsets.Player_Armor);
                        PlayerList[i].Alive = Memory.ReadUInt(EnemyPlayerEntity + Offsets.Player_State);
                        PlayerList[i].Team = Memory.ReadUInt(EnemyPlayerEntity + Offsets.Player_Team);
                        PlayerList[i].Spoted = Memory.ReadUInt(EnemyPlayerEntity + Offsets.m_bSpottedByMask);
                        PlayerList[i].Dormant = Memory.ReadUInt(EnemyPlayerEntity + Offsets.m_bDormant);

                        PlayerList[i].Pos = new Vector3(
                        Memory.ReadFloat(EnemyPlayerEntity + Offsets.Player_X),
                        Memory.ReadFloat(EnemyPlayerEntity + Offsets.Player_Y),
                        Memory.ReadFloat(EnemyPlayerEntity + Offsets.Player_Z)
                        );
                        
                        if (PlayerList[i].Alive == 0 && PlayerList[i].Team != Program.LocalPlayer.Team && PlayerList[i].HP > 0 && PlayerList[i].Dormant == 0)
                        {
                            uint bPointer = Memory.ReadUInt(EnemyPlayerEntity + Offsets.Player_BoneMatrix);
                            PlayerList[i].distance = (float)Math.Sqrt(Math.Pow(PlayerPos.X - PlayerList[i].Pos.X, 2.0) + Math.Pow(PlayerPos.Y - PlayerList[i].Pos.Y, 2.0) + Math.Pow(PlayerPos.Z - PlayerList[i].Pos.Z, 2.0));

                            PlayerList[i].HeadPos= new Vector3(
                            Memory.ReadFloat(bPointer + 0x0c + (8 * 0x30)),
                            Memory.ReadFloat(bPointer + 0x1c + (8 * 0x30)),
                            Memory.ReadFloat(bPointer + 0x2c + (8 * 0x30))
                            );
                            if (PlayerList[i].distance < MinDist)
                            {
                                ClosestEnemy = i;
                                MinDist = PlayerList[i].distance;
                            }

                            float Box_Width = (WorldToScreen.WTS(PlayerList[i].Pos, resolution, vMatrix).Y - WorldToScreen.WTS(PlayerList[i].HeadPos, resolution, vMatrix).Y) / 2;
                            float Box_Height = WorldToScreen.WTS(PlayerList[i].Pos, resolution, vMatrix).Y - WorldToScreen.WTS(PlayerList[i].HeadPos, resolution, vMatrix).Y + (Box_Width / 3);
                            float Box_X = WorldToScreen.WTS(PlayerList[i].HeadPos, resolution, vMatrix).X - (Box_Width / 2);
                            float Box_Y = WorldToScreen.WTS(PlayerList[i].HeadPos, resolution, vMatrix).Y - Box_Width / 3;

                            //d2d.DrawTextWithBackground(PlayerList[i].HeadPos + " : " + PlayerList[i].distance, 200, (i * 20), 12, font, redBrush, blackBrush);
                            d2d.DrawLine(d2d.Width / 2, d2d.Height, WorldToScreen.WTS(PlayerList[i].Pos, resolution, vMatrix).X, WorldToScreen.WTS(PlayerList[i].Pos, resolution, vMatrix).Y, 1, PlayerList[i].Spoted == 1 ? redBrush : greenBrush);

                            d2d.BorderedRectangle(Box_X, Box_Y, Box_Width, Box_Height, 1, PlayerList[i].Spoted == 1 ? redBrush : greenBrush, PlayerList[i].Spoted == 1 ? redBrush : greenBrush);

                            d2d.BorderedRectangle(WorldToScreen.WTS(PlayerList[i].HeadPos, resolution, vMatrix).X - 2, WorldToScreen.WTS(PlayerList[i].HeadPos, resolution, vMatrix).Y - 2, 5, 5, 1, PlayerList[i].Spoted == 1 ? redBrush : greenBrush, PlayerList[i].Spoted == 1 ? redBrush : greenBrush);

                            d2d.DrawHorizontalBar(PlayerList[i].HP, Box_X - 5, Box_Y, 2, Box_Height, 2, greenBrush, greenBrush);

                            d2d.DrawHorizontalBar(PlayerList[i].Armor, Box_X - 10, Box_Y, 2, Box_Height, 2, blueBrush, blueBrush);
                        }

                    }
                }

                d2d.EndScene();
            }
        }
    }
}
