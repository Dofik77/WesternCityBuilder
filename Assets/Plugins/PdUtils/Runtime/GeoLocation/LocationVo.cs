using Newtonsoft.Json;

namespace PdUtils.GeoLocation
{
    public class LocationVo
    {
        [JsonProperty("geoplugin_request")]
        public string Ip;
        [JsonProperty("geoplugin_status")]
        public int Status;
        [JsonProperty("geoplugin_city")]
        public string City;
        [JsonProperty("geoplugin_region")]
        public string Region;
        [JsonProperty("geoplugin_regionCode")]
        public string RegionCode;
        [JsonProperty("geoplugin_regionName")]
        public string RegionName;
        [JsonProperty("geoplugin_areaCode")]
        public string AreaCode;
        [JsonProperty("geoplugin_dmaCode")]
        public string DmaCode;
        [JsonProperty("geoplugin_countryCode")]
        public string CountryCode;
        [JsonProperty("geoplugin_countryName")]
        public string CountryName;
        [JsonProperty("geoplugin_inEU")]
        public int InEu;
        [JsonProperty("geoplugin_continentCode")]
        public string ContinentCode;
        [JsonProperty("geoplugin_continentName")]
        public string ContinentName;
        [JsonProperty("geoplugin_latitude")]
        public string Latitude;
        [JsonProperty("geoplugin_longitude")]
        public string Longitude;
        [JsonProperty("geoplugin_locationAccuracyRadius")]
        public string LocationAccuracyRadius;
        [JsonProperty("geoplugin_timezone")]
        public string Timezone;
        [JsonProperty("ts")]
        public long TimestampSec;

        protected bool Equals(LocationVo other)
        {
            return Ip == other.Ip && Status == other.Status && City == other.City && Region == other.Region && RegionCode == other.RegionCode && RegionName == other.RegionName && AreaCode == other.AreaCode && DmaCode == other.DmaCode && CountryCode == other.CountryCode && CountryName == other.CountryName && InEu == other.InEu && ContinentCode == other.ContinentCode && ContinentName == other.ContinentName && Latitude == other.Latitude && Longitude == other.Longitude && LocationAccuracyRadius == other.LocationAccuracyRadius && Timezone == other.Timezone && TimestampSec == other.TimestampSec;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LocationVo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Ip != null ? Ip.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Status;
                hashCode = (hashCode * 397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Region != null ? Region.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RegionCode != null ? RegionCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RegionName != null ? RegionName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AreaCode != null ? AreaCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DmaCode != null ? DmaCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CountryCode != null ? CountryCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CountryName != null ? CountryName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ InEu;
                hashCode = (hashCode * 397) ^ (ContinentCode != null ? ContinentCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ContinentName != null ? ContinentName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Latitude != null ? Latitude.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Longitude != null ? Longitude.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LocationAccuracyRadius != null ? LocationAccuracyRadius.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Timezone != null ? Timezone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ TimestampSec.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Ip)}: {Ip}, {nameof(Status)}: {Status}, {nameof(City)}: {City}, {nameof(Region)}: {Region}, {nameof(RegionCode)}: {RegionCode}, {nameof(RegionName)}: {RegionName}, {nameof(AreaCode)}: {AreaCode}, {nameof(DmaCode)}: {DmaCode}, {nameof(CountryCode)}: {CountryCode}, {nameof(CountryName)}: {CountryName}, {nameof(InEu)}: {InEu}, {nameof(ContinentCode)}: {ContinentCode}, {nameof(ContinentName)}: {ContinentName}, {nameof(Latitude)}: {Latitude}, {nameof(Longitude)}: {Longitude}, {nameof(LocationAccuracyRadius)}: {LocationAccuracyRadius}, {nameof(Timezone)}: {Timezone}, {nameof(TimestampSec)}: {TimestampSec}";
        }
    }
}