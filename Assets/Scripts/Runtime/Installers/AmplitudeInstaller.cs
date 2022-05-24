using UnityEngine;
using Zenject;

namespace Runtime.Installers
{
    public class AmplitudeInstaller : MonoInstaller
    {
        [SerializeField] private string apiKey;
        public override void InstallBindings()
        {
            var amplitude = Amplitude.Amplitude.Instance;
            amplitude.logging = true;
            amplitude.init(apiKey);
        }
    }
}