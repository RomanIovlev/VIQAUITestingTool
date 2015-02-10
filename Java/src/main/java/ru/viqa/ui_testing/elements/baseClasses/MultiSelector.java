package ru.viqa.ui_testing.elements.baseClasses;

import ru.viqa.ui_testing.common.utils.*;
import ru.viqa.ui_testing.elements.interfaces.*;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static ru.viqa.ui_testing.common.utils.LinqUtils.*;

/**
 * Created by roman.i on 28.10.2014.
 */
public class MultiSelector<T extends IClickable, T1 extends Enum> extends Selector<T, T1> {

    public MultiSelector() throws Exception{ super(); }
    public MultiSelector(String name) throws Exception { super(name); }
    public MultiSelector(String name, String cssSelector) throws Exception { super(name, cssSelector); }
    public MultiSelector(String name, By byLocator) throws Exception { super(name, byLocator); }
    public MultiSelector(By byLocator) throws Exception { super(byLocator); }
    public MultiSelector(String name, WebElement webElement) throws Exception { super(name, webElement); }
    public MultiSelector(WebElement webElement) throws Exception { super(webElement); }

    protected final Map<String, T> getAllNamedElements() throws Exception {
        Map<String, T> namedMap = new HashMap<>();
        if (map != null && map.size() != 0)
            for (Map.Entry<String, String> entry : map.entrySet())
                namedMap.put(entry.getKey(), getElement(entry.getKey()));
        return namedMap;
    }

    protected List<String> areSelectedAction() throws Exception {
        return (List<String>) selectMap(where(getAllNamedElements(),
                cl -> cl.getValue().getWebElement().isSelected()), Map.Entry::getKey);
    }
    public final List<String> areSelected() throws Exception {
        return doVIActionResult("Are selected", this::areSelectedAction, PrintUtils::print);
    }
    protected List<String> areNotSelectedAction() throws Exception {
        return (List<String>) selectMap(where(getAllNamedElements(),
                cl -> !cl.getValue().getWebElement().isSelected()), Map.Entry::getKey);
    }
    public final List<String> areNotSelected() throws Exception {
        return doVIActionResult("Are not selected", this::areNotSelectedAction, PrintUtils::print);
    }
    public final void select(String... valueNames) throws Exception { doVIAction("Select", () -> {
        for (String value : valueNames)
            selectAction(value);
        });
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
