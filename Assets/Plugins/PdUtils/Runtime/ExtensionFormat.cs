namespace PdUtils
{
    public static class ExtensionFormat
    {
        public static string TimeLeftFormat(this int seconds)
        {
            var mins = (int) (seconds / 60f);
            var secs = seconds % 60;
            return $"{mins}:{secs:00}";
        }
    }
}