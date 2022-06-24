using System;
using Runtime.Services.AnalyticsService.Impls;

namespace Runtime.Services.AnalyticsService
{
    public interface IAnalyticsService
    {
        void SendProgressionEvent(Enum status, string message1, string message2 = null);
        void SendDesignEvent(AnalyticsState state, string message1, string message2);
    }
}