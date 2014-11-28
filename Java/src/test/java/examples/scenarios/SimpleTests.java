package examples.scenarios;

import examples.data.User;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import static examples.setup.TestsSetup.mySite;
import static examples.setup.TestsSetup.newBrowser;
import static examples.site.MySite.*;

/**
 * Created by roman.i on 07.11.2014.
 */
public class SimpleTests {

    @BeforeMethod
    public void before() throws Exception { newBrowser(); }

    @Test
    public void simpleTest() throws Exception {
        User user = new User("myLogin", "myPassword");

        mySite.mainPage.open();
        loginForm.login(user);

        mainPage.checkUrl();
        mainPage.searchField.newInput("My Search");
        mainPage.agreeTerms.check();
        mainPage.city.select("Saint Petersburg");
        mainPage.searchButton.click();

        resultsPage.searchResults.cell("Total", 1);


    }

}
