package VIElements.SimpleElements;

import VIElements.Interfaces.*;
import org.openqa.selenium.*;

import static java.lang.String.format;

/**
 * Created by 12345 on 02.10.2014.
 */
public class Button extends ClickableText implements IButton {
    private static final String LocatorTmpl = "input[type=button][%s=%s]";
    public static String commonLocatorById(String id) { return format(LocatorTmpl, "id", id); }
    public static String commonLocatorByNamed(String id) { return format(LocatorTmpl, "name", id); }
    public static String commonLocatorByClassName(String id) { return format(LocatorTmpl, "class", id); }

    public Button() throws Exception{ super(); TypeName = "Button";}
    public Button(String name) throws Exception { super(name); }
    public Button(String name, String cssSelector) throws Exception { super(name, cssSelector); }
    public Button(String name, By byLocator) throws Exception { super(name, byLocator); }
    public Button(By byLocator) throws Exception { super(byLocator); }
    public Button(By byClickLocator, By byTextLocator) throws Exception { super(byClickLocator, byTextLocator); }
    public Button(String name, WebElement webElement) throws Exception { super(name, webElement); }
    public Button(WebElement webElement) throws Exception { super(webElement); }

    @Override
    public String getTextAction() throws Exception {
        return getUniqueWebElement().getAttribute("value");
    }
}
