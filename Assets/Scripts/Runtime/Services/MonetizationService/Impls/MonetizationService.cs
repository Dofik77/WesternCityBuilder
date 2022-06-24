using System;

namespace Runtime.Services.MonetizationService.Impls
{
    public class MonetizationService : IMonetizationService
    {
        // private readonly MaxAdsManager _maxAdsManager;
        // private readonly PurchaseController _purchaseController;
        // private readonly PurchaseConfig _purchaseConfig;
        
        public MonetizationService()
        {
            // _maxAdsManager = GameObject.FindGameObjectWithTag("AdsManager").GetComponent<MaxAdsManager>();
            // _purchaseController = GameObject.FindGameObjectWithTag("PurchaseController").GetComponent<PurchaseController>();
            // _purchaseConfig = _purchaseController.PurchaseConfig;
        }

        public void TryToInAppPurchase(string purchaseKey, Action onPurchaseComplete = null)
        {
            // _purchaseController.onProcessPurchase = CheckPurchaseDefinition;
            // _purchaseController.YourProductBuy(purchaseKey);
            //
            // void CheckPurchaseDefinition(PurchaseEventArgs eventArgs)
            // {
            //     if (eventArgs.purchasedProduct.definition.id == purchaseKey)
            //         onPurchaseComplete?.Invoke();
            // }
        }

        public void ShowRewardVideo(string placement, Action afterShow = null)
        {
            // _maxAdsManager.ShowRewardVideo(placement, afterShow);
        }

        public void ShowBanner(string placement)
        {
            // _maxAdsManager.ShowBaner(placement);
        }

        public void ShowInterstitial(string placement)
        {
            // _maxAdsManager.ShowInter(placement);
        }

        public void RemoveAds()
        {
            // _maxAdsManager.RemoveAds();
        }

        public void SetAdsRemoved(bool value)
        {
            // _maxAdsManager.IsAdsRemoved = value;
        }

        public double GetAndroidCost(string key)
        {
            // return _purchaseConfig.Products.Get(key).Price;
            return 0d;
        }
    }
}