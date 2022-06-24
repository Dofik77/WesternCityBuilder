using System;

namespace Runtime.Services.AnalyticsService.Impls
{
    public class AnalyticsService : IAnalyticsService
    {
        public void SendProgressionEvent(Enum status, string message1, string message2 = null)
        {
            // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, message1, message2);
        }
        
        public void SendDesignEvent(AnalyticsState state, string message1, string message2)
        {
            // GameAnalytics.NewDesignEvent(message1 + ":" + message2 + ":" + Enum.GetName(typeof(AnalyticsState), state));
        }
    }

    public enum AnalyticsState
    {
        Get,
        Spent
    }
    
    
}