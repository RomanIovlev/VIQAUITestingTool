package ru.viqa.ui_testing.common.utils;

import ru.viqa.ui_testing.common.funcInterfaces.FuncTT;
import ru.viqa.ui_testing.page_objects.VISite;
import org.openqa.selenium.By;

import java.util.*;

import static ru.viqa.ui_testing.common.utils.PrintUtils.*;
import static ru.viqa.ui_testing.common.utils.LinqUtils.*;
import static java.lang.String.format;

/**
 * Created by roman.i on 30.09.2014.
 */
public class WebDriverByUtils {

    public static FuncTT<String, By> getByFunc(By by) throws Exception {
        return first(getMapByTypes(), key -> by.toString().contains(key));
    }
    private static String getBadLocatorMsg(String byLocator, Object... args) throws Exception {
        return "Bad locator template '" + byLocator + "'. Args: " + print(select(args, Object::toString), ", ", "'%s'") + ".";
    }

    public static By fillByTemplate(By by, Object... args) throws Exception {
        String byLocator = getByLocator(by);
        try { byLocator = format(getByLocator(by), args); }
        catch(Exception ex) {
            throw VISite.Alerting.throwError(getBadLocatorMsg(byLocator, args)); }
        return getByFunc(by).invoke(byLocator);
    }

    public static String getByLocator(By by) {
        String byAsString = by.toString();
        int index = byAsString.indexOf(": ") + 2;
        return byAsString.substring(index);
    }

    private static Map<String, FuncTT<String, By>> getMapByTypes() {
        Map<String, FuncTT<String, By>> map = new HashMap<>();
        map.put("selector", By::cssSelector);
        map.put("className", By::className);
        map.put("id", By::id);
        map.put("linkText", By::linkText);
        map.put("name", By::name);
        map.put("partialLinkText", By::partialLinkText);
        map.put("tagName", By::tagName);
        map.put("xpath", By::xpath);
        return map;
    }
}
