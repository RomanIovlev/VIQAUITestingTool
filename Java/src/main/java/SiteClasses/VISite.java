package SiteClasses;

import Common.*;
import Common.Alertings.DefaultAlerting;
import Common.FuncInterfaces.*;
import Common.Interfaces.*;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;
import java.util.concurrent.TimeUnit;

import Common.Loggers.DefaultLogger;
import VIAnnotations.AnnotationsUtil;
import VIAnnotations.DemoSettings;
import VIAnnotations.Site;
import VIElements.BaseClasses.VIElement;
import VIElements.BaseClasses.VIElementSet;
import org.openqa.selenium.*;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.ie.InternetExplorerDriver;
import org.openqa.selenium.os.*;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.testng.ITestResult;

import static SiteClasses.BrowserType.Firefox;
import static Common.Utils.ReflectionUtils.getFields;
import static Common.Utils.TimeUtils.nowTime;
import static Common.Utils.LinqUtils.*;
import static VIAnnotations.AnnotationsUtil.getElementName;

/**
 * Created by roman.i on 24.09.2014.
 */
public class VISite extends VIElementSet {
    public static ILogger Logger;
    public static IAlerting Alerting;
    public static ITestResult testContext;

    public IWebDriverTimeouts WebDriverTimeouts = new DefaultWebDriverTimeouts();
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
    public FuncT<WebDriver> WebDriverFunc;
    public BrowserType Browser = Firefox;
    public VISite setWebDriver(BrowserType browserType) { WebDriverFunc = getDriver(browserType); return this; }
    public WebDriver getWebDriver() throws Exception {
        if (_webDriver == null)
            return runWebDriver();
        return _webDriver; }
    private static List<WebDriver> RunWebDrivers = new ArrayList<>();

    public WebDriver runWebDriver() throws Exception {
        _webDriver = WebDriverFunc.invoke();
        RunWebDrivers.add(_webDriver);
        _webDriver.manage().window().maximize();
        _webDriver.manage().timeouts().implicitlyWait(WebDriverTimeouts.WaitWebElementInSec, TimeUnit.SECONDS);
        return _webDriver;
    }

    private static FuncT<WebDriver> getDriver() {
        return getDriver(Firefox);
    }
    private static FuncT<WebDriver> getDriver(BrowserType browserType)
    {
        switch (browserType)
        {
            case Firefox:
                return FirefoxDriver::new;
            case Chrome:
                return () -> {
                    DesiredCapabilities chromeCapabilities = DesiredCapabilities.chrome();
                    String path = new File(".").getCanonicalPath() + "\\drivers\\chromedriver.exe";
                    //String path = new File(".").getCanonicalPath() + "/drivers/chromedriver"; //for mac
                    System.setProperty("webdriver.chrome.driver", path);
                    return new ChromeDriver(chromeCapabilities);
                };
            case IE:
                return () -> {
                    DesiredCapabilities ieCapabilities = DesiredCapabilities.internetExplorer();
                    String path = new File(".").getCanonicalPath() + "\\drivers\\IEDriverServer.exe";
                    ieCapabilities.setCapability(
                            InternetExplorerDriver.INTRODUCE_FLAKINESS_BY_IGNORING_SECURITY_DOMAINS, true);
                    System.setProperty("webdriver.ie.driver", path);
                    return new InternetExplorerDriver(ieCapabilities);
                };
            default:
                return FirefoxDriver::new;
        }
    }

    public VISite() throws Exception { this(getDriver(), true, null); }
    public VISite(BrowserType browserType) throws Exception { this(getDriver(browserType), true, null); }
    public VISite(FuncT<WebDriver> webDriver) throws Exception { this(webDriver, true, null); }
    public VISite(BrowserType browserType, String group) throws Exception { this(getDriver(browserType), true, group); }
    public VISite(FuncT<WebDriver> webDriver, String group) throws Exception { this(webDriver, true, group); }
    public VISite(BrowserType browserType, boolean isMain) throws Exception { this(getDriver(browserType), isMain, null); }
    public VISite(BrowserType browserType, boolean isMain, String group) throws Exception { this(getDriver(browserType), isMain, group); }
    public VISite(FuncT<WebDriver> webDriver, boolean isMain, String groupName) throws Exception {
        setName(getElementName(this));
        WebDriverFunc = webDriver;
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
        initSubElements();
        VIPage.init(this);
    }

    private void fillSiteSettings() throws IOException {
        Site siteParams = getClass().getAnnotation(Site.class);
        SiteSettings = new SiteSettings();
        if (siteParams != null)
        {
            if (!siteParams.settingsFromPropertyFile().equals("")) {
                PropertyLoader props = new PropertyLoader(siteParams.settingsFromPropertyFile());
                if (props.get("viSite.domain") != null && !props.get("viSite.domain").equals(""))
                    Domain = props.get("viSite.domain");
                switch (props.get("viSite.useCache")) {
                    case "true":
                    case "1":
                        SiteSettings.UseCache = true;
                    case "false":
                    case "0":
                        SiteSettings.UseCache = false;
                }
                if (props.get("viSite.demoMode").equals("true") || props.get("viSite.demoMode").equals("1"))
                    SiteSettings.DemoSettings = new HighlightSettings();
                if (props.get("viSite.allerter").equals("screenshot"))
                    Alerting = AnnotationsUtil.getScreenshotAlert(this);
            }
            if (siteParams.domain() != null)
                Domain = siteParams.domain();
            if (siteParams.useCache())
                SiteSettings.UseCache = siteParams.useCache();
            if (siteParams.demoMode())
                SiteSettings.DemoSettings = new HighlightSettings(getClass().getAnnotation(DemoSettings.class));
            if (siteParams.screenshotAlert())
                Alerting = AnnotationsUtil.getScreenshotAlert(this);
        }
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
        if (_webDriver == null) return;
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
