using UnityEngine;

namespace PdUtils.PdAudio
{
    public interface IPdAudio
    {
        void PlayMusic(AudioClip track);
        
        void StopMusic();

        void PauseMusic();
        
        void UnpauseMusic();

        void PlayFx(string clip);
        
        void PlayUi(string clip);

        void SetMusicVolume(float val);
        void SetFxAndUiVolume(float val);
    }
}