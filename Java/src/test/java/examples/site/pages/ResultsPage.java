package examples.site.pages;

import ru.viqa.ui_testing.page_objects.VIPage;
import ru.viqa.ui_testing.elements.complexElements.Dropdown;
import ru.viqa.ui_testing.elements.interfaces.IDropDown;
import ru.viqa.ui_testing.elements.interfaces.ITable;
import ru.viqa.ui_testing.elements.simpleElements.Table;
import org.openqa.selenium.support.FindBy;
import org.openqa.selenium.support.ui.Select;

/**
 * Created by 12345 on 20.11.2014.
 */
public class ResultsPage extends VIPage {

    @FindBy(xpath = "//div[@class='searchResults']")
    public ITable searchResults = new Table();
    @FindBy(xpath = "//div[@class='searchResults']")
    public IDropDown cityFilter = new Dropdown() {

        @Override
        protected void selectAction(String name) throws Exception {
            if (_rootLocator != null) {
                getWebDriver().findElement(_rootLocator).click();
                getElement(name).click();
            }
            else
                new Select(getWebDriver().findElement(getLocator())).selectByValue(name);
        }
    };

    public ResultsPage() throws Exception {}
}
