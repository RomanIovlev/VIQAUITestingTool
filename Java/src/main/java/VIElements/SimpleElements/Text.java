package VIElements.SimpleElements;

import VIElements.BaseClasses.*;
import VIElements.Interfaces.*;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

/**
 * Created by roman.i on 29.09.2014.
 */
public class Text extends HaveValue implements IText {
    public Text() throws Exception { super(); TypeName = "Text"; }
    public Text(String name) throws Exception { super(name); }
    public Text(String name, String cssSelector) throws Exception  { super(name, cssSelector); }
    public Text(String name, By byLocator) throws Exception { super(name, byLocator); }
    public Text(By byLocator) throws Exception { super(byLocator);}
    public Text(String name, WebElement webElement) throws Exception { super(name, webElement);}
    public Text(WebElement webElement) throws Exception  { super(webElement);}

    protected String getTextAction() throws Exception { return getUniqueWebElement().getText(); }
    @Override
    protected String getValueAction() throws Exception { return getTextAction(); }
    /*
        public Func<Object, Object> FillRule { set; get; }
        public static Func<Object, Object> ToFillRule<T>(Func<T, Object> typeFillRule)
        {
            return o -> new Func<T, object>(typeFillRule)((T)o);
        }*/
    public final String getText() throws Exception{
        return doVIActionResult("Get text", this::getTextAction, text -> text);
    }

}
