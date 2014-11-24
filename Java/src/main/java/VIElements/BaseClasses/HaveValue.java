package VIElements.BaseClasses;

import VIElements.Interfaces.IHaveValue;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

/**
 * Created by roman.i on 28.10.2014.
 */
public abstract class HaveValue extends VIElement implements IHaveValue {
    public HaveValue() throws Exception { super(); }
    public HaveValue(String name) throws Exception { super(name); }
    public HaveValue(String name, String cssSelector) throws Exception { super(name, cssSelector);  }
    public HaveValue(String name, By byLocator) throws Exception { super(name, byLocator); }
    public HaveValue(By byLocator) throws Exception { super(byLocator);  }
    public HaveValue(String name, WebElement webElement) throws Exception { super(name, webElement);  }
    public HaveValue(WebElement webElement) throws Exception { super(webElement);  }

    protected String getValueAction() throws Exception { return ""; }
    protected void setValueAction(String value) throws Exception { }

    public final String getValue() throws Exception { return doVIActionResult("Get value", this::getValueAction, text -> text); }
    public final void setValue(String value) throws Exception {
        doVIAction("Set Value '" + value + "'", () -> setValueAction(value));
    }
}
