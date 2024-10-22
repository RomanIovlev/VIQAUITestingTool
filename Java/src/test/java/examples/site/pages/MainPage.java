package examples.site.pages;

import ru.viqa.ui_testing.page_objects.VIPage;
import ru.viqa.ui_testing.annotations.Page;
import ru.viqa.ui_testing.elements.interfaces.*;
import org.openqa.selenium.support.FindBy;

/**
 * Created by roman.i on 07.11.2014.
 */
@Page(url = "http://mySite.com")
public class MainPage extends VIPage{
    @FindBy(xpath = "//div[@class='searchField']")
    public ITextField searchField;
    @FindBy(xpath = "//div[@class='agreeTerms']")
    public ICheckbox agreeTerms;
    @FindBy(xpath = "//div[@class='city']")
    public IDropDown city;
    @FindBy(xpath = "//div[@class='searchButton']")
    public IButton searchButton;

    public MainPage() throws Exception { }
}
