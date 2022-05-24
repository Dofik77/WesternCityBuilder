using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Game.Utils
{
    public static class LevelUtils
    {
        public static Vector3 CalculateRandomPosition(Vector3 pos1, Vector3 pos2)
        {
            return new Vector3(
                Random.Range(pos1.x, pos2.x),
                0,
                Random.Range(pos1.z, pos2.z)
            );
        }

        public static Vector3 RandomPositionFromCircleArea(this Vector3 center, float radius, float angle)
        {
            return new Vector3(
                (float)Math.Cos(angle) * radius + center.x,
                    center.y,
                (float)Math.Sin(angle) * radius + center.z
                );
        }

        public static bool ValidatePoint(this Vector3 position, Vector3 pos1, Vector3 pos2)
        {
            return pos1.x < position.x && position.x < pos2.x && pos1.z > position.z && position.z > pos2.z;
        }
    }
}