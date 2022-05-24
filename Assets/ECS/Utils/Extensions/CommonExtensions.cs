using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace ECS.Utils.Extensions
{
    public static class CommonExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            var provider = new RNGCryptoServiceProvider();
            var n = list.Count;
            while (n > 1)
            {
                var box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (byte.MaxValue / n)));
                var k = (box[0] % n);
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        
        public static float Remap (this float value, float from1, float to1, float from2, float to2)
            => (value - from1) / (to1 - from1) * (to2 - from2) + from2;

        public static float Remap01(this float value, float max) => value.Remap(0, max, 0, 1);
        public static float Remap01(this int value, float max)
        {
            var toFloat = (float) value;
            return toFloat.Remap(0, max, 0, 1);
        }

        public static Vector2 GetDirection(this Vector2 value)
        {
            return new Vector2(value.x > 0 ? 1 : value.x < 0 ? -1 : 0, value.y > 0 ? 1 : value.y < 0 ? -1 : 0);
        }
    }
}