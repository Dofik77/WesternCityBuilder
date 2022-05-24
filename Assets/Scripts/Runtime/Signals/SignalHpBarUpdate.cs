using UnityEngine;

namespace Runtime.Signals
{
    public struct SignalHpBarUpdate
    {
        public int Hp;
        public Vector2 Position;

        public SignalHpBarUpdate(int hp, Vector2 position)
        {
            Hp = hp;
            Position = position;
        }
    }
}