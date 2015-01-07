package ru.viqa.ui_testing.elements.baseClasses;

import ru.viqa.ui_testing.common.funcInterfaces.*;
import ru.viqa.ui_testing.common.interfaces.WebDriverTimeouts;
import ru.viqa.ui_testing.common.Named;
import ru.viqa.ui_testing.common.pairs.Pair;
import ru.viqa.ui_testing.common.pairs.Pairs;
import ru.viqa.ui_testing.common.Scenario;
import ru.viqa.ui_testing.common.utils.Timer;
import ru.viqa.ui_testing.page_objects.HighlightSettings;
import ru.viqa.ui_testing.page_objects.VISite;
import ru.viqa.ui_testing.annotations.AnnotationsUtil;
import ru.viqa.ui_testing.elements.interfaces.*;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.*;

import java.lang.reflect.Field;
import java.util.List;
import java.util.concurrent.TimeUnit;

import static ru.viqa.ui_testing.common.utils.LinqUtils.first;
import static ru.viqa.ui_testing.common.utils.LinqUtils.foreach;
import static ru.viqa.ui_testing.common.utils.ReflectionUtils.getField;
import static ru.viqa.ui_testing.common.utils.ReflectionUtils.getFields;
import static ru.viqa.ui_testing.common.utils.WebDriverByUtils.*;
import static ru.viqa.ui_testing.common.utils.StringUtils.*;
import static ru.viqa.ui_testing.common.pairs.Pairs.*;
import static java.lang.String.format;
import static ru.viqa.ui_testing.page_objects.VISite.Alerting;

/**
 * Created by 12345 on 10.05.2014.
 */
public class VIElement extends Named implements IVIElement {
    private VISite _site;
    public VISite getSite() { return _site;}
    protected boolean isSiteSet() { return _site != null; }
    public void setSite(VISite site) { _site = site;}
    public Pairs<ContextType, By> Context = new Pairs<>();
    protected By _locator;
    public static VISite DefaultSite;
    public boolean haveLocator() { return _locator != null; }

    public void fillFrom(Object obj) throws Exception {
        if (obj == null) return;
        List<Field> fields = getFields(obj, String.class);
        foreach(getFields(this, IHaveValue.class), element -> {
            Field fieldWithName = first(fields, field ->
                    AnnotationsUtil.getElementName(field)
                            .equals(AnnotationsUtil.getElementName(element)));
            if (fieldWithName != null) {
                String value = (String) getField(fieldWithName, obj);
                if (value != null)
                    ((IHaveValue) getField(element, this)).setValue(value);
            }
        });
    }
    public SearchContext getSearchContext() throws Exception {
        if (Context == null)
            return getWebDriver();
        SearchContext context = getWebDriver().switchTo().defaultContent();
        for (Pair<ContextType, By> locator : Context) {
            WebElement element = context.findElement(locator.Value2);
            if (locator.Value1 == ContextType.Locator)
                context = element;
            else {
                getWebDriver().switchTo().frame(element);
                context = getWebDriver();
            }
        }
        return context;
    }

    public void setFrame(String id) {
        Context.add(ContextType.Frame, By.id(id));
    }

    private String printLocator() throws Exception {
        return format("Context: '%s'. Locator: '%s'",
                Context != null ? toPairs(Context, el -> el.Value1.toString(), el -> el.Value2).print() : "null",
                _locator != null ? _locator.toString() : "null");
    }

    public void setLocator(By value) {
        _locator = value;
    }

    public By getPrivateLocator() throws Exception {
        return _locator;
    }

    public By getLocator() throws Exception {
        By locator = (getTemplateId() == null || getTemplateId().equals(""))
                ? _locator
                : fillByTemplate(_locator, getTemplateId());
        if (locator != null)
            return locator;
        throw Alerting.throwError(getDefaultLogMessage("LocatorAttribute cannot be null"));
    }

    protected Timer getTimer() {
        return new Timer(getWaitTimeoutInSec() * 1000, getSite().WebDriverTimeouts.RetryActionInMSec);
    }
    private Timer getTimer(long timeoutInSec, long retryTimeoutInMSec)
    {
        if (timeoutInSec < 0)
            return getTimer();
        return new Timer(timeoutInSec, ((retryTimeoutInMSec >= 0) ? retryTimeoutInMSec : 100));
    }

    public WebDriver getWebDriver() throws Exception {
        return getSite().getWebDriver();
    }

    public WebDriverTimeouts getTimeouts() {
        return getSite().WebDriverTimeouts;
    }

    private WebElement _webElement;

    private void setWebElement(WebElement we) {
        dropCache();
        _webElement = we;
    }
    public WebElement getWebElement() { return _webElement; }

    private long getDefaultWaitTimeoutInSec() { return getTimeouts().WaitWebElementInSec; }

    private Long _waitTimeoutInSec = null;
    private String _templateId;
    public String getTemplateId() { return _templateId; }
    public IVIElement setTemplateId(String tId) { dropCache(); _templateId = tId; return this; }

    public IVIElement setWaitTimeout(long waitTimeoutInSec) {
        _waitTimeoutInSec = waitTimeoutInSec;
        return this;
    }

    private long getWaitTimeoutInSec() {
        if (OpenPageName == null)
            return (_waitTimeoutInSec != null) ? _waitTimeoutInSec : getDefaultWaitTimeoutInSec();
        if (!OpenPageName.equals(""))
            getSite().checkPage(OpenPageName);
        OpenPageName = null;
        return getTimeouts().WaitPageToLoadInSec;
    }

    protected String TypeName = "Element type undefined";
    public String getFullName() throws Exception { return (getName() != null) ? getName() : TypeName + " with undefiened Name";}

    private WebElement getUniqueWebElement(List<WebElement> webElements) throws Exception {
        if (webElements.size() == 1)
            return webElements.get(0);
        if (webElements.size() > 0)
            throw Alerting.throwError(getLotOfFindElementsMessage(webElements));
        throw Alerting.throwError(getCantFindElementMessage());
    }
    private String getLotOfFindElementsMessage(List<WebElement> webElements) throws Exception {
        return format("Find %s elements '%s' but expected 1. Please correct locator '%s'",
                Integer.toString(webElements.size()), getName(), printLocator()); }

    private String getCantFindElementMessage() throws Exception {
        return format("Can't find element '%s' by selector '%s'. Please correct locator" + LineBreak, getName(), printLocator()); }

    public List<WebElement> searchElements() throws Exception { return searchElements(getLocator()); }
    public List<WebElement> searchElements(By locator) throws Exception {
        try { return getSearchContext().findElements(locator); }
        catch(Exception ex) { throw Alerting.throwError(getCantFindElementMessage());}
    }

    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc) throws Exception {
        return waitElementState(waitFunc, getUniqueWebElement(searchElements()), -1, -1); }
    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement) throws Exception {
        return waitElementState(waitFunc, webElement, -1, -1); }
    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec) throws Exception {
        return waitElementState(waitFunc, webElement, timeoutInSec, -1); }
    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec, long retryTimeoutInMSec) throws Exception
    {
        return doVIActionResult("Wait element State",
                () -> getTimer(timeoutInSec, retryTimeoutInMSec)
                    .wait(() -> waitFunc.invoke(webElement)),
                Object::toString);
    }

    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc) throws Exception {
        return waitElementWithState(waitFunc, getUniqueWebElement(searchElements()), -1, -1, "");
    }
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement) throws Exception {
        return waitElementWithState(waitFunc, webElement, -1, -1, "");
    }
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, String msg) throws Exception {
        return waitElementWithState(waitFunc, webElement, -1, -1, msg);
    }
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec) throws Exception {
        return waitElementWithState(waitFunc, webElement, timeoutInSec, -1, "");
    }
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec, long retryTimeoutInMSec) throws Exception {
        return waitElementWithState(waitFunc, webElement, timeoutInSec, retryTimeoutInMSec, "");
    }
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec, long retryTimeoutInMSec, String msg) throws Exception {
        if (waitElementState(waitFunc, webElement, timeoutInSec, retryTimeoutInMSec))
            return getUniqueWebElement();
        throw Alerting.throwError(msg);
    }
    private List<WebElement> waitWebElements() throws Exception {
        if (getTimer().wait(() -> searchElements().size() > 0))
            try {
                return searchElements();
            } catch (Exception ex) {return null; }
        return null;
    }

    public boolean isPresent() throws Exception {
        return doVIActionResult("Is Element Present",
            () -> {
                try {
                    getWebDriver().manage().timeouts().implicitlyWait(0, TimeUnit.SECONDS);
                    List<WebElement> elements = searchElements();
                    getWebDriver().manage().timeouts().implicitlyWait(getSite().WebDriverTimeouts.WaitWebElementInSec, TimeUnit.SECONDS);
                    return elements.size() > 0;
                } catch (Exception ex) { return false; }
            }, Object::toString);
    }

    public boolean isDisplayed() throws Exception {
        return doVIActionResult("Is Element Displayed",
            () -> {
                try {
                    getWebDriver().manage().timeouts().implicitlyWait(0, TimeUnit.SECONDS);
                    List<WebElement> elements = doVIActionResult("Is Displayed Present", this::searchElements, els -> "Find " + els.size() + " element(s)");
                    getWebDriver().manage().timeouts().implicitlyWait(getSite().WebDriverTimeouts.WaitWebElementInSec, TimeUnit.SECONDS);
                    for (WebElement el : elements)
                        if (el.isDisplayed())
                            return true;
                    return false;
                } catch(Exception ex) { return false; }
        }, Object::toString);
    }

    public int cashDropTimes;

    private void isClearCashNeeded() {
        if (getSite().SiteSettings.useCache) {
            if (cashDropTimes != getSite().SiteSettings.cashDropTimes)
                dropCache();
            return;
        }
        _webElement = null;
    }

    private void dropCache() {
        cashDropTimes = getSite().SiteSettings.cashDropTimes;
        _webElement = null;
    }

    public WebElement getUniqueWebElement() throws Exception {
        isClearCashNeeded();
        if (_webElement != null)
            return _webElement;
        List<WebElement> foundElements = waitWebElements();
        if (foundElements == null)
            throw Alerting.throwError(getCantFindElementMessage());
        setWebElement(getUniqueWebElement(foundElements));
        _waitTimeoutInSec = null;
        return waitElementWithState(WebElement::isDisplayed, getWebElement(), getDefaultLogMessage(("Found element stay invisible.")));
    }

    public IVIElement getVIElement() throws Exception {
        setWebElement(getUniqueWebElement());
        return this;
    }

    public VIElement() throws Exception { }
    public VIElement(String name) throws Exception { setName(name); }
    public VIElement(String name, String cssSelector) throws Exception { this(name); setLocator(By.cssSelector(cssSelector)); }
    public VIElement(String name, By byLocator) throws Exception { this(name); setLocator(byLocator); }
    public VIElement(By byLocator) throws Exception { this(); setLocator(byLocator); }
    public VIElement(String name, WebElement webElement) throws Exception { this(name); setWebElement(webElement); }
    public VIElement(WebElement webElement) throws Exception { this("", webElement); }

    public String getDefaultLogMessage(String text) throws Exception {
        return text +  format(" (Name: '%s', Type: '%s', LocatorAttribute: '%s')", getFullName(), TypeName, printLocator());
    }

    public static Scenario viScenario = (viElement, actionName, viAction) -> {
        VISite.Logger.Event(viElement.getDefaultLogMessage(actionName));
        return viAction.invoke();
    };

    public final <T> T doVIActionResult(String logActionName, FuncT<T> viAction) throws Exception {
        return doVIActionResult(logActionName, viAction, null);
    }
    public final <T> T doVIActionResult(String actionName, FuncT<T> viAction, FuncTT<T, String> logResult) throws Exception
    {
        try {
            T result = (T) viScenario.invoke(this, actionName, () -> viAction.invoke());
            HighlightSettings demoMode = getSite().SiteSettings.demoSettings;
            if (demoMode != null)
                highlight(demoMode);
            if (logResult != null)
                VISite.Logger.Event(logResult.invoke(result));
            return result;
            }
        catch (Exception ex)
        {
            throw Alerting.throwError(format(LineBreak + "Failed to do '%s' action. Exception: %s", actionName, ex));
        }
    }

    public final void doVIAction(String actionName, Action viAction) throws Exception
    {
        try {
            viScenario.invoke(this, actionName, () -> {viAction.invoke(); return null;});
            HighlightSettings demoMode = getSite().SiteSettings.demoSettings;
            if (demoMode != null)
                highlight(demoMode);
        }
        catch (Exception ex) {
            throw Alerting.throwError(format(LineBreak + "Failed to do '%s' action. Exception: %s", actionName, ex));
        }
    }

    public static FuncTTT<String, String, Boolean> DefaultCompareFunc = (a, e) -> a.equals(e);

    protected static String OpenPageName;

    public static void init(VISite site) throws Exception {
        DefaultSite = site;
    }

    public void SetAttribute(String attributeName, String value) throws Exception {
        ((JavascriptExecutor)getWebDriver()).executeScript("arguments[0].setAttribute(arguments[1], arguments[2])", getUniqueWebElement(), attributeName, value);
    }
    public void highlight() throws Exception { highlight("yellow", "red", 1); }
    public void highlight(String bgColor) throws Exception { highlight(bgColor, "red", 1); }
    public void highlight(String bgColor, String frameColor) throws Exception { highlight(bgColor, frameColor, 1); }
    public void highlight(String bgColor, String frameColor, int timeoutInSec) throws Exception {
        highlight(new HighlightSettings(bgColor, frameColor, timeoutInSec));
    }

    public void highlight(HighlightSettings highlightSettings) throws Exception {
        if (highlightSettings == null)
            highlightSettings = new HighlightSettings();
        String orig = getUniqueWebElement().getAttribute("style");
        SetAttribute("style",  format("border: 3px solid %s; background-color: %s;", highlightSettings.FrameColor, highlightSettings.BgColor));
        Thread.sleep(highlightSettings.TimeoutInSec * 1000);
        SetAttribute("style", orig);
    }
}
