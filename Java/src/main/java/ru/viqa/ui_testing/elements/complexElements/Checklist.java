package ru.viqa.ui_testing.elements.complexElements;

import ru.viqa.ui_testing.elements.baseClasses.MultiSelector;
import ru.viqa.ui_testing.elements.interfaces.ICheckList;
import ru.viqa.ui_testing.elements.simpleElements.Checkbox;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import static ru.viqa.ui_testing.common.utils.PrintUtils.print;
import static ru.viqa.ui_testing.common.utils.LinqUtils.foreach;

/**
 * Created by roman.i on 28.10.2014.
 */
public class Checklist extends MultiSelector<Checkbox> implements ICheckList {
    public Checklist() throws Exception{ super(); TypeName = "Checklist";}
    public Checklist(String name) throws Exception { super(name); }
    public Checklist(String name, String cssSelector) throws Exception { super(name, cssSelector); }
    public Checklist(String name, By byLocator) throws Exception { super(name, byLocator); }
    public Checklist(By byLocator) throws Exception { super(byLocator); }
    public Checklist(String name, WebElement webElement) throws Exception { super(name, webElement); }
    public Checklist(WebElement webElement) throws Exception { super(webElement); }

    protected void checkGroupAction(String... values) throws Exception {
        foreach(values, val -> getElement(val).check()); }
    protected void uncheckGroupAction(String... values) throws Exception {
        foreach(values, val -> getElement(val).uncheck()); }
    protected void clearAction() throws Exception { }
    protected void checkOnlyAction(String... values) throws Exception {
        clearAction();
        checkGroupAction(values);
    }
    protected void uncheckOnlyAction(String... values) throws Exception {
        clearAction();
        uncheckGroupAction(values);
    }

    public void checkGroup(String... values) throws Exception {
        doVIAction("Check Group: " + print(values), () -> checkGroupAction(values));
    }
    public void uncheckGroup(String... values) throws Exception {
        doVIAction("Uncheck Group: " + print(values), () -> uncheckGroupAction(values));
    }
    public void clear(String... values) throws Exception {
        doVIAction("Clear Checklist", this::clearAction);
    }
    public void checkOnly(String... values) throws Exception {
        doVIAction("Check Only: " + print(values), () -> checkOnlyAction(values));
    }
    public void uncheckOnly(String... values) throws Exception {
        doVIAction("Uncheck Only: " + print(values), () -> uncheckOnlyAction(values));
    }
}
