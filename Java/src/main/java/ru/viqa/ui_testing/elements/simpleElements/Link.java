package ru.viqa.ui_testing.elements.simpleElements;

import ru.viqa.ui_testing.elements.interfaces.*;
import org.openqa.selenium.*;

import static java.lang.String.format;

/**
 * Created by 12345 on 02.10.2014.
 */
public class Link extends ClickableText implements ILink {
    private static final String LocatorTmpl = "a[%s=%s]";
    public static String commonLocatorById(String id) { return format(LocatorTmpl, "id", id); }
    public static String commonLocatorByNamed(String id) { return format(LocatorTmpl, "name", id); }
    public static String commonLocatorByClassName(String id) { return format(LocatorTmpl, "class", id); }

    public Link() throws Exception{ super(); TypeName = "Link";}
    public Link(String name) throws Exception { super(name); }
    public Link(String name, String cssSelector) throws Exception { super(name, cssSelector); }
    public Link(String name, By byLocator) throws Exception { super(name, byLocator); }
    public Link(By byLocator) throws Exception { super(byLocator); }
    public Link(By byClickLocator, By byTextLocator) throws Exception { super(byClickLocator, byTextLocator); }
    public Link(String name, WebElement webElement) throws Exception { super(name, webElement); }
    public Link(WebElement webElement) throws Exception { super(webElement); }

    protected String getReferenceAction() throws Exception { return getUniqueWebElement().getAttribute("href"); }

    public final String getReference() throws Exception {
        return doVIActionResult("Get Reference", this::getReferenceAction, href -> "Get href of link '" + href + "'");
    }
}
