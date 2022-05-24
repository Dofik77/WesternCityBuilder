using Plugins.Vibration;

namespace Runtime.Services.VibrationService.Impl
{
    public class VibrationService : IVibrationService
    {
        public void Vibrate(long mlsec)
        {
            Vibration.Vibrate(mlsec);
        }
    }
}