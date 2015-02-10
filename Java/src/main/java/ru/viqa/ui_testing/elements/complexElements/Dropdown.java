package ru.viqa.ui_testing.elements.complexElements;

import ru.viqa.ui_testing.elements.baseClasses.Selector;
import ru.viqa.ui_testing.elements.interfaces.IClickable;
import ru.viqa.ui_testing.elements.interfaces.IDropdown;
import ru.viqa.ui_testing.elements.baseClasses.Clickable;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import org.openqa.selenium.support.ui.Select;

/**
 * Created by roman.i on 27.10.2014.
 */
public class Dropdown<T extends Enum> extends Selector<IClickable, T> implements IDropdown<T> {
    protected By _rootLocator = null;

    public Dropdown() throws Exception{ super(); }
    public Dropdown(String name) throws Exception { super(name); }
    public Dropdown(String name, String cssSelector) throws Exception { super(name, cssSelector); }
    public Dropdown(String name, By rootLocator, By byLocator) throws Exception { super(name, byLocator); _rootLocator = rootLocator; }
    public Dropdown(String name, By byLocator) throws Exception { super(name, byLocator); }
    public Dropdown(By rootLocator, By byLocator) throws Exception {
        super(byLocator); _rootLocator = rootLocator; }
    public Dropdown(By byLocator) throws Exception { super(byLocator); }
    public Dropdown(String name, WebElement webElement) throws Exception { super(name, webElement); }
    public Dropdown(WebElement webElement) throws Exception { super(webElement); }

    @Override
    protected void selectAction(String name) throws Exception {
        if (_rootLocator != null) {
            getSearchContext().findElement(_rootLocator).click();
            getElement(name).click();
        }
        else
            new Select(getWebDriver().findElement(getLocator())).selectByValue(map.get(name));
    }

}
