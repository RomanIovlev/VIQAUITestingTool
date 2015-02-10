package ru.viqa.ui_testing.elements.simpleElements;

import ru.viqa.ui_testing.elements.baseClasses.*;
import ru.viqa.ui_testing.elements.interfaces.*;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import static java.lang.String.format;

/**
 * Created by roman.i on 17.10.2014.
 */
public class Checkbox extends HaveValue implements ICheckbox {
    public VIElement CheckSignElement;

    //<input type="checkbox" name="vehicle" value="Bike" id="bike">
    //<label for="bike">I have a bike<br></label>
    private static String LocatorTemplate = "input[type=checkbox][%s=%s]";
    public static String commonLocatorById(String id) { return format(LocatorTemplate, "id", id); }
    public static String commonLocatorByNamed(String id) { return format(LocatorTemplate, "name", id); }
    public static String commonLocatorByClassName(String id) { return format(LocatorTemplate, "class", id); }

    public static String CommonLabelLocator(String id) { return format("label[for='%s']", id); }

    public Checkbox() throws Exception { }
    public Checkbox(String name) throws Exception { super(name); }
    public Checkbox(String name, By bySelector) throws Exception  { super(name, bySelector); CheckSignElement = new VIElement(name + " label", bySelector); }
    public Checkbox(String name, String cssSelector) throws Exception { super(cssSelector, name); CheckSignElement = new VIElement(name + " label", cssSelector); }
    public Checkbox(By bySelector) throws Exception { super(bySelector); CheckSignElement = new VIElement("", bySelector); }
    public Checkbox(String name, WebElement webElement) throws Exception { super(name, webElement); CheckSignElement = new VIElement(name + " label", webElement); }
    public Checkbox(WebElement webElement) throws Exception { super(webElement); CheckSignElement = new VIElement(webElement); }

    public Checkbox(String name, ElementId id) throws Exception {
        super(name, By.cssSelector(commonLocatorById(id.getId())));
    }

    protected void checkAction() throws Exception {
        if (!isCheckedAction())
            click(); 
    }
    protected void uncheckAction() throws Exception {
        if (isCheckedAction())
            click();
    }
    protected boolean isCheckedAction() throws Exception { return getUniqueWebElement().isSelected(); }
    @Override
    protected String getValueAction() throws Exception { return isChecked().toString(); }
    @Override
    protected void setValueAction(String value) throws Exception {
        if (value.toLowerCase().equals("true") || value.toLowerCase().equals("1"))
            check();
        if (value.toLowerCase().equals("false") || value.toLowerCase().equals("0"))
            uncheck();
    }
    protected void clickAction() throws Exception { getUniqueWebElement().click(); }

    public final void check() throws Exception {
        doVIAction("Check Checkbox", this::checkAction);
    }
    public final void uncheck() throws Exception {
        doVIAction("Uncheck Checkbox", this::uncheckAction);
    }
    public final Boolean isChecked() throws Exception {
        return doVIActionResult("IsChecked",
                this::isCheckedAction,
                result -> "Checkbox is " + (result ? "checked" : "unchecked"));
    }
    public final void click() throws Exception {
        doVIAction("Click on element", this::clickAction);
    }
    @Deprecated
    public void clickOpenPage(String openPageName) throws Exception { }
    @Deprecated
    public void clickOnInvisible() throws Exception { }
}
