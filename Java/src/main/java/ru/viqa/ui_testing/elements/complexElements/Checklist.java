package ru.viqa.ui_testing.elements.complexElements;

import ru.viqa.ui_testing.common.funcInterfaces.*;
import ru.viqa.ui_testing.common.utils.LinqUtils;
import ru.viqa.ui_testing.elements.baseClasses.MultiSelector;
import ru.viqa.ui_testing.elements.interfaces.ICheckList;
import ru.viqa.ui_testing.elements.interfaces.ICheckbox;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import static java.util.Arrays.asList;
import static java.util.stream.Collectors.toList;
import static ru.viqa.ui_testing.common.utils.LinqUtils.select;
import static ru.viqa.ui_testing.common.utils.PrintUtils.*;
import static ru.viqa.ui_testing.common.utils.LinqUtils.foreach;

/**
 * Created by roman.i on 28.10.2014.
 */
public class Checklist<T extends Enum> extends MultiSelector<ICheckbox, T> implements ICheckList<T> {
    public Checklist() throws Exception{ super(); }
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
    protected void checkOnlyAction(String... values) throws Exception {
        uncheckAll();
        checkGroupAction(values);
    }
    protected void uncheckOnlyAction(String... values) throws Exception {
        checkAll();
        uncheckGroupAction(values);
    }

    public void checkGroup(String... values) throws Exception {
        doVIAction("Check Group: " + print(asList(values)), () -> checkGroupAction(values));
    }
    public void checkGroup(T... values) throws Exception {
        enumArrayToString(values, this::checkGroup);
    }
    public void checkOnly(String... values) throws Exception {
        doVIAction("Check Only: " + print(asList(values)), () -> checkOnlyAction(values));
    }
    public void checkOnly(T... values) throws Exception {
        enumArrayToString(values, this::checkOnly);
    }
    public void checkAll() throws Exception {
        checkGroup((String[]) map.keySet().stream().collect(toList()).toArray());
    }
    public void checkAll(Class<T> enumType) throws Exception {
        Collection<String> values = LinqUtils.select(enumType.getEnumConstants(), this::getEnumValue);
        checkGroup(values.toArray(new String[values.size()]));
    }
    public void uncheckGroup(String... values) throws Exception {
        doVIAction("Uncheck Group: " + print(asList(values)), () -> uncheckGroupAction(values));
    }
    public void uncheckGroup(T... values) throws Exception {
        enumArrayToString(values, this::uncheckGroup);
    }
    public void uncheckOnly(String... values) throws Exception {
        doVIAction("Uncheck Only: " + print(asList(values)), () -> uncheckOnlyAction(values));
    }
    public void uncheckOnly(T... values) throws Exception {
        enumArrayToString(values, this::uncheckOnly);
    }
    public void uncheckAll() throws Exception {
        uncheckGroup((String[]) map.keySet().stream().collect(toList()).toArray());
    }
    public void uncheckAll(Class<T> enumType) throws Exception {
        Collection<String> values = LinqUtils.select(enumType.getEnumConstants(), this::getEnumValue);
        uncheckGroup(values.toArray(new String[values.size()]));
    }

    private void enumArrayToString(T[] array, ActionT<String[]> func) throws Exception {
        List<String> list = new ArrayList<>();
        for (T value : array)
            list.add(getEnumValue(value));
        func.invoke(list.toArray(new String[list.size()]));
    }

}
