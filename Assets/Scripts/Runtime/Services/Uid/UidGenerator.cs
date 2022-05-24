using System.Collections.Generic;

namespace Runtime.Services.Uid
{
    public static class UidGenerator
    {
        private static readonly HashSet<PdUtils.Uid> HashSet = new HashSet<PdUtils.Uid>();
        private static int _current;

        public static void Clear()
        {
            _current = 0;
            HashSet.Clear();
        }

        private static int NextUid
        {
            get
            {
                if (_current == int.MaxValue)
                    _current = 0;
                return _current++;
            }
        }

        public static PdUtils.Uid Next()
        {
            PdUtils.Uid uid;
            do
            {
                uid = (PdUtils.Uid) NextUid;
            } while (HashSet.Contains(uid));

            HashSet.Add(uid);
            return uid;
        }

        public static void Reserve(PdUtils.Uid uid) => HashSet.Add(uid);

        public static void Remove(PdUtils.Uid uid)
        {
            HashSet.Remove(uid);
        }
    }
}