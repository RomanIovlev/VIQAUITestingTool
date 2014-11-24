package VIElements.SimpleElements;

import SiteClasses.*;
import VIElements.BaseClasses.*;
import VIElements.Interfaces.*;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import java.util.Set;

import static Common.Utils.LinqUtils.last;

/**
 * Created by 12345 on 01.10.2014.
 */
public class Clickable extends VIElement implements IClickable {

    public Clickable() throws Exception { super(); TypeName = "Text"; }
    public Clickable(String name) throws Exception { super(name); }
    public Clickable(String name, String cssSelector) throws Exception  { super(name, cssSelector); }
    public Clickable(String name, By byLocator) throws Exception { super(name, byLocator); }
    public Clickable(By byLocator) throws Exception { super(byLocator);}
    public Clickable(String name, WebElement webElement) throws Exception { super(name, webElement);}
    public Clickable(WebElement webElement) throws Exception  { super(webElement);}

    public String ClickLoadsPage;
    protected void clickAction() throws Exception { getUniqueWebElement().click(); }

    public final void click() throws Exception {
        doVIAction("Click", () -> {
            SmartClickAction();
            Set<String> windowHandles = getWebDriver().getWindowHandles();
            if (windowHandles.size() > 1) {
                String windowHandle = last(windowHandles);
                getSite().Navigate.WindowHandle = windowHandle;
                getWebDriver().switchTo().window(windowHandle);
            }
            if (ClickLoadsPage != null && !ClickLoadsPage.equals("")) return;
            OpenPageName = ClickLoadsPage;
            getSite().SiteSettings.CashDropTimes ++;
        });
    }

    private void SmartClickAction() throws Exception {
        boolean clicked = getTimer().wait(() -> {
            clickAction();
            VISite.Logger.Event(getDefaultLogMessage("Done click"));
            return true;
        });
        if (!clicked)
            throw VISite.Alerting.ThrowError(getDefaultLogMessage("Failed to click element"));

    }
}
