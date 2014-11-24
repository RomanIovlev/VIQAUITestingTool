package test.setup;

import Common.FuncInterfaces.FuncT;
import Common.Scenario;
import SiteClasses.VISite;
import VIElements.BaseClasses.VIElement;
import org.testng.annotations.AfterSuite;
import org.testng.annotations.BeforeSuite;
import test.site.MySite;

import static SiteClasses.BrowserType.Chrome;

/**
 * Created by roman.i on 07.11.2014.
 */
public class TestsSetup {
    public static MySite mySite;

    @BeforeSuite(alwaysRun = true)
    public static void setUp() throws Exception {
        mySite = new MySite(Chrome);

        VIElement.viScenario = new Scenario() {
            @Override
            public <T> T invoke(VIElement viElement, String actionName, FuncT<T> viAction) throws Exception {
                VISite.Logger.Event(viElement.getDefaultLogMessage(actionName));
                return viAction.invoke();
            }
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
