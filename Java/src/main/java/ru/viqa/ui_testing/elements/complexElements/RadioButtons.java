package ru.viqa.ui_testing.elements.complexElements;

import ru.viqa.ui_testing.elements.baseClasses.Selector;
import ru.viqa.ui_testing.elements.interfaces.IRadioButtons;
import ru.viqa.ui_testing.elements.simpleElements.Clickable;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import static java.lang.String.format;

/**
 * Created by roman.i on 28.10.2014.
 */
public class RadioButtons<T extends Clickable> extends Selector<T> implements IRadioButtons {

    private static String RadioButtonTemplate = "input[type=radio][id={0}]";
    private static String LocatorTmpl = "input[type=radio][{0}={1}]";

    public static String commonLocatorById(String id) { return format(LocatorTmpl, "id", id); }
    public static String commonLocatorByNamed(String id) { return format(LocatorTmpl, "name", id); }
    public static String commonLocatorByClassName(String id) { return format(LocatorTmpl, "class", id); }

    public String getSelectedItem() {return "";}

    public RadioButtons() throws Exception { super(); TypeName = "RadioButtons"; }
    public RadioButtons(String name) throws Exception { super(name); }
    public RadioButtons(String name, String cssSelector) throws Exception { super(name, cssSelector); }
    public RadioButtons(String name, By byLocator) throws Exception { super(name, byLocator); }
    public RadioButtons(By byLocator) throws Exception { super(byLocator); }
    public RadioButtons(String name, WebElement webElement) throws Exception { super(name, webElement); }
    public RadioButtons(WebElement webElement) throws Exception { super(webElement); }

}
