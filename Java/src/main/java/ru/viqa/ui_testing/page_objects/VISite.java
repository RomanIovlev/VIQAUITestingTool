package ru.viqa.ui_testing.page_objects;

import ru.viqa.ui_testing.common.*;
import ru.viqa.ui_testing.common.alertings.DefaultAlerting;
import ru.viqa.ui_testing.common.funcInterfaces.*;
import ru.viqa.ui_testing.common.interfaces.*;

import java.io.File;
import java.io.FileInputStream;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;
import java.util.concurrent.TimeUnit;

import ru.viqa.ui_testing.common.loggers.DefaultLogger;
import ru.viqa.ui_testing.annotations.AnnotationsUtil;
import ru.viqa.ui_testing.annotations.DemoSettings;
import ru.viqa.ui_testing.annotations.Site;
import ru.viqa.ui_testing.elements.baseClasses.VIElement;
import org.openqa.selenium.*;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.ie.InternetExplorerDriver;
import org.openqa.selenium.os.*;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.openqa.selenium.remote.RemoteWebDriver;
import org.testng.ITestResult;

import static ru.viqa.ui_testing.common.utils.StringUtils.LineBreak;
import static ru.viqa.ui_testing.page_objects.BrowserType.Firefox;
import static ru.viqa.ui_testing.common.utils.ReflectionUtils.getFields;
import static ru.viqa.ui_testing.common.utils.TimeUtils.nowTime;
import static ru.viqa.ui_testing.common.utils.LinqUtils.*;
import static ru.viqa.ui_testing.annotations.AnnotationsUtil.getElementName;

/**
 * Created by roman.i on 24.09.2014.
 */
public class VISite extends VIElement {
    public static ILogger Logger;
    public static IAlerting Alerting;
    public static ITestResult testContext;

    public WebDriverTimeouts WebDriverTimeouts = new WebDriverTimeouts();
    public SiteSettings SiteSettings = new SiteSettings();

    public static String RunId = nowTime("yy-MM-dd_HH-mm-ss");
    private static Properties _properties;
    public static String getProperty(String name) {
        return _properties.getProperty(name);
    }
    public void readPropertiesFromFile(String fileName) throws Exception {
        FileInputStream propFile = new FileInputStream(fileName);
        _properties = new Properties(System.getProperties());
        _properties.load(propFile);
    }
    public Navigation Navigate;

    public String Domain = "/";
    public void setDomain(String value) { Domain = value.replaceAll("/*$", "") + "/";  }

    public String locatorGroup;
    private WebDriver _webDriver;
    public boolean driverIsRun() {return _webDriver != null; }
    public BrowserType Browser = Firefox;
    public VISite setWebDriver(BrowserType browserType) { WebDriverFunc = getDriver(browserType); return this; }
    public WebDriver getWebDriver() throws Exception {
        if (!driverIsRun())
            return runWebDriver();
        return _webDriver; }
    public static List<WebDriver> RunWebDrivers = new ArrayList<>();

    public WebDriver runWebDriver() throws Exception {
        _webDriver = WebDriverFunc.invoke();
        RunWebDrivers.add(_webDriver);
        _webDriver.manage().window().maximize();
        _webDriver.manage().timeouts().implicitlyWait(WebDriverTimeouts.WaitWebElementInSec, TimeUnit.SECONDS);
        return _webDriver;
    }

    public FuncT<WebDriver> WebDriverFunc;

    private ActionT<DesiredCapabilities> desiredCapabilities;
    public void setDesiredCapabilities(ActionT<DesiredCapabilities> action) {
        desiredCapabilities = action;
    }

    public FuncT<WebDriver> getDriver(BrowserType browserType)
    {
        switch (browserType)
        {
            case Firefox:
                return () -> {
                    DesiredCapabilities capabilities = DesiredCapabilities.firefox();
                    if (desiredCapabilities != null)
                        desiredCapabilities.invoke(capabilities);
                    return new FirefoxDriver(capabilities);
                };
            case Chrome:
                return () -> {
                    DesiredCapabilities capabilities = DesiredCapabilities.chrome();
                    String path = new File(".").getCanonicalPath() + "\\drivers\\chromedriver.exe";
                    //String path = new File(".").getCanonicalPath() + "/drivers/chromedriver"; //for mac
                    System.setProperty("webdriver.chrome.driver", path);
                    if (desiredCapabilities != null)
                        desiredCapabilities.invoke(capabilities);
                    return new ChromeDriver(capabilities);
                };
            case IE:
                return () -> {
                    DesiredCapabilities capabilities = DesiredCapabilities.internetExplorer();
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

    public VISite() throws Exception {
        this(Firefox);
    }
    public VISite(BrowserType browserType) throws Exception { this(browserType, true, null); }
    public VISite(BrowserType browserType, String groupName) throws Exception { this(browserType, true, groupName); }
    public VISite(BrowserType browserType, boolean isMain) throws Exception { this(browserType, isMain, null); }
    public VISite(BrowserType browserType, boolean isMain, String groupName) throws Exception {
        WebDriverFunc = getDriver(browserType);
        initSite(isMain, groupName);
    }
    public VISite(FuncT<WebDriver> webDriver) throws Exception { this(webDriver, true, null); }
    public VISite(FuncT<WebDriver> webDriver, String groupName) throws Exception { this(webDriver, true, groupName); }
    public VISite(FuncT<WebDriver> webDriver, boolean isMain) throws Exception { this(webDriver, isMain, null); }
    public VISite(FuncT<WebDriver> webDriver, boolean isMain, String groupName) throws Exception {
        WebDriverFunc = webDriver;
        initSite(isMain, groupName);
    }
    public VISite startRemote(String settingsFileName) throws Exception {
        PropertyLoader props = new PropertyLoader(settingsFileName);
        props.fillAction(prop -> Domain = prop, "site.url");
        WebDriverFunc = () -> {
            final DesiredCapabilities[] capabilities = {new DesiredCapabilities()};
            props.fillAction(prop -> {
                if (prop != null && !prop.equals(""))
                    switch (prop) {
                        case "Firefox":
                            capabilities[0] = DesiredCapabilities.firefox();
                            break;
                        case "Chrome":
                            capabilities[0] = DesiredCapabilities.chrome();
                            break;
                        case "IE":
                            capabilities[0] = DesiredCapabilities.internetExplorer();
                            break;
                        default:
                            throw Alerting.throwError("Remote, Unsupported browser:" + prop);
                    }
                else
                    capabilities[0] = DesiredCapabilities.firefox();
            }, "browser.name");
            props.fillAction(prop -> capabilities[0].setPlatform(Platform.valueOf(prop)), "browser.os");
            props.fillAction(capabilities[0]::setVersion, "browser.version");
            return new RemoteWebDriver(new URL(props.get("grid2.hub")), capabilities[0]);
        };
        return this;
    }

    public void initSite(boolean isMain, String groupName) throws Exception {
        try {
            setName(getElementName(this));
            Logger = new DefaultLogger();
            ((DefaultLogger) Logger).LogFileFormat = () -> "%s_" + RunId + ".log";
            Navigate = new Navigation(this);
            fillSiteSettings();
            if (Alerting == null)
                Alerting = new DefaultAlerting();
            if (isMain)
                VIElement.init(this);
            locatorGroup = groupName;
            setSite(this);
        } catch (Exception ex) { throw new Exception("Init site failed: " + ex.getMessage()); }
        //PageObjectsInit.initPagesCascade(this);
    }

    private void fillSiteSettings() throws Exception {
        try {
            Site siteParams = getClass().getAnnotation(Site.class);
            SiteSettings = new SiteSettings();
            if (siteParams == null) return;
            if (siteParams.domain() != null && !siteParams.domain().equals(""))
                Domain = siteParams.domain();
            if (siteParams.useCache())
                SiteSettings.useCache = siteParams.useCache();
            if (siteParams.demoMode())
                SiteSettings.demoSettings = new HighlightSettings(getClass().getAnnotation(DemoSettings.class));
            if (siteParams.screenshotAlert())
                Alerting = AnnotationsUtil.getScreenshotAlert(this);

            if (!siteParams.settingsFromPropertyFile().equals("")) {
                try {
                    PropertyLoader props = new PropertyLoader(siteParams.settingsFromPropertyFile());
                    if (props.get("viSite.domain") != null && !props.get("viSite.domain").equals(""))
                        Domain = props.get("viSite.domain");
                    switch (props.get("viSite.useCache")) {
                        case "true":
                        case "1":
                            SiteSettings.useCache = true;
                        case "false":
                        case "0":
                            SiteSettings.useCache = false;
                    }
                    if (props.get("viSite.demoMode").equals("true") || props.get("viSite.demoMode").equals("1"))
                        SiteSettings.demoSettings = new HighlightSettings();
                    if (props.get("viSite.allerter").equals("screenshot"))
                        Alerting = AnnotationsUtil.getScreenshotAlert(this);
                } catch (Exception ex) { Logger.Event("Can't load Site properties from file: " + siteParams.settingsFromPropertyFile());}
            }
        } catch (Exception ex) { throw new Exception("Error in Fill Site params from Settings" + LineBreak + ex.getMessage()); }
    }

    public void takeScreenshot() throws Exception {
        AnnotationsUtil.getScreenshotAlert(this).takeScreenshot();
    }

    public void takeScreenshot(String path, String outputFileName) throws Exception {
        AnnotationsUtil.getScreenshotAlert(this).takeScreenshot(path, outputFileName);
    }
/*    public List<FieldInfo> PagesFields {  get  {
        return GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(_ => typeof(VIPage).IsAssignableFrom(_.FieldType)).ToList();
    } }*/

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
        List<VIPage> homePages = (List<VIPage>)where(getPages(), page -> page.IsHomePage);
        if (homePages.size() == 1)
            return first(getPages(), page -> page.IsHomePage);
        if (homePages.size() == 0) {
            VIPage page = new VIPage();
            page.setSite(this);
            page.setUrl(Domain);
            return page;
        }
        throw Alerting.throwError("Site have more than one HomePage. Please specify only one HomePage. Current HomePages: " +
                select(where(getPages(), page -> page.IsHomePage), Named::getName));
    }
    public void dispose()
    {
        if (!driverIsRun()) return;
        try { _webDriver.quit(); _webDriver = null; }
        catch(Exception ignored) { }
    }
    public static void disposeAll() throws Exception { foreach(RunWebDrivers, WebDriver::quit); }

    public static void killAllRunWebDrivers() throws Exception {
        String pid = getPid();
        while (pid != null){
            killPID(pid);
            pid = getPid();
        }
    }

    private static String getPid() throws Exception {
        return first(where(WindowsUtils.procMap(), el -> el.getKey() != null
                && (el.getKey().contains("firefox") && el.getKey().contains("-foreground"))
                | el.getKey().contains("chromedriver")
                | el.getKey().contains("IEDriverServer")));
    }

    private static void killPID(String processID) {
        executeCommand("taskkill", "/f", "/t", "/pid", processID);
    }

    private static void executeCommand(String commandName, String... args) {
        CommandLine cmd = new CommandLine(commandName, args);
        cmd.execute();
    }
}
