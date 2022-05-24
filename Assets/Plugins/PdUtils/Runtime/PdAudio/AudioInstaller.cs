using System.Collections.Generic;
using PdUtils.PdAudio;
using UnityEngine;
using Zenject;

namespace Plugins.PdUtils.Runtime.PdAudio
{
	public class AudioInstaller : MonoInstaller
	{
		public PdAudioSources pdAudioSourcesPrefab;
		public override void InstallBindings()
		{
			var audioSources = Instantiate(pdAudioSourcesPrefab);
			Container.BindInterfacesTo<PdAudioSources>().FromInstance(audioSources).AsSingle().WhenInjectedInto<PdAudio>();
			Container.BindInterfacesAndSelfTo<PdAudio>().AsSingle().NonLazy();
			Container.BindInterfacesTo<PdAudioUiPlayables>().AsSingle().NonLazy();
		}
	}
}