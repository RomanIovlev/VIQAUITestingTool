package ru.viqa.ui_testing.elements.baseClasses;

import ru.viqa.ui_testing.page_objects.*;
import ru.viqa.ui_testing.elements.interfaces.*;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import java.util.Set;

import static ru.viqa.ui_testing.common.utils.LinqUtils.last;

/**
 * Created by 12345 on 01.10.2014.
 */
public class Clickable extends VIElement implements IClickable {

    public String clickOpensPage;

    public Clickable() throws Exception { super(); }
    public Clickable(String name) throws Exception { super(name); }
    public Clickable(String name, String cssSelector) throws Exception  { super(name, cssSelector); }
    public Clickable(String name, By byLocator) throws Exception { super(name, byLocator); }
    public Clickable(By byLocator) throws Exception { super(byLocator);}
    public Clickable(String name, WebElement webElement) throws Exception { super(name, webElement);}
    public Clickable(WebElement webElement) throws Exception  { super(webElement);}

    protected void clickAction() throws Exception { getUniqueWebElement().click(); }

    public final void click() throws Exception {
        clickOpenPage(clickOpensPage);
    }
    public final void clickOnInvisible() throws Exception {
        checkVisibility = false;
        clickOpenPage(clickOpensPage);
    }

    public final void clickOpenPage(String openPageName) throws Exception {
        doVIAction("Click on element", () -> {
            SmartClickAction();
            Set<String> windowHandles = getWebDriver().getWindowHandles();
            if (windowHandles.size() > 1) {
                String windowHandle = last(windowHandles);
                getSite().navigate.WindowHandle = windowHandle;
                getWebDriver().switchTo().window(windowHandle);
            }
            VIElement.openPageName = openPageName;
            getSite().siteSettings.dropCash();
        });
    }

    private void SmartClickAction() throws Exception {
        boolean clicked = getTimer().wait(() -> {
            clickAction();
            VISite.Logger.event(getDefaultLogMessage("Done click"));
            return true;
        });
        if (!clicked)
            throw VISite.Alerting.throwError(getDefaultLogMessage("Failed to click element"));
    }
}
