package examples.site;

import ru.viqa.ui_testing.page_objects.BrowserType;
import ru.viqa.ui_testing.page_objects.VISite;
import ru.viqa.ui_testing.annotations.Page;
import ru.viqa.ui_testing.elements.simpleElements.Clickable;
import examples.site.pages.LoginForm;
import examples.site.pages.MainPage;
import examples.site.pages.ResultsPage;

/**
 * Created by roman.i on 07.11.2014.
 */
public class MySite extends VISite {
    public MySite(BrowserType type) throws Exception{ super(type); }

    public static LoginForm loginForm;
    public static MainPage mainPage;
    @Page(url = "http://mySite.com/results")
    public static ResultsPage resultsPage;

    public static Clickable MySiteLogo;
}
