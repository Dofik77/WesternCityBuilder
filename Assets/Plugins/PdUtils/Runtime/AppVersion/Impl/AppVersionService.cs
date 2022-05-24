using PdUtils.PlayerPrefs;
using UnityEngine;
using Zenject;

namespace PdUtils.AppVersion.Impl
{
	public class AppVersionService : IAppVersionService, IInitializable
	{
		private const string AppVersionKey = "AppVersion";
		
		private readonly IPlayerPrefsManager _playerPrefs;

		public AppVersionService(IPlayerPrefsManager playerPrefs)
		{
			_playerPrefs = playerPrefs;
		}
		
		public void Initialize()
		{
			_playerPrefs.SetValue(AppVersionKey, Application.version);
			_playerPrefs.Save();
		}

		public Version GetVersion()
		{
			return Version.FromString(Application.version);
		}
	}
}