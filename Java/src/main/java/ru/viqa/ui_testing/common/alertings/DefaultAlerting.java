package ru.viqa.ui_testing.common.alertings;

import org.testng.Assert;
import ru.viqa.ui_testing.common.interfaces.IAlerting;
import ru.viqa.ui_testing.page_objects.VISite;

import static org.testng.Assert.assertTrue;
import static ru.viqa.ui_testing.page_objects.VISite.Logger;

/**
 * Created by roman.i on 17.11.2014.
 */
public class DefaultAlerting implements IAlerting {
    public Exception throwError(String errorMsg) throws Exception {
        Logger.error(errorMsg);
        assertTrue(false, errorMsg);
        throw new Exception(errorMsg);
    }

}
