package VIElements.BaseClasses;

import VIElements.Interfaces.*;
import VIElements.SimpleElements.Clickable;
import org.openqa.selenium.*;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static Common.Utils.ReflectionUtils.isClass;
import static Common.Utils.LinqUtils.*;

/**
 * Created by roman.i on 03.10.2014.
 */

public class Selector<T extends Clickable> extends HaveValue implements ISelector {
    protected Map<String,String> map;

    public Selector() throws Exception{ super(); TypeName = "Selector";}
    public Selector(String name) throws Exception { super(name); }
    public Selector(String name, String cssSelector) throws Exception { super(name, cssSelector); }
    public Selector(String name, By byLocator) throws Exception { super(name, byLocator); }
    public Selector(By byLocator) throws Exception { super(byLocator); }
    public Selector(String name, WebElement webElement) throws Exception { super(name, webElement); }
    public Selector(WebElement webElement) throws Exception { super(webElement); }

    public T getElement(String name) throws Exception {
        setTemplateId(getTemplateIdFromMap(name));
        Clickable ce = new Clickable(getLocator());
        ce.setName(name);
        ce.setSite(getSite());
        return (T)ce;
    }

    private String getTemplateIdFromMap(String name) {
        return  (map != null && map.containsKey(name)) ? map.get(name) : name;
    }

    protected void selectAction(String name) throws Exception {
        getElement(name).click();
    }
    protected void getElementLabelAction(String name) throws Exception {
        IClickable cl = getElement(name);
        if (isClass(cl.getClass(), IText.class))
            ((IText) getElement(name)).getText();
        else
            throw new Exception(getFullName() + "is not a text. Please override getElementLabelAction or correct locator");
    }
    protected String isSelectedAction() throws Exception {
        return first(getListOfElements(), cl -> cl.getWebElement().isSelected()).getName();
    }
    @Override
    protected String getValueAction() throws Exception { return isSelectedAction(); }
    @Override
    protected void setValueAction(String value) throws Exception { select(value);}

    public final Selector<T> setValuesMap(Map<String,String> valuesMap) {
        map = valuesMap;
        return this;
    }
    public final List<IClickable> getListOfElements() throws Exception {
        List<IClickable> list = new ArrayList<>();
        if (map != null && map.size() != 0)
            for (Map.Entry<String, String> entry : map.entrySet())
                list.add(getElement(entry.getValue()));
        return list;
    }
    public final void select(String valueName) throws Exception { doVIAction("Select", () -> selectAction(valueName)); }
    public final void getLabel(String valueName) throws Exception {
        doVIAction("Get label", () -> getElementLabelAction(valueName));
    }
    public final String isSelected() throws Exception {
        return doVIActionResult("IsSelected", this::isSelectedAction, text -> text);
    }
}
