package SiteClasses;

import VIAnnotations.DemoSettings;

/**
 * Created by roman.i on 28.09.2014.
 */
public class HighlightSettings {
    public String BgColor = "yellow";
    public String FrameColor = "red";
    public int TimeoutInSec = 2;

    public HighlightSettings() {}
    public HighlightSettings(String bgColor, String frameColor, int timeoutInSec)
    {
        BgColor = bgColor;
        FrameColor = frameColor;
        TimeoutInSec = timeoutInSec;
    }
    public HighlightSettings(DemoSettings demoSettings)
    {
        if (demoSettings == null) return;
        if (demoSettings.bgColor() != null && !demoSettings.bgColor().equals(""))
            BgColor = demoSettings.bgColor();
        if (demoSettings.frameColor() != null && !demoSettings.frameColor().equals(""))
            FrameColor = demoSettings.frameColor();
        TimeoutInSec = demoSettings.timeoutInSec();
    }
    public HighlightSettings setBgColor(String bgColor) { BgColor = bgColor; return this; }
    public HighlightSettings setFrameColor(String frameColor) { FrameColor = frameColor; return this; }
    public HighlightSettings setTimeoutInSec(int timeoutInSec) { TimeoutInSec = timeoutInSec; return this; }
}
