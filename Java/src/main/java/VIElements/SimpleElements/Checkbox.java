package VIElements.SimpleElements;

import Common.FuncInterfaces.Action;
import Common.FuncInterfaces.FuncT;
import SiteClasses.VISite;
import VIElements.BaseClasses.ElementId;
import VIElements.BaseClasses.VIElement;
import VIElements.Interfaces.*;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import static java.lang.String.format;

/**
 * Created by roman.i on 17.10.2014.
 */
public class Checkbox extends ClickableText implements ICheckbox, ISelected {
    public VIElement CheckSignElement;

    @Override
    public String getValueAction() throws Exception { return isChecked().toString(); }

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
        TypeName = "Checkbox";
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
    protected boolean isSelectedAction() throws Exception { return isCheckedAction(); }

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

    public final Boolean isSelected() throws Exception {
        return doVIActionResult("IsSelected", this::isSelectedAction, bool -> bool ? "True" : "False"); }

   /* public void SetValue<T>(T value)
    {
        if (value == null) return;
        var val = value.ToString();
        if (!new [] {"check", "uncheck", "true", "false"}.Contains(val = val.ToLower()))
        throw VISite.Alerting.ThrowError("Wrong Value type. For Checkbox availabel only 'check', 'uncheck', 'true', 'false'values of type String");
        if (val == "check" || val == "true")
            Check();
        else
            Uncheck();
    }*/

}
