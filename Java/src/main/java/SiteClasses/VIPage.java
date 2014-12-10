package SiteClasses;

import VIAnnotations.Page;
import VIElements.BaseClasses.VIElementSet;
import org.openqa.selenium.WebDriver;

import static Common.Utils.StringUtils.*;
import static Common.Utils.ReflectionUtils.*;
import static Common.Utils.LinqUtils.*;
import static java.lang.String.format;

/**
 * Created by roman.i on 26.09.2014.
 */
public class VIPage extends VIElementSet {
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

    private void setEmptyPage()
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

    public VIPage() {
        DefaultNameFunc = () -> format("Page with Title: '%s', Url: '%s'", Title != null ? Title : "", getUrl() != null ? getUrl() : "");
        setEmptyPage();
    }
    
    public VIPage(String name, String url, String title, VISite site) throws Exception { Url = url; setName(name); Title = title; setSite(site); }

    public void open() throws Exception {
        getSite().Navigate.openPage(this);
        verifyPage(true);
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

    public static void init(VISite site) throws Exception {
        foreach(getFields(site, VIPage.class), page -> {
            VIPage instance = (VIPage) page.getType().newInstance();
            instance.setSite(site);
            Page pageAttr = page.getAnnotation(Page.class);
            if (pageAttr == null)
                instance.setEmptyPage();
            else
                instance.fillFromPageAttribute(pageAttr);
            pageAttr = page.getType().getAnnotation(Page.class);
            if (pageAttr != null)
                instance.updatePageAttribute(pageAttr);
            instance.initSubElements();
            page.set(site, instance);
        });
    }
}
