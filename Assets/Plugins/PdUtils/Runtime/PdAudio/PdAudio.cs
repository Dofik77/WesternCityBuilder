using System;
using System.Collections.Generic;
using PdUtils.PdAudio;
using PdUtils.PlayerPrefs;
using UniRx;
using UnityEngine;
using Zenject;

namespace Plugins.PdUtils.Runtime.PdAudio
{
	public class PdAudio : IPdAudio, IPdAudioSettings, IInitializable, IDisposable
	{
		private readonly IPlayerPrefsManager _playerPrefsManager;
		private readonly IPdAudioSources _pdAudioSources;

		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		private readonly IDictionary<string, AudioClip> _fullPathToClipMap = new Dictionary<string, AudioClip>();


		private bool? _musicEnabled;
		private bool? _soundFxEnabled;


		public PdAudio(
			IPlayerPrefsManager playerPrefsManager,
			IPdAudioSources pdAudioSources
			
		)
		{
			_playerPrefsManager = playerPrefsManager;
			_pdAudioSources = pdAudioSources;
		}

		public void Initialize()
		{
			_pdAudioSources.UiAndFxAudioSource.ignoreListenerPause = true;
		}

		public void Dispose()
		{
			_disposables?.Dispose();
		}

		public void PlayMusic(AudioClip track)
		{
			if (!MusicEnabled)
				return;

			_pdAudioSources.MusicAudioSource.clip = track;
			_pdAudioSources.MusicAudioSource.Play();
		}

		public void StopMusic()
		{
			_pdAudioSources.MusicAudioSource.Stop();
		}

		public void PauseMusic()
		{
			AudioListener.pause = true;
		}

		public void UnpauseMusic()
		{
			AudioListener.pause = false;
		}

		public void PlayFx(string clip)
		{
			if (!SoundFxEnabled)
				return;

			var audioClip = GetAudioClipByPathAndName("Sound/Fx", clip);
			_pdAudioSources.UiAndFxAudioSource.PlayOneShot(audioClip);
		}

		public void PlayUi(string clip)
		{
			if (!SoundFxEnabled)
				return;

			var audioClip = GetAudioClipByPathAndName("Sound/Ui", clip);
			_pdAudioSources.UiAndFxAudioSource.PlayOneShot(audioClip);
		}

		public void SetMusicVolume(float val) => _pdAudioSources.MusicAudioSource.volume = val;

		public void SetFxAndUiVolume(float val) => _pdAudioSources.UiAndFxAudioSource.volume = val;

		public bool MusicEnabled
		{
			set
			{
				_playerPrefsManager.SetValue(PlayerPrefsKeys.MusicOn.Value(), value);
				_musicEnabled = value;
			}
			get
			{
				if (!_musicEnabled.HasValue)
					_musicEnabled = _playerPrefsManager.GetValue(PlayerPrefsKeys.MusicOn.Value(), true);
				return _musicEnabled.Value;
			}
		}

		public bool SoundFxEnabled
		{
			set
			{
				_playerPrefsManager.SetValue(PlayerPrefsKeys.SoundFxOn.Value(), value);
				_soundFxEnabled = value;
			}
			get
			{
				if (!_soundFxEnabled.HasValue)
					_soundFxEnabled = _playerPrefsManager.GetValue(PlayerPrefsKeys.SoundFxOn.Value(), true);
				return _soundFxEnabled.Value;
			}
		}
		
		// =====================================================================================

		private AudioClip GetAudioClipByPathAndName(string path, string name)
		{
			var fullPath = path + "/" + name;
			if (_fullPathToClipMap.ContainsKey(fullPath))
				return _fullPathToClipMap[fullPath];

			var audioClip = Resources.Load<AudioClip>(fullPath);
			if (audioClip == null)
				throw new ArgumentException("Ui clip with name " + name + " in path " + path + " does not exist");

			_fullPathToClipMap.Add(fullPath, audioClip);
			return audioClip;
		}
	}
}