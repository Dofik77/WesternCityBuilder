using System;

namespace Runtime.Services.MonetizationService
{
    public interface IMonetizationService
    {
        void TryToInAppPurchase(string purchaseKey, Action onPurchaseComplete = null);
        void ShowRewardVideo(string placement, Action afterShow = null);
        void ShowBanner(string placement);
        void ShowInterstitial(string placement);
        
        void RemoveAds();
        void SetAdsRemoved(bool value);

        double GetAndroidCost(string key);
    }
}