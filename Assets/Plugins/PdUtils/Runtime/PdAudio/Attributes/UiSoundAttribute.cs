using System;

namespace PdUtils.PdAudio.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class UiSoundAttribute : Attribute
    {
        public string Clip { get; }

        public UiSoundAttribute(string clip)
        {
            Clip = clip;
        }
    }
}