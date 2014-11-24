package Common;

import Common.FuncInterfaces.FuncT;
import VIElements.BaseClasses.VIElement;

/**
 * Created by 12345 on 20.11.2014.
 */
public class Scenario {
    public <T> T invoke(VIElement viElement, String actionName, FuncT<T> viAction) throws Exception {
        return viAction.invoke();
    }
}
