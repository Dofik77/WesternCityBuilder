namespace Runtime.Game.Ui.Objects.Purchase
{
    public class UiRewardedPurchaseEntity : UiPurchaseEntity
    {
        
        public string RewardedKey;
        
        public override void OnPurchase()
        {
            _monetizationService.ShowRewardVideo(RewardedKey, OnAfterPurchase);
        }
        
    }
}