using System;

namespace PdUtils.AppVersion
{
    [Serializable]
    public struct Version : IComparable<Version>
    {
        public int major;
        public int minor;
        public int patch;

        public Version(int major, int minor, int patch)
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
        }

        public override string ToString()
        {
            return major + "." + minor + "." + patch;
        }

        public bool IsGreaterThanOrEqual(Version other)
        {
            if (major > other.major) return true;

            if (major == other.major && minor >= other.minor) return true;

            if (major == other.major && minor == other.minor &&
                patch >= other.patch)
                return true;

            return false;
        }

        public bool IsGreaterThan(Version other)
        {
            if (major > other.major) return true;

            if (major == other.major && minor > other.minor) return true;

            if (major == other.major && minor == other.minor &&
                patch > other.patch)
                return true;

            return false;
        }
        
        public bool IsLessThan(Version other)
        {
            if (major < other.major) return true;

            if (major == other.major && minor < other.minor) return true;

            if (major == other.major && minor == other.minor &&
                patch < other.patch)
                return true;

            return false;
        }
        
        public bool IsEqual(Version other)
        {
            return major == other.major && minor == other.minor &&
                   patch == other.patch;
        }

        public static Version FromString(string version)
        {
            var split = version.Split('.');
            var major = 0;
            var minor = 0;
            var patch = 0;
            var success = false;

            if (split.Length >= 1)
            {
                success = true;
                success &= int.TryParse(split[0], out major);
            }

            if (split.Length >= 2) success &= int.TryParse(split[1], out minor);

            if (split.Length >= 3) success &= int.TryParse(split[2], out patch);
            
            if(!success) 
                throw new ArgumentException("version " + version + " has wrong format");
            
            return new Version
            {
                major = major,
                minor = minor,
                patch = patch
            };
        }

        public int CompareTo(Version other)
        {
            if (IsEqual(other)) return 0;
            if (IsGreaterThan(other)) return 1;
            return -1;
        }
    }
}