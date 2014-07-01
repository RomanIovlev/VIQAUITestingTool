namespace VIQA.SiteClasses
{
    public class HighlightSettings
    {
        public readonly string BgColor;
        public readonly string FrameColor;
        public readonly int TimeoutInSec;

        public HighlightSettings(string bgColor = "yellow", string frameColor = "red", int timeoutInSec = 1)
        {
            BgColor = bgColor;
            FrameColor = frameColor;
            TimeoutInSec = timeoutInSec;
        }
    }
}
