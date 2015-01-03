package ru.viqa.ui_testing.page_objects;

import ru.viqa.ui_testing.annotations.Page;
import ru.viqa.ui_testing.elements.baseClasses.VIElement;
import org.openqa.selenium.WebDriver;

import static ru.viqa.ui_testing.common.utils.StringUtils.*;
import static java.lang.String.format;

/**
 * Created by roman.i on 26.09.2014.
 */
public class VIPage extends VIElement {
    private String Url;
    public String getUrl() { return Url; }
    public VIPage setUrl(String url) { Url = getUrlValue(url, getSite()); return this; }
    public static String getUrlValue(String url, VISite site)
    {
        return (url == null || url.equals(""))
            ? site.Domain
            : (url.contains("http://") || url.contains("file:///"))
                ? url
                : site.Domain.replaceAll("/*$", "") + "/"+ url.replaceAll("^/*", "");
    }

    public String Title;
    public boolean IsHomePage;

    public PageCheckType UrlCheckType;
    public PageCheckType TitleCheckType;

    public void setEmptyPage()
    {
        if (isSiteSet())
            setUrl("");
        else
            Url = "";
        Title = "";
        UrlCheckType = PageCheckType.NoCheck;
        TitleCheckType = PageCheckType.NoCheck;
        IsHomePage = false;
    }

    public VIPage() throws Exception {
        DefaultNameFunc = () -> format("Page with Title: '%s', Url: '%s'", Title != null ? Title : "", getUrl() != null ? getUrl() : "");
        setEmptyPage();
    }
    
    public VIPage(String name, String url, String title, VISite site) throws Exception { Url = url; setName(name); Title = title; setSite(site); }

    private Navigation navigation() {
        return getSite().Navigate;
    }

    public void open() throws Exception {
        VISite.Logger.Event("Open page: " + getUrl());
        getWebDriver().navigate().to(getUrl());
        verifyPage(true);
        navigation().processNewPage(this);
    }

    public void goBack() throws Exception {
        VISite.Logger.Event("GoBack to previous page");
        getWebDriver().navigate().back();
        navigation().processGoBack();
    }
    public void goForward() throws Exception {
        VISite.Logger.Event("GoForward to next page");
        getWebDriver().navigate().forward();
        navigation().processGoForward();
    }

    public void refreshPage() throws Exception {
        VISite.Logger.Event("Refresh current page");
        getWebDriver().navigate().refresh();
        navigation().processRefreshPage();
    }

    public boolean verifyPage() throws Exception {
        return verifyPage(false);
    }

    public boolean verifyPage(boolean throwError) throws Exception {
        return checkUrl(UrlCheckType, throwError) && checkTitle(TitleCheckType, throwError);
    }

    public boolean checkUrl() throws Exception {
        return checkUrl(UrlCheckType, false);
    }
    public boolean checkUrl(PageCheckType checkType) throws Exception {
        return checkUrl(checkType, false);
    }
    public boolean checkUrl(PageCheckType checkType, boolean throwError) throws Exception {
        return checkPageAttribute(checkType, throwError, "url",
                getWebDriver().getCurrentUrl().toLowerCase().replaceAll("/*$", ""), getUrl().toLowerCase().replaceAll("/*$", ""));
    }

    public boolean checkTitle(PageCheckType checkType) throws Exception {
        return checkTitle(checkType, false);
    }
    public boolean checkTitle(PageCheckType checkType, boolean throwError) throws Exception {
        return checkPageAttribute(checkType, throwError, "title",
                getWebDriver().getTitle(), Title);
    }
    private boolean checkPageAttribute(PageCheckType checkType, boolean throwError, String checkWhat, String actual, String expected)
            throws Exception {
        if (checkType == PageCheckType.NoCheck) return true;
        if (expected == null || expected.equals(""))
            throw VISite.Alerting.throwError(format("Page '%s' %s is empty. Please set %s for this page", getName(), checkWhat, checkWhat));
        VISite.Logger.Event(format("Check page '%s' %s %s '%s'", getName(), checkWhat,
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
        VISite.Logger.Error(errorMsg);
        return false;
    }

    public Navigation Navigate() { return getSite().Navigate; }

    public WebDriver getWebDriver() throws Exception { return getSite().getWebDriver(); }

    public void fillFromPageAttribute(Page pageAttr)
    {
        if (pageAttr == null) return;
        Url = pageAttr.url();
        Title = pageAttr.title();
        UrlCheckType = pageAttr.urlCheckType();
        TitleCheckType = pageAttr.titleCheckType();
        IsHomePage = pageAttr.isHomePage();
    }

    public void updatePageAttribute(Page pageAttr)
    {
        if (pageAttr == null) return;
        if (pageAttr.url() != null && !pageAttr.url().equals(""))
            Url = pageAttr.url();
        if (pageAttr.title() != null && !pageAttr.title().equals(""))
            Title = pageAttr.title();
        if (pageAttr.urlCheckType() != PageCheckType.NotSet)
            UrlCheckType = pageAttr.urlCheckType();
        if (pageAttr.titleCheckType() != PageCheckType.NotSet)
            TitleCheckType = pageAttr.titleCheckType();
        if (pageAttr.isHomePage())
            IsHomePage = true;
    }
}
