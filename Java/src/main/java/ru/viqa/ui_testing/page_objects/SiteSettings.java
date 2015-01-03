package ru.viqa.ui_testing.page_objects;

import ru.viqa.ui_testing.common.interfaces.WebDriverTimeouts;

/**
 * Created by roman.i on 26.09.2014.
 */
public class SiteSettings {
    public WebDriverTimeouts timeouts = new WebDriverTimeouts();
    public int cashDropTimes = -1;
    public boolean useCache = true;
    public HighlightSettings demoSettings;

    public void dropCash() { cashDropTimes++;}
}
