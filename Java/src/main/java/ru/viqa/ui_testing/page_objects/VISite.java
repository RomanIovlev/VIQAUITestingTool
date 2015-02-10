package ru.viqa.ui_testing.page_objects;

import ru.viqa.ui_testing.common.*;
import ru.viqa.ui_testing.common.alertings.DefaultAlerting;
import ru.viqa.ui_testing.common.alertings.ScreenshotAlert;
import ru.viqa.ui_testing.common.funcInterfaces.*;
import ru.viqa.ui_testing.common.interfaces.*;

import java.io.File;
import java.io.IOException;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;

import ru.viqa.ui_testing.common.loggers.DefaultLogger;
import ru.viqa.ui_testing.annotations.DemoSettings;
import ru.viqa.ui_testing.annotations.Site;
import ru.viqa.ui_testing.elements.baseClasses.VIElement;
import org.openqa.selenium.*;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.ie.InternetExplorerDriver;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.openqa.selenium.remote.RemoteWebDriver;

import static java.lang.Integer.parseInt;
import static org.openqa.selenium.remote.DesiredCapabilities.*;
import static ru.viqa.ui_testing.common.utils.StringUtils.LineBreak;
import static ru.viqa.ui_testing.page_objects.BrowserType.FIREFOX;
import static ru.viqa.ui_testing.common.utils.ReflectionUtils.getFields;
import static ru.viqa.ui_testing.common.utils.TimeUtils.nowTime;
import static ru.viqa.ui_testing.common.utils.LinqUtils.*;
import static ru.viqa.ui_testing.annotations.AnnotationsUtil.getElementName;
import static ru.viqa.ui_testing.page_objects.PageObjectsInit.interfaceTypeMapInit;

/**
 * Created by roman.i on 24.09.2014.
 */
public class VISite extends VIElement {
    public static ILogger Logger;
    public static IAlerting Alerting;

    public SiteSettings siteSettings = new SiteSettings();
    private static PropertyLoader properties;
    public static String getProperty(String name) throws IOException {
        return properties.get(name);
    }
    public static void setProperties(PropertyLoader properties) throws IOException {
        VISite.properties = properties;
    }


    public static String RunId;

    public Navigation navigate;
    public void openPage(String uri) throws Exception {
        new VIPage("Page with url " + VIPage.getUrlValue(uri, this), uri, "", this).open();
    }

    private String Domain = "/";
    public String getDomain() { return Domain; }
    public void setDomain(String value) { Domain = value.replaceAll("/*$", "");  }

    private String locatorVersion;
    public String getLocatorVersion() { return locatorVersion; }
    private WebDriver _webDriver;
    private BrowserType browserType;
    public BrowserType getBrowserType() { return browserType; }
    public boolean isDriverRun() {return _webDriver != null; }
    public VISite setWebDriver(BrowserType browserType) {
        this.browserType = browserType;
        WebDriverFunc = getWebDriver(browserType); return this; }
    public WebDriver getWebDriver() throws Exception {
        if (!isDriverRun())
            return runWebDriver();
        return _webDriver; }
    private static List<WebDriver> RunWebDrivers = new ArrayList<>();
    public static List<WebDriver> getRunWebDrivers() { return RunWebDrivers;}

    public WebDriver runWebDriver() throws Exception {
        _webDriver = WebDriverFunc.invoke();
        RunWebDrivers.add(_webDriver);
        _webDriver.manage().window().maximize();
        _webDriver.manage().timeouts().implicitlyWait(siteSettings.timeouts.WaitWebElementInSec, TimeUnit.SECONDS);
        return _webDriver;
    }

    public FuncT<WebDriver> WebDriverFunc;

    private ActionT<DesiredCapabilities> desiredCapabilities;
    public void setDesiredCapabilities(ActionT<DesiredCapabilities> action) {
        desiredCapabilities = action;
    }

    public FuncT<WebDriver> getWebDriver(BrowserType browserType)
    {
        this.browserType = browserType;
        switch (browserType)
        {
            case FIREFOX:
                return () -> {
                    DesiredCapabilities capabilities = firefox();
                    if (desiredCapabilities != null)
                        desiredCapabilities.invoke(capabilities);
                    return new FirefoxDriver(capabilities);
                };
            case CHROME:
                return () -> {
                    DesiredCapabilities capabilities = chrome();
                    String path = new File(".").getCanonicalPath() + "\\drivers\\chromedriver.exe";
                    //String path = new File(".").getCanonicalPath() + "/drivers/chromedriver"; //for mac
                    System.setProperty("webdriver.chrome.driver", path);
                    if (desiredCapabilities != null)
                        desiredCapabilities.invoke(capabilities);
                    return new ChromeDriver(capabilities);
                };
            case IE:
                return () -> {
                    DesiredCapabilities capabilities = internetExplorer();
                    String path = new File(".").getCanonicalPath() + "\\drivers\\IEDriverServer.exe";
                    capabilities.setCapability(
                            InternetExplorerDriver.INTRODUCE_FLAKINESS_BY_IGNORING_SECURITY_DOMAINS, true);
                    System.setProperty("webdriver.ie.driver", path);
                    if (desiredCapabilities != null)
                        desiredCapabilities.invoke(capabilities);
                    return new InternetExplorerDriver(capabilities);
                };
            default:
                return FirefoxDriver::new;
        }
    }

    public VISite() throws Exception { this(FIREFOX); }
    public VISite(BrowserType browserType) throws Exception { this(browserType, null); }
    public VISite(BrowserType browserType, String locatorVersion) throws Exception {
        WebDriverFunc = getWebDriver(browserType);
        initSite(locatorVersion);
    }
    public VISite(FuncT<WebDriver> webDriver) throws Exception { this(webDriver, null); }
    public VISite(FuncT<WebDriver> webDriver, String locatorVersion) throws Exception {
        WebDriverFunc = webDriver;
        initSite(locatorVersion);
    }

    public VISite startRemote(PropertyLoader props) throws Exception {
        props.fillAction(prop -> Domain = prop, "site.url");
        WebDriverFunc = () -> {
            final DesiredCapabilities[] capabilities = {new DesiredCapabilities()};
            props.fillAction(prop -> {
                if (prop != null && !prop.equals(""))
                    switch (prop) {
                        case "FIREFOX":
                            capabilities[0] = firefox();
                            break;
                        case "CHROME":
                            capabilities[0] = chrome();
                            break;
                        case "IE":
                            capabilities[0] = internetExplorer();
                            break;
                        default:
                            throw Alerting.throwError("Remote, Unsupported browser:" + prop);
                    }
                else
                    capabilities[0] = firefox();
            }, "browser.name");
            props.fillAction(prop -> capabilities[0].setPlatform(Platform.valueOf(prop)), "browser.os");
            props.fillAction(capabilities[0]::setVersion, "browser.version");
            return new RemoteWebDriver(new URL(props.get("grid2.hub")), capabilities[0]);
        };
        return this;
    }

    private void initSite(String locatorVersion) throws Exception {
        try {
            setName(getElementName(this));
            if (RunId == null)
                RunId = nowTime("yyyy-MM-dd_HH-mm-ss");
            if (Logger == null)
                Logger = new DefaultLogger(RunId);
            interfaceTypeMapInit();
            navigate = new Navigation(this);
            fillSiteSettings();
            if (Alerting == null)
                Alerting = new DefaultAlerting();
            this.locatorVersion = locatorVersion;
            setSite(this);
        } catch (Exception ex) { throw new Exception("Init site failed: " + ex.getMessage()); }
    }

    private void fillSiteSettings() throws Exception {
        try {
            Site siteParams = getClass().getAnnotation(Site.class);
            siteSettings = new SiteSettings();
            if (siteParams == null) return;
            if (siteParams.settingsFromPropertyFile().equals("")) {
                if (siteParams.domain() != null && !siteParams.domain().equals(""))
                    Domain = siteParams.domain();
                if (siteParams.useCache())
                    siteSettings.useCache = siteParams.useCache();
                if (siteParams.demoMode())
                    siteSettings.demoSettings = new HighlightSettings(getClass().getAnnotation(DemoSettings.class));
                if (siteParams.screenshotAlert())
                    Alerting = new ScreenshotAlert(this);
            } else 
                try {
                    properties = new PropertyLoader(siteParams.settingsFromPropertyFile());
                    fillSettingsFromProperties(properties);
                } catch (Exception ex) { Logger.event("Can't load Site properties from file: " + siteParams.settingsFromPropertyFile());}

        } catch (Exception ex) { throw new Exception("Error in Fill Site params from Settings" + LineBreak + ex.getMessage()); }
    }

    public void fillSettingsFromProperties(PropertyLoader props) throws Exception {
        if (props.get("viSite.domain") != null && !props.get("viSite.domain").equals(""))
            Domain = props.get("viSite.domain");
        switch (props.get("viSite.useCache")) {
            case "true":
            case "1":
                siteSettings.useCache = true;
            case "false":
            case "0":
                siteSettings.useCache = false;
        }
        if (props.get("viSite.demoMode").equals("true") || props.get("viSite.demoMode").equals("1"))
            siteSettings.demoSettings = new HighlightSettings();
        if (props.get("viSite.alerter").equals("screenshot"))
            Alerting = new ScreenshotAlert(this);
        props.fillAction(prop ->
                siteSettings.timeouts.WaitWebElementInSec = parseInt(prop), "wait.element.timeout.sec");
        props.fillAction(prop ->
                siteSettings.timeouts.WaitPageToLoadInSec = parseInt(prop), "wait.pageload.timeout.sec");
        props.fillAction(Logger::setLogFolder, "logger.folder");
        props.fillAction(Logger::setLogFileName, "logger.file.name");
        props.fillAction(Logger::setLogRecord, "logger.record");
    }

    public void open(String url) throws Exception {
        new VIPage(url, url, "", this).open();
    }
    public void open() throws Exception {
        getHomePage().open();
    }

    public void takeScreenshot() throws Exception {
        takeScreenshot(null, null);
    }

    public void takeScreenshot(String path) throws Exception {
        takeScreenshot(path, null);
    }

    public void takeScreenshot(String path, String outputFileName) throws Exception {
        new ScreenshotAlert(this).takeScreenshot(path, outputFileName);
    }
/*
    public List<FieldInfo> PagesFields {  get  {
        return GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(_ => typeof(VIPage).IsAssignableFrom(_.FieldType)).ToList();
    } }
*/

    public List<VIPage> getPages() throws Exception {
        return (List<VIPage>)select(getFields(this, VIPage.class), field -> (VIPage) field.get(this)); }

    public void checkPage(String name)
    {
/*        VIPage viPage = null;
        if (getPages().Any(page => String.Equals(page.Name, name)))
        viPage = Pages.First(page => String.Equals(page.Name, name));
        if (PagesFields.Any(field => String.Equals(field.Name, name)))
        viPage = ((VIPage)PagesFields.First(field => String.Equals(field.Name, name, StringComparison.CurrentCultureIgnoreCase)).GetValue(this));
        if (viPage != null)
            viPage.VerifyPage(true);
        else
            Alerting.throwError("Can't check page '" + name + "'. Site have no pages with this name.");*/
    }

    public VIPage getHomePage() throws Exception {
        List<VIPage> homePages = (List<VIPage>)where(getPages(), page -> page.isHomePage);
        if (homePages.size() == 1)
            return first(getPages(), page -> page.isHomePage);
        if (homePages.size() == 0) {
            VIPage page = new VIPage();
            page.setSite(this);
            page.setUrl(Domain);
            return page;
        }
        throw Alerting.throwError("Site have more than one HomePage. Please specify only one HomePage. Current HomePages: " +
                select(where(getPages(), page -> page.isHomePage), Named::getName));
    }
    public void dispose()
    {
        if (!isDriverRun()) return;
        try { _webDriver.quit(); _webDriver = null; }
        catch(Exception ignored) { }
    }
    public static void disposeAll() throws Exception { foreach(RunWebDrivers, WebDriver::quit); }
}
