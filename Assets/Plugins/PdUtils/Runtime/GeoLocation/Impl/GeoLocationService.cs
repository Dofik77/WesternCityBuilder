using System;
using Newtonsoft.Json;
using PdUtils.DateTimeService;
using PdUtils.PlayerPrefs;
using PdUtils.Web;
using UnityEngine;

namespace PdUtils.GeoLocation.Impl
{
    public class GeoLocationService : IGeoLocationService
    {
        private const string GeoLocUrlApi = "http://www.geoplugin.net/json.gp";
        public const string GEO_LOCATION_KEY = "GeoLoc";

        private readonly IPlayerPrefsManager _playerPrefsManager;
        private readonly IWebRequester<LocationVo> _locationWebRequester;
        private readonly IDateTimeService _dateTimeService;
        private readonly long _hoursUpdateInterval;

        public GeoLocationService(long hoursUpdateInterval, IPlayerPrefsManager playerPrefsManager, 
            IWebRequester<LocationVo> locationWebRequester, IDateTimeService dateTimeService)
        {
            _playerPrefsManager = playerPrefsManager;
            _locationWebRequester = locationWebRequester;
            _dateTimeService = dateTimeService;
            _hoursUpdateInterval = hoursUpdateInterval;
        }

        public void RetrieveLocation(Action<LocationVo> callback)
        {
            var nowTimestamp = _dateTimeService.UtcNow.ToUnixTimeSeconds();
            if (_playerPrefsManager.HasKey(GEO_LOCATION_KEY))
            {
                var locationJson = _playerPrefsManager.GetValue<string>(GEO_LOCATION_KEY);
                var location = JsonConvert.DeserializeObject<LocationVo>(locationJson);
                
                var updateRequired = nowTimestamp - location.TimestampSec > _hoursUpdateInterval * 3600;
                if (updateRequired)
                {
                    RetrieveAndStoreLocation(callback);
                }
                else
                {
                    callback?.Invoke(location);
                }
            }
            else
            {
                RetrieveAndStoreLocation(callback);   
            }
        }

        private void RetrieveAndStoreLocation(Action<LocationVo> callback)
        {
            _locationWebRequester.Get(GeoLocUrlApi, locResponse =>
            {
                if (locResponse.IsSuccess)
                {
                    locResponse.Value.TimestampSec = _dateTimeService.UtcNow.ToUnixTimeSeconds();
                    var locationJson = JsonConvert.SerializeObject(locResponse.Value);
                    _playerPrefsManager.SetValue(GEO_LOCATION_KEY, locationJson);
                    _playerPrefsManager.Save();   
                    callback?.Invoke(locResponse.Value);
                }
                else
                {
                    Debug.LogError("[GeoLocationService] Retrieve location: " + locResponse.Error);
                }
            });
        }
    }
}