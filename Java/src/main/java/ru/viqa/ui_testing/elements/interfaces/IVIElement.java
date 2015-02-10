package ru.viqa.ui_testing.elements.interfaces;

import ru.viqa.ui_testing.common.funcInterfaces.*;
import ru.viqa.ui_testing.page_objects.HighlightSettings;
import ru.viqa.ui_testing.page_objects.VISite;
import org.openqa.selenium.*;

import java.util.List;

/**
 * Created by 12345 on 10.05.2014.
 */
public interface IVIElement extends Cloneable {
    WebElement getUniqueWebElement() throws Exception;
    WebElement getWebElement();
    boolean isPresent() throws Exception;
    boolean isDisplayed() throws Exception;
    boolean isEnabled() throws Exception;
    void fillFrom(Object obj) throws Exception;
    VISite getSite();
    WebDriver getWebDriver() throws Exception;
    String getName() throws Exception;
    IVIElement setTemplateId(String tId);
    IVIElement setWaitTimeout(long waitTimeoutInSec);
    List<WebElement> waitListOfWebElements() throws Exception;

    boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc) throws Exception;
    boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement) throws Exception;
    boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec) throws Exception;
    boolean waitElementState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec, long retryTimeoutInMSec) throws Exception;
    WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc) throws Exception;
    WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement) throws Exception;
    WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, String msg) throws Exception;
    WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec) throws Exception;
    WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec, long retryTimeoutInMSec) throws Exception;
    WebElement waitElementWithState(FuncTT<WebElement, Boolean> waitFunc, WebElement webElement, long timeoutInSec, long retryTimeoutInMSec, String msg) throws Exception;

    void setAttribute(String attributeName, String value) throws Exception;

    void highlight() throws Exception;
    void highlight(String bgColor) throws Exception;
    void highlight(String bgColor, String frameColor) throws Exception;
    void highlight(String bgColor, String frameColor, int timeoutInSec) throws Exception;

    void highlight(HighlightSettings highlightSettings) throws Exception;
    Object clone() throws CloneNotSupportedException;
}
