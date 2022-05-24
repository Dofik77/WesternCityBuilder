using DG.Tweening;

namespace TweenAni
{
    public static class DOTweenEaseExtensions
    {
        public static bool IsBack(this Ease ease)
        {
            return ease == Ease.InBack
                || ease == Ease.OutBack
                || ease == Ease.InOutBack;
        }

        public static bool IsFlash(this Ease ease)
        {
            return ease == Ease.Flash
                || ease == Ease.InFlash
                || ease == Ease.OutFlash
                || ease == Ease.InOutFlash;
        }

        public static bool IsElastic(this Ease ease)
        {
            return ease == Ease.InElastic
                || ease == Ease.OutElastic
                || ease == Ease.InOutElastic;
        }
    }
}