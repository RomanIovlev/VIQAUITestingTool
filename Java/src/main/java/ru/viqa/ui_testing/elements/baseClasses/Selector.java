package ru.viqa.ui_testing.elements.baseClasses;

import ru.viqa.ui_testing.common.utils.PrintUtils;
import ru.viqa.ui_testing.elements.interfaces.*;
import org.openqa.selenium.*;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static ru.viqa.ui_testing.common.utils.ReflectionUtils.isClass;
import static ru.viqa.ui_testing.common.utils.LinqUtils.*;

/**
 * Created by roman.i on 03.10.2014.
 */

public class Selector<T extends IClickable, T1 extends Enum> extends HaveValue implements ISelector<T1> {
    protected Map<String,String> map = new HashMap<>();

    public Selector() throws Exception{ super(); }
    public Selector(String name) throws Exception { super(name); }
    public Selector(String name, String cssSelector) throws Exception { super(name, cssSelector); }
    public Selector(String name, By byLocator) throws Exception { super(name, byLocator); }
    public Selector(By byLocator) throws Exception { super(byLocator); }
    public Selector(String name, WebElement webElement) throws Exception { super(name, webElement);  }
    public Selector(WebElement webElement) throws Exception { super(webElement);  }

    protected T getElement(String name) throws Exception {
        setTemplateId(getTemplateIdFromMap(name));
        Clickable ce = new Clickable(getLocator());
        ce.setName(name);
        ce.setSite(getSite());
        return (T)ce;
    }

    private String getTemplateIdFromMap(String name) {
        return  (map != null && map.containsKey(name)) ? map.get(name) : name.toString();
    }

    protected void selectAction(String name) throws Exception {
        getElement(name).click();
    }
    protected String getElementTextAction(String name) throws Exception {
        IClickable cl = getElement(name);
        if (isClass(cl.getClass(), IText.class))
            return ((IText) getElement(name)).getText();
        else
            throw new Exception(getFullName() + "is not a text. Please override getElementTextAction or correct locator");
    }
    protected String isSelectedAction() throws Exception {
        return first(getAllElements(), cl -> cl.getWebElement().isSelected()).getName();
    }
    protected List<String> getAllLabelsAction() throws Exception {
        List<String> result = new ArrayList<>();
        for (String value : map.keySet())
            result.add(getElementTextAction(value));
        return result;
    }

    @Override
    protected String getValueAction() throws Exception { return isSelectedAction(); }
    @Override
    protected void setValueAction(String value) throws Exception { selectAction(value);}

    public final ISelector setValuesMap(Object[][] valuesMap) throws IllegalAccessException, InstantiationException {
        if (valuesMap == null || !valuesMap.getClass().isArray()) return this;
        for (Object[] value : valuesMap)
            if (value != null && value.length == 2 && value[0].getClass() == String.class && value[1].getClass() == String.class)
                map.put((String)value[0], (String)value[1]);
        return this;
    }
    protected final List<T> getAllElements() throws Exception {
        List<T> list = new ArrayList<>();
        if (map != null && map.size() != 0)
            for (Map.Entry<String, String> entry : map.entrySet())
                list.add(getElement(entry.getKey()));
        return list;
    }
    public final void select(String valueName) throws Exception { doVIAction("Select", () -> selectAction(valueName)); }
    public final void select(T1 valueName) throws Exception { doVIAction("Select", () -> selectAction(getEnumValue(valueName))); }
    protected String getEnumValue(T1 enumWithValue) throws Exception {
        Field field;
        try { field = enumWithValue.getClass().getField("value");
            if (field.getType() != String.class)
                throw new Exception("Can't get Value from enum");
        } catch (Exception ex) { return enumWithValue.toString(); }
        return (String) field.get(enumWithValue);
    }

    public final String getText(String valueName) throws Exception {
        return doVIActionResult("Get label", () -> getElementTextAction(valueName), text -> text);
    }
    public final List<String> getAllLabels() throws Exception {
        return doVIActionResult("Get Labels", this::getAllLabelsAction, PrintUtils::print);
    }
    public final String isSelected() throws Exception {
        return doVIActionResult("IsSelected", this::isSelectedAction, text -> text);
    }
}
