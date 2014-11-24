package Common.Alertings;

import Common.Interfaces.IAlerting;
import SiteClasses.VISite;

/**
 * Created by roman.i on 17.11.2014.
 */
public class DefaultAlerting implements IAlerting {
    public Exception ThrowError(String errorMsg) throws Exception {
        VISite.Logger.Error(errorMsg);
        throw new Exception(errorMsg);
    }

}
