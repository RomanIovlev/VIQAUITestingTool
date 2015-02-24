package ru.viqa.ui_testing.elements.baseClasses;

import ru.viqa.ui_testing.common.funcInterfaces.*;
import ru.viqa.ui_testing.common.interfaces.WebDriverTimeouts;
import ru.viqa.ui_testing.common.Named;
import ru.viqa.ui_testing.common.pairs.*;
import ru.viqa.ui_testing.common.Scenario;
import ru.viqa.ui_testing.common.utils.Timer;
import ru.viqa.ui_testing.page_objects.*;
import ru.viqa.ui_testing.annotations.AnnotationsUtil;
import ru.viqa.ui_testing.elements.interfaces.*;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.*;

import java.lang.reflect.Field;
import java.util.Collection;
import java.util.List;
import java.util.concurrent.TimeUnit;

import static ru.viqa.ui_testing.common.utils.LinqUtils.*;
import static ru.viqa.ui_testing.common.utils.ReflectionUtils.*;
import static ru.viqa.ui_testing.common.utils.WebDriverByUtils.*;
import static ru.viqa.ui_testing.common.utils.StringUtils.*;
import static ru.viqa.ui_testing.common.pairs.Pairs.*;
import static java.lang.String.format;
import static ru.viqa.ui_testing.page_objects.VISite.Alerting;
import static ru.viqa.ui_testing.page_objects.VISite.Logger;

/**
 * Created by 12345 on 10.05.2014.
 */
public class VIElement extends Named implements IVIElement {
    private VISite site;
    public VISite getSite() { return site;}
    protected boolean isSiteSet() { return site != null; }
    public VIElement setSite(VISite site) { this.site = site; return this;}
    public Pairs<ContextType, By> Context = new Pairs<>();
    protected By _locator;
    public boolean haveLocator() { return _locator != null; }
    protected boolean checkVisibility = true;

    public void fillFrom(Object obj) throws Exception {
        if (obj == null) return;
        List<Field> fields = getFields(obj, String.class);
        foreach(getFields(this, IHaveValue.class), element -> {
            Field fieldWithName = first(fields, field ->
                AnnotationsUtil.getElementName(field)
                    .equals(AnnotationsUtil.getElementName(element)));
            if (fieldWithName != null) {
                String value = (String) getFieldValue(fieldWithName, obj);
                if (value != null && !value.equals("")) {
                    if (value.equals("#CLEAR#")) value = "";
                    ((IHaveValue) getFieldValue(element, this)).setValue(value);
                }
            }
        });
    }

    public SearchContext getSearchContext() throws Exception {
        boolean isFirst = true;
        if (Context == null || Context.size() == 0)
            return getWebDriver();
        SearchContext context = getWebDriver().switchTo().defaultContent();
        for (Pair<ContextType, By> locator : Context) {
            if (!isFirst && locator.Value2.toString().contains("By.xpath: //"))
                locator.Value2 = getByFunc(locator.Value2).invoke(getByLocator(locator.Value2)
                    .replaceFirst("/", "./"));
            WebElement element = context.findElement(locator.Value2);
            if (locator.Value1 == ContextType.Locator)
                context = element;
            else {
                getWebDriver().switchTo().frame(element);
                context = getWebDriver();
            }
            isFirst = false;
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

    protected Timer getTimer() throws Exception {
        return new Timer(getWaitTimeoutInSec() * 1000, getSite().siteSettings.timeouts.RetryActionInMSec);
    }
    private Timer getTimer(long timeoutInSec, long retryTimeoutInMSec) throws Exception {
        if (timeoutInSec < 0)
            return getTimer();
        return new Timer(timeoutInSec, ((retryTimeoutInMSec >= 0) ? retryTimeoutInMSec : 100));
    }

    public WebDriver getWebDriver() throws Exception {
        return getSite().getWebDriver();
    }

    public WebDriverTimeouts getTimeouts() {
        return getSite().siteSettings.timeouts;
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
        if (openPageName == null || openPageName.equals(""))
            return (_waitTimeoutInSec != null) ? _waitTimeoutInSec : getDefaultWaitTimeoutInSec();
        getSite().checkPage(openPageName);
        return getTimeouts().WaitPageToLoadInSec;
    }

    protected String getTypeName() { return last(this.getClass().getTypeName().split("\\."));}
    public String getFullName() throws Exception { return (getName() != null) ? getName() : getTypeName() + " with undefiened Name";}

    private WebElement getUniqueWebElement(List<WebElement> webElements) throws Exception {
        if (webElements.size() == 1)
            return webElements.get(0);
        if (webElements.size() > 0) {
            webElements = (List<WebElement>)where(webElements, WebElement::isDisplayed);
            if (webElements.size() == 1)
                return webElements.get(0);
            throw Alerting.throwError(getLotOfFindElementsMessage(webElements));
        }
        throw Alerting.throwError(getCantFindElementMessage());
    }
    private String getLotOfFindElementsMessage(List<WebElement> webElements) throws Exception {
        return format("Find %s elements '%s' but expected 1. Please correct locator '%s'",
                Integer.toString(webElements.size()), getName(), printLocator()); }

    private String getCantFindElementMessage() throws Exception {
        return format("Can't find element '%s' by selector '%s'. Please correct locator" + LineBreak, getName(), printLocator()); }

    private List<WebElement> searchElements() throws Exception { return searchElements(getLocator()); }
    private List<WebElement> searchElements(By locator) throws Exception {
        try {
            if (Context.size() > 0 && locator.toString().contains("By.xpath: //"))
                setLocator(getByFunc(locator).invoke(getByLocator(locator)
                        .replaceFirst("/", "./")));
            SearchContext sc = getSearchContext();
            By locatorS = getLocator();
            return sc.findElements(locatorS); }
        catch(Exception ex) { throw Alerting.throwError(getCantFindElementMessage());}
    }

    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc) throws Exception {
        return waitElementState(waitFunc, getUniqueWebElement(searchElements()), -1, -1); }
    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement) throws Exception {
        return waitElementState(waitFunc, webElement, -1, -1); }
    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec)
            throws Exception {
        return waitElementState(waitFunc, webElement, timeoutInSec, -1); }
    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec,
            long retryTimeoutInMSec) throws Exception {
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
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, String msg)
            throws Exception {
        return waitElementWithState(waitFunc, webElement, -1, -1, msg);
    }
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec)
            throws Exception {
        return waitElementWithState(waitFunc, webElement, timeoutInSec, -1, "");
    }
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec,
            long retryTimeoutInMSec) throws Exception {
        return waitElementWithState(waitFunc, webElement, timeoutInSec, retryTimeoutInMSec, "");
    }
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec,
           long retryTimeoutInMSec, String msg) throws Exception {

        if (waitElementState(waitFunc, webElement, timeoutInSec, retryTimeoutInMSec))
            return getUniqueWebElement();
        throw Alerting.throwError(msg);
    }
    public List<WebElement> waitListOfWebElements() throws Exception {
        if (getTimer().wait(() -> searchElements().size() > 0))
            try {
                return searchElements();
            } catch (Exception ex) {return null; }
        return null;
    }

    public boolean isPresent() throws Exception {
        return checkNowElement("Present", elements -> elements.size() > 0);
    }

    public boolean isDisplayed() throws Exception {
        return checkNowElement("Displayed", elements -> {
            for (WebElement el : elements)
                if (el.isDisplayed())
                    return true;
            return false;
        });
    }

    public boolean isEnabled() throws Exception {
        return checkNowElement("Enabled", elements -> {
            for (WebElement el : elements)
                if (el.isDisplayed())
                    return el.isEnabled();
            return false;
        });
    }

    private boolean checkNowElement(String checkName, FuncTT<List<WebElement>, Boolean> checkCriteria) throws Exception {
        return doVIActionResult("Is Element " + checkName, () -> {
                try {
                    getWebDriver().manage().timeouts().implicitlyWait(0, TimeUnit.SECONDS);
                    List<WebElement> elements = doVIActionResult("Is Element" + checkName, this::searchElements,
                        els -> "Find " + els.size() + " element(s)");
                    getWebDriver().manage().timeouts().implicitlyWait(getSite().siteSettings.timeouts.WaitWebElementInSec,
                        TimeUnit.SECONDS);
                    return checkCriteria.invoke(elements);
                } catch(Exception ex) { return false; }
            }, Object::toString);
    }

    public int cashDropTimes;

    private void isClearCashNeeded() {
        if (getSite().siteSettings.useCache) {
            if (cashDropTimes != getSite().siteSettings.cashDropTimes)
                dropCache();
            return;
        }
        _webElement = null;
    }

    private void dropCache() {
        cashDropTimes = getSite().siteSettings.cashDropTimes;
        _webElement = null;
    }

    public WebElement getUniqueWebElement() throws Exception {
        try {
            isClearCashNeeded();
            if (_webElement != null)
                return _webElement;
            List<WebElement> foundElements = waitListOfWebElements();
            if (foundElements == null)
                throw Alerting.throwError(getCantFindElementMessage());
            setWebElement(getUniqueWebElement(foundElements));
            return (checkVisibility)
                    ? waitElementWithState(WebElement::isDisplayed, getWebElement(), getDefaultLogMessage(("Found element stay invisible.")))
                    : getWebElement();
        } catch (Exception ex) { throw Alerting.throwError("Error in getUniqueWebElement" + ex.getMessage()); }
    }

    public IVIElement getVIElement() throws Exception {
        setWebElement(getUniqueWebElement());
        return this;
    }

    public VIElement() throws Exception { }
    public VIElement(VISite site) throws Exception { this.site = site; }
    public VIElement(String name) throws Exception { setName(name); }
    public VIElement(String name, String cssSelector) throws Exception { this(name); setLocator(By.cssSelector(cssSelector)); }
    public VIElement(String name, By byLocator) throws Exception { this(name); setLocator(byLocator); }
    public VIElement(By byLocator) throws Exception { this(); setLocator(byLocator); }
    public VIElement(String name, WebElement webElement) throws Exception { this(name); setWebElement(webElement); }
    public VIElement(WebElement webElement) throws Exception { this("", webElement); }

    public String getDefaultLogMessage(String text) throws Exception {
        return text +  format(" (Name: '%s', Type: '%s', LocatorAttribute: '%s')", getFullName(), getTypeName(), printLocator());
    }

    public static Scenario viScenario = (viElement, actionName, viAction) -> {
        Logger.event(viElement.getDefaultLogMessage(actionName));
        return viAction.invoke();
    };

    protected final <T> T doVIActionResult(String logActionName, FuncT<T> viAction) throws Exception {
        return doVIActionResult(logActionName, viAction, null);
    }
    protected final <T> T doVIActionResult(String actionName, FuncT<T> viAction, FuncTT<T, String> logResult) throws Exception
    {
        try {
            T result = (T) viScenario.invoke(this, actionName, () -> viAction.invoke());
            HighlightSettings demoMode = getSite().siteSettings.demoSettings;
            _waitTimeoutInSec = null;
            openPageName = null;
            if (demoMode != null)
                highlight(demoMode);
            if (logResult != null)
                Logger.event(logResult.invoke(result));
            return result;
            }
        catch (Exception ex)
        {
            throw Alerting.throwError(format(LineBreak + "Failed to do '%s' action. Exception: %s", actionName, ex));
        }
    }

    protected final void doVIAction(String actionName, Action viAction) throws Exception
    {
        try {
            viScenario.invoke(this, actionName, () -> {viAction.invoke(); return null;});
            HighlightSettings demoMode = getSite().siteSettings.demoSettings;
            if (demoMode != null)
                highlight(demoMode);
        }
        catch (Exception ex) {
            throw Alerting.throwError(format(LineBreak + "Failed to do '%s' action. Exception: %s", actionName, ex));
        }
    }

    protected static String openPageName;

    public void setAttribute(String attributeName, String value) throws Exception {
        ((JavascriptExecutor)getWebDriver()).executeScript("arguments[0].setAttribute(arguments[1], arguments[2])",
            getUniqueWebElement(), attributeName, value);
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
        setAttribute("style", format("border: 3px solid %s; background-color: %s;", highlightSettings.FrameColor,
                highlightSettings.BgColor));
        Thread.sleep(highlightSettings.TimeoutInSec * 1000);
        setAttribute("style", orig);
    }

    public static <T extends IVIElement> T cloneVIElement(T element) throws CloneNotSupportedException {
        return (T) element.clone();
    }

    public Object clone() throws CloneNotSupportedException {
        VIElement element = (VIElement) super.clone();
        element.Context = new Pairs<>();
        for (Pair<ContextType, By>locator : Context)
            element.Context.add(locator);
        try { element._locator = copyBy(_locator); } catch (Exception ignore) {}
        return element;
    }
}
