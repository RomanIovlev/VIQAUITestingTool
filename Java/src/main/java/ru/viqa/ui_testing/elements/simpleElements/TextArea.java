package ru.viqa.ui_testing.elements.simpleElements;

import ru.viqa.ui_testing.elements.interfaces.*;
import org.openqa.selenium.*;

import static java.lang.String.format;

/**
 * Created by 12345 on 02.10.2014.
 */
public class TextArea extends ClickableText implements ITextArea {

    public final static String LocatorTmpl = "textarea[%s=%s]";
    public static String commonLocatorById(String id) { return format(LocatorTmpl, "id", id); }
    public static String commonLocatorByNamed(String id) { return format(LocatorTmpl, "name", id); }
    public static String commonLocatorByClassName(String id) { return format(LocatorTmpl, "class", id); }

    public TextArea() throws Exception { super(); TypeName = "TextArea"; }
    public TextArea(String name) throws Exception { super(name); }
    public TextArea(String name, String cssSelector) throws Exception  { super(name, cssSelector); }
    public TextArea(String name, By byLocator) throws Exception { super(name, byLocator); }
    public TextArea(By byLocator) throws Exception { super(byLocator);}
    public TextArea(String name, WebElement webElement) throws Exception { super(name, webElement);}
    public TextArea(WebElement webElement) throws Exception  { super(webElement);}

    protected void inputAction(String text) throws Exception { getUniqueWebElement().sendKeys(text); }
    protected void clearAction() throws Exception { getUniqueWebElement().clear(); }
    protected void setValueAction(Object value) throws Exception {
        if (value == null) return;
        newInput(value.toString());
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
