package ru.viqa.ui_testing.page_objects;

import ru.viqa.ui_testing.annotations.Page;
import ru.viqa.ui_testing.elements.baseClasses.VIElement;
import org.openqa.selenium.WebDriver;

import static ru.viqa.ui_testing.common.utils.StringUtils.*;
import static java.lang.String.format;
import static ru.viqa.ui_testing.page_objects.VISite.Logger;

/**
 * Created by roman.i on 26.09.2014.
 */
public class VIPage extends VIElement {
    private String url;
    public String getUrl() { return url; }
    public VIPage setUrl(String url) { this.url = getUrlValue(url, getSite()); return this; }
    public static String getUrlValue(String url, VISite site)
    {
        return (url == null || url.equals(""))
            ? site.Domain
            : (url.contains("http://") || url.contains("file:///"))
                ? url
                : site.Domain.replaceAll("/*$", "") + "/"+ url.replaceAll("^/*", "");
    }

    public String title;
    public boolean isHomePage;

    public PageCheckType urlCheckType;
    public PageCheckType titleCheckType;

    public void setEmptyPage()
    {
        if (isSiteSet())
            setUrl("");
        else
            url = "";
        title = "";
        urlCheckType = PageCheckType.NoCheck;
        titleCheckType = PageCheckType.NoCheck;
        isHomePage = false;
    }

    public VIPage() throws Exception {
        DefaultNameFunc = () -> format("Page with title: '%s', url: '%s'", title != null ? title : "", getUrl() != null ? getUrl() : "");
        setEmptyPage();
    }
    
    public VIPage(String name, String url, String title, VISite site) throws Exception { this.url = url; setName(name); this.title = title; setSite(site); }

    private Navigation navigation() {
        return getSite().navigate;
    }

    public VIPage open() throws Exception {
        Logger.event("Open page: " + getUrl());
        getWebDriver().navigate().to(getUrl());
        //verifyPage(true);
        navigation().processNewPage(this);
        openPageName = getName();
        return this;
    }

    public VIPage goBack() throws Exception {
        Logger.event("GoBack to previous page");
        getWebDriver().navigate().back();
        navigation().processGoBack();
        openPageName = "From page: " + getName();
        return this;
    }
    public VIPage goForward() throws Exception {
        Logger.event("GoForward to next page");
        getWebDriver().navigate().forward();
        navigation().processGoForward();
        openPageName = "From page: " + getName();
        return this;
    }

    public VIPage refreshPage() throws Exception {
        Logger.event("Refresh current page");
        getWebDriver().navigate().refresh();
        navigation().processRefreshPage();
        openPageName = getName();
        return this;
    }

    public boolean verifyPage() throws Exception {
        return verifyPage(false);
    }

    public boolean verifyPage(boolean throwError) throws Exception {
        return checkUrl(urlCheckType, throwError) && checkTitle(titleCheckType, throwError);
    }

    public boolean checkUrl() throws Exception {
        return checkUrl(urlCheckType, false);
    }
    public boolean checkUrl(PageCheckType checkType) throws Exception {
        return checkUrl(checkType, false);
    }
    public boolean checkUrl(PageCheckType checkType, boolean throwError) throws Exception {
        return checkPageAttribute(checkType, throwError, "url",
                getWebDriver().getCurrentUrl().toLowerCase().replaceAll("/*$", ""), getUrl().toLowerCase().replaceAll("/*$", ""));
    }

    public boolean checkTitle() throws Exception {
        return checkTitle(titleCheckType, false);
    }
    public boolean checkTitle(PageCheckType checkType) throws Exception {
        return checkTitle(checkType, false);
    }
    public boolean checkTitle(PageCheckType checkType, boolean throwError) throws Exception {
        return checkPageAttribute(checkType, throwError, "title",
                getWebDriver().getTitle(), title);
    }
    private boolean checkPageAttribute(PageCheckType checkType, boolean throwError, String checkWhat, String actual, String expected)
            throws Exception {
        if (checkType == PageCheckType.NoCheck) return true;
        if (expected == null || expected.equals(""))
            throw VISite.Alerting.throwError(format("Page '%s' %s is empty. Please set %s for this page", getName(), checkWhat, checkWhat));
        Logger.event(format("Check page '%s' %s %s '%s'", getName(), checkWhat,
                checkType == PageCheckType.Equal ? "equal to " : "contains", expected));
        boolean result =
                (checkType == PageCheckType.Equal)
                        ? actual.equals(expected)
                        : actual.contains(expected);
        if (result) return true;
        String errorMsg = format("Failed to check page %s '%s'." +
                LineBreak + "Actual:   '%s'" +
                LineBreak + "Expected: '%s'" +
                "CheckType: '%s'", checkWhat, getName(), actual, expected, checkType);
        if (throwError)
            throw VISite.Alerting.throwError(errorMsg);
        Logger.error(errorMsg);
        return false;
    }

    public void fillFromPageAttribute(Page pageAttr)
    {
        if (pageAttr == null) return;
        setUrl(pageAttr.url());
        title = pageAttr.title();
        urlCheckType = pageAttr.urlCheckType();
        titleCheckType = pageAttr.titleCheckType();
        isHomePage = pageAttr.isHomePage();
    }

    public void updatePageAttribute(Page pageAttr)
    {
        if (pageAttr == null) return;
        if (pageAttr.url() != null && !pageAttr.url().equals(""))
            setUrl(pageAttr.url());
        if (pageAttr.title() != null && !pageAttr.title().equals(""))
            title = pageAttr.title();
        if (pageAttr.urlCheckType() != PageCheckType.NotSet)
            urlCheckType = pageAttr.urlCheckType();
        if (pageAttr.titleCheckType() != PageCheckType.NotSet)
            titleCheckType = pageAttr.titleCheckType();
        if (pageAttr.isHomePage())
            isHomePage = true;
    }
}
