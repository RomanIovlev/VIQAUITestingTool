package ru.viqa.ui_testing.elements.simpleElements;

import ru.viqa.ui_testing.elements.interfaces.*;
import org.openqa.selenium.*;

import static java.lang.String.format;

/**
 * Created by 12345 on 02.10.2014.
 */
public class TextField extends Text implements ITextField{

    public final static String LocatorTmpl = "input[type=text][%s=%s]";
    public static String commonLocatorById(String id) { return format(LocatorTmpl, "id", id); }
    public static String commonLocatorByNamed(String id) { return format(LocatorTmpl, "name", id); }
    public static String commonLocatorByClassName(String id) { return format(LocatorTmpl, "class", id); }

    public TextField() throws Exception { super(); TypeName = "TextField"; }
    public TextField(String name) throws Exception { super(name); }
    public TextField(String name, String cssSelector) throws Exception  { super(name, cssSelector); }
    public TextField(String name, By byLocator) throws Exception { super(name, byLocator); }
    public TextField(By byLocator) throws Exception { super(byLocator);}
    public TextField(String name, WebElement webElement) throws Exception { super(name, webElement);}
    public TextField(WebElement webElement) throws Exception  { super(webElement);}

    protected void inputAction(String text) throws Exception { getUniqueWebElement().sendKeys(text); }
    protected void clearAction() throws Exception { getUniqueWebElement().clear(); }
    @Override
    protected void setValueAction(String value) throws Exception {
        if (value == null) return;
        newInput(value);
    }

    @Override
    public String getTextAction() throws Exception {
        return getUniqueWebElement().getAttribute("value");
    }

    public final void input(String text) throws Exception {
        doVIAction("Input text '" + text + "' in text area", () -> inputAction(text));
    }

    public final void newInput(String text) throws Exception {
        doVIAction("New input text '" + text + "' in text area", () -> {
            clearAction();
            inputAction(text);
        });
    }

    public final void clear() throws Exception {
        doVIAction("Clear text area", this::clearAction);
    }
}
