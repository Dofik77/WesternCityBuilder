using System;
using UnityEngine;

namespace Runtime.Data.Base.Audio.Impl
{
    [CreateAssetMenu(menuName = "Bases/AudioBase", fileName = "AudioBase")]
    public class AudioBase : ScriptableObject, IAudioBase
    {
        [SerializeField] private Clip[] clips;

        public AudioClip Get(string key)
        {
            for (var i = 0; i < clips.Length; i++)
            {
                var clip = clips[i];
                if (clip.Name == key)
                    return clip.AudioClip;
            }

            throw new Exception("[AudioBase] Can't find AudioClip with name: " + key);
        }

        [Serializable]
        public class Clip
        {
            public string Name;
            public AudioClip AudioClip;
        }
    }
}