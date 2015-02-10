package ru.viqa.ui_testing.common.utils;

import ru.viqa.ui_testing.common.funcInterfaces.FuncTT;
import ru.viqa.ui_testing.page_objects.VISite;
import org.openqa.selenium.By;

import java.util.*;

import static ru.viqa.ui_testing.common.utils.PrintUtils.*;
import static ru.viqa.ui_testing.common.utils.LinqUtils.*;
import static java.lang.String.format;
import static ru.viqa.ui_testing.page_objects.VISite.Alerting;

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
            throw Alerting.throwError(getBadLocatorMsg(byLocator, args)); }
        return getByFunc(by).invoke(byLocator);
    }
    public static By copyBy(By by) throws Exception {
        String byLocator = getByLocator(by);
        return getByFunc(by).invoke(byLocator);
    }


    public static String getByLocator(By by) {
        String byAsString = by.toString();
        int index = byAsString.indexOf(": ") + 2;
        return byAsString.substring(index);
    }

    private static Map<String, FuncTT<String, By>> getMapByTypes() {
        Map<String, FuncTT<String, By>> map = new HashMap<>();
        map.put("By.selector:", By::cssSelector);
        map.put("By.className", By::className);
        map.put("By.id", By::id);
        map.put("By.linkText", By::linkText);
        map.put("By.name", By::name);
        map.put("By.partialLinkText", By::partialLinkText);
        map.put("By.tagName", By::tagName);
        map.put("By.xpath", By::xpath);
        return map;
    }
}
