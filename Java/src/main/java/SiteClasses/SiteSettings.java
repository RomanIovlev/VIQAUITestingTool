package SiteClasses;

import Common.DefaultWebDriverTimeouts;
import Common.Interfaces.IWebDriverTimeouts;

/**
 * Created by roman.i on 26.09.2014.
 */
public class SiteSettings {
    public IWebDriverTimeouts WebDriverTimeouts = new DefaultWebDriverTimeouts();
    public int CashDropTimes = -1;
    public boolean UseCache = true;
    public HighlightSettings DemoSettings;
}
