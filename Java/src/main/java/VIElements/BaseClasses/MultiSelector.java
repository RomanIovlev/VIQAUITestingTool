package VIElements.BaseClasses;

import Common.Utils.PrintUtils;
import Common.Utils.LinqUtils;
import VIElements.Interfaces.IVIElement;
import VIElements.SimpleElements.Clickable;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import java.util.List;

import static Common.Utils.LinqUtils.*;

/**
 * Created by roman.i on 28.10.2014.
 */
public class MultiSelector<T extends Clickable> extends Selector<T> {

    public MultiSelector() throws Exception{ super(); TypeName = "MultiSelector";}
    public MultiSelector(String name) throws Exception { super(name); }
    public MultiSelector(String name, String cssSelector) throws Exception { super(name, cssSelector); }
    public MultiSelector(String name, By byLocator) throws Exception { super(name, byLocator); }
    public MultiSelector(By byLocator) throws Exception { super(byLocator); }
    public MultiSelector(String name, WebElement webElement) throws Exception { super(name, webElement); }
    public MultiSelector(WebElement webElement) throws Exception { super(webElement); }

    protected List<String> areSelectedAction() throws Exception {
        return (List<String>) LinqUtils.select(where(getListOfElements(), cl -> cl.getWebElement().isSelected()), IVIElement::getName);
    }
    public final List<String> areSelected() throws Exception {
        return doVIActionResult("Are selected", this::areSelectedAction, PrintUtils::print);
    }
    protected List<String> areNotSelectedAction() throws Exception {
        return (List<String>) LinqUtils.select(where(getListOfElements(), cl -> !cl.getWebElement().isSelected()), IVIElement::getName);
    }
    public final List<String> areNotSelected() throws Exception {
        return doVIActionResult("Are not selected", this::areNotSelectedAction, PrintUtils::print);
    }
    // TODO
    @Override
    protected void setValueAction(String value) {

    // TODO
    }
    @Override
    protected String getValueAction() {
        return "";
    }
}
