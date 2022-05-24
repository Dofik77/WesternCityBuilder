using System;
using System.Collections.Generic;
using System.Reflection;
using PdUtils.PdAudio.Attributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using Zenject;

namespace PdUtils.PdAudio
{
	public class PdAudioUiPlayables : IInitializable
	{
		private readonly IList<IPdUiSoundPlayable> _soundPlayables;
		private readonly IPdAudio _pdAudio;


		public PdAudioUiPlayables(IList<IPdUiSoundPlayable> clipPlayables, IPdAudio pdAudio)
		{
			_soundPlayables = clipPlayables;
			_pdAudio = pdAudio;
		}

		public void Initialize()
		{
			foreach (var soundPlayable in _soundPlayables)
			{
				ProcessSoundPlayable(soundPlayable);
			}
		}

		private void ProcessSoundPlayable(IPdUiSoundPlayable uiSoundPlayable)
		{
			var fields = uiSoundPlayable.GetType().GetFields();
			foreach (var fieldInfo in fields)
			{
				TryProcessUiSound(fieldInfo, uiSoundPlayable);
			}
		}

		private void TryProcessUiSound(FieldInfo fieldInfo, IPdUiSoundPlayable uiSoundPlayable)
		{
			var attr = fieldInfo.GetCustomAttribute<UiSoundAttribute>();
			if (attr == null) return;
			var selectable = fieldInfo.GetValue(uiSoundPlayable) as Selectable;

			if (selectable == null)
			{
				throw new ArgumentException(
					"Ui sound attribute can be applied only to UI.Button, UI.Toggle, sound playable: " +
					uiSoundPlayable.GetType().Name);
			}

			selectable.OnPointerClickAsObservable()
				.Subscribe(_ => _pdAudio.PlayUi(attr.Clip))
				.AddTo(selectable);
		}
	}
}