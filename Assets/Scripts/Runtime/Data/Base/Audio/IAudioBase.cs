using UnityEngine;

namespace Runtime.Data.Base.Audio
{
    public interface IAudioBase
    {
        AudioClip Get(string key);
    }
}