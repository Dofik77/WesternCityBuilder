using UnityEngine;

namespace PdUtils.PdAudio
{

    public interface IPdAudioSources
    {
        AudioSource MusicAudioSource { get; }
        AudioSource UiAndFxAudioSource { get; }  
    }
    
    public class PdAudioSources : MonoBehaviour, IPdAudioSources
    {
        public AudioSource musicAudioSource;
        public AudioSource uiAndFxAudioSource;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public AudioSource MusicAudioSource => musicAudioSource;
        public AudioSource UiAndFxAudioSource => uiAndFxAudioSource;
    }
}