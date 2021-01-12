using System;
using System.Windows.Forms;

namespace Yato.DirectXOverlay
{
    class AimAssist
    {
        private static float AimSpeed = 5;
        private static bool smooth = false;
        public static void Begin(float x, float y , int[] Res)
        {
            float ScreenCenterX = (Res[0] / 2);
            float ScreenCenterY = (Res[1] / 2);
            float TargetX = 0;
            float TargetY = 0;
            if (x != 0)
            {
                if (x > ScreenCenterX)
                {
                    TargetX = -(ScreenCenterX - x);
                    TargetX /= AimSpeed;
                    if (TargetX + ScreenCenterX > ScreenCenterX * 2) TargetX = 0;
                }

                if (x < ScreenCenterX)
                {
                    TargetX = x - ScreenCenterX;
                    TargetX /= AimSpeed;
                    if (TargetX + ScreenCenterX < 0) TargetX = 0;
                }
            }
            if (y != 0)
            {
                if (y > ScreenCenterY)
                {
                    TargetY = -(ScreenCenterY - y);
                    TargetY /= AimSpeed;
                    if (TargetY + ScreenCenterY > ScreenCenterY * 2) TargetY = 0;
                }

                if (y < ScreenCenterY)
                {
                    TargetY = y - ScreenCenterY;
                    TargetY /= AimSpeed;
                    if (TargetY + ScreenCenterY < 0) TargetY = 0;
                }
            }
            if (!smooth)
            {
                PInvoke.WinApi.mouse_event(0x0001, (int)TargetX, (int)TargetY, 0x0, 0x0);
                return;
            }
            TargetX /= 10;
            TargetY /= 10;
            if (Math.Abs(TargetX) < 1)
            {
                if (TargetX > 0)
                {
                    TargetX = 1;
                }
                if (TargetX < 0)
                {
                    TargetX = -1;
                }
            }
            if (Math.Abs(TargetY) < 1)
            {
                if (TargetY > 0)
                {
                    TargetY = 1;
                }
                if (TargetY < 0)
                {
                    TargetY = -1;
                }
            }
            PInvoke.WinApi.mouse_event(0x0001, (int)TargetX, (int)TargetY, 0x0, 0x0);
        }
    }
}
