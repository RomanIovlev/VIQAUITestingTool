package Common;

import Common.FuncInterfaces.FuncT;
import VIElements.BaseClasses.VIElement;

/**
 * Created by 12345 on 20.11.2014.
 */
public interface Scenario {
    public Object invoke(VIElement viElement, String actionName, FuncT<Object> viAction) throws Exception;/* {
        return viAction.invoke();
    }*/
}
