using System;
using System.Numerics;

namespace Yato.DirectXOverlay
{
    class WorldToScreen
    {

        public static Vector3 WTS(Vector3 from, int[] res , float[] _viewMatrix)
        {
            Vector3 to = new Vector3();
            float w = 0.0f;

            to.X = _viewMatrix[0] * from.X + _viewMatrix[1] * from.Y + _viewMatrix[2] * from.Z + _viewMatrix[3];
            to.Y = _viewMatrix[4] * from.X + _viewMatrix[5] * from.Y + _viewMatrix[6] * from.Z + _viewMatrix[7];

            w = _viewMatrix[12] * from.X + _viewMatrix[13] * from.Y + _viewMatrix[14] * from.Z + _viewMatrix[15];

            
            if (w < 0.01f)
            {
                to.X = res[0] / 2;
                to.Y = res[1];
                return to;
            }
                
            
            
            to.X *= (1.0f / w);
            to.Y *= (1.0f / w);

            int width = res[0];
            int height = res[1];

            float x = width / 2;
            float y = height / 2;

            x += 0.5f * to.X * width + 0.5f;
            y -= 0.5f * to.Y * height + 0.5f;

            to.X = x;
            to.Y = y;

            //Console.WriteLine(to);

            return to;
        }

    }
}
