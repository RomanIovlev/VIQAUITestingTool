package ru.viqa.ui_testing.elements.interfaces;

import ru.viqa.ui_testing.common.funcInterfaces.*;
import ru.viqa.ui_testing.page_objects.VISite;
import org.openqa.selenium.*;

/**
 * Created by 12345 on 10.05.2014.
 */
public interface IVIElement {
    WebElement getWebElement();
    boolean isPresent() throws Exception;
    boolean isDisplayed() throws Exception;
    VISite getSite();
    WebDriver getWebDriver() throws Exception;
    //List<T> GetElements<T>();
    String getName() throws Exception;
    void fillFrom(Object obj) throws Exception;
    WebElement getUniqueWebElement() throws Exception;

    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc) throws Exception;
    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement) throws Exception;
    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec) throws Exception;
    public boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec, long retryTimeoutInMSec) throws Exception;
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc) throws Exception;
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement) throws Exception;
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, String msg) throws Exception;
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec) throws Exception;
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec, long retryTimeoutInMSec) throws Exception;
    public WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec, long retryTimeoutInMSec, String msg) throws Exception;
    /*List<WebElement> searchElements() throws Exception;
    List<WebElement> searchElements(By locator) throws Exception;
    By getLocator() throws Exception;
    void setWaitTimeout(long waitTimeoutInSec);
    void fillSubElements_(Map<IHaveValue, Object> values);
    void fillSubElements(Map<String, Object> values);
    void fillSubElements(Object data);
    void fillSubElement(String name, String value);*/
}
