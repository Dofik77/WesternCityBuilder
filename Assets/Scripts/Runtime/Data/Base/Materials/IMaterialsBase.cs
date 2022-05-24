using UnityEngine;

namespace Runtime.Data.Base.Materials
{
    public interface IMaterialsBase
    {
        Material Get(string name);
    }
}