using System;

namespace PdUtils.GeoLocation
{
    public interface IGeoLocationService
    {
        void RetrieveLocation(Action<LocationVo> callback);
    }
}