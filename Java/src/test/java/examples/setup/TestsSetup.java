package examples.setup;

import ru.viqa.ui_testing.page_objects.VISite;
import ru.viqa.ui_testing.elements.baseClasses.VIElement;
import org.testng.annotations.AfterSuite;
import org.testng.annotations.BeforeSuite;
import examples.site.MySite;

import static ru.viqa.ui_testing.page_objects.BrowserType.Chrome;

/**
 * Created by roman.i on 07.11.2014.
 */
public class TestsSetup {
    public static MySite mySite;

    @BeforeSuite(alwaysRun = true)
    public static void setUp() throws Exception {
        mySite = new MySite(Chrome);

        VIElement.viScenario = (viElement, actionName, viAction) -> {
                VISite.Logger.Event(viElement.getDefaultLogMessage(actionName));
                return viAction.invoke();
            };
        //killAllRunWebDrivers();
    }

    @AfterSuite
    public static void tearDown() throws Exception {
        VISite.disposeAll();
    }

    public static void newBrowser() throws Exception {
    }
}
