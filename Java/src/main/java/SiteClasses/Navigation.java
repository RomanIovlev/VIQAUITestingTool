package SiteClasses;

import org.openqa.selenium.WebDriver;

import java.util.*;

/**
 * Created by roman.i on 25.09.2014.
 */
public class Navigation {

    private VISite _site;
    private WebDriver getWebDriver() throws Exception { return _site.getWebDriver(); }

    public Navigation(VISite site)
    {
        PagesHistory = new ArrayList<>();
        _currentPageNum = -1;
        _site = site;
    }

    public ArrayList<VIPage> PagesHistory;
    private int _currentPageNum;
    public VIPage getCurrentPage() { return PagesHistory.toArray(new VIPage[PagesHistory.size()])[_currentPageNum]; }
    public String WindowHandle;

    public void openPage(String uri) throws Exception {
        openPage(new VIPage("Page with url " + VIPage.getUrlValue(uri, _site), uri, "", _site));
    }

    public void openPage(VIPage page) throws Exception {
        VISite.Logger.Event("Open page: " + page.getUrl());
        WebDriver driver = getWebDriver();
        driver.navigate().to(page.getUrl());
        WindowHandle = driver.getWindowHandle();
        _site.SiteSettings.CashDropTimes++;
        PagesHistory.add(page);
        _currentPageNum++;
    }

    public void GoBack() throws Exception {
        VISite.Logger.Event("GoBack to previous page");
        _site.getWebDriver().navigate().back();
        _site.SiteSettings.CashDropTimes++;
        _currentPageNum--;
    }
    public void GoForward() throws Exception {
        VISite.Logger.Event("GoForward to next page");
        _site.getWebDriver().navigate().forward();
        _site.SiteSettings.CashDropTimes++;
        _currentPageNum++;
    }

    public void RefreshPage() throws Exception {
        VISite.Logger.Event("Refresh current page");
        _site.getWebDriver().navigate().refresh();
        _site.SiteSettings.CashDropTimes++;
    }
}
