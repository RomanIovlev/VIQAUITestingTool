package ru.viqa.ui_testing.annotations;

import ru.viqa.ui_testing.common.alertings.ScreenshotAlert;
import ru.viqa.ui_testing.page_objects.VISite;
import org.openqa.selenium.By;
import org.openqa.selenium.support.FindBy;

import java.lang.reflect.Field;

/**
 * Created by roman.i on 25.09.2014.
 */
public class AnnotationsUtil {
    public static <T> String getElementName(T clazz) {
        Class<T> cl = (Class<T>)clazz.getClass();
        if (cl.isAnnotationPresent(Name.class)) {
            return cl.getAnnotation(Name.class).value();
        } else {
            return splitCamelCase(cl.getSimpleName());
        }
    }

    public static String getElementName(Field field) {
        Class<?> cl = field.getClass();
        if (cl.isAnnotationPresent(Name.class)) {
            return cl.getAnnotation(Name.class).value();
        } else {
            return splitCamelCase(field.getName());
        }
    }
    private static String splitCamelCase(String camel) {
        String result = (camel.charAt(0) + "").toUpperCase();
        for (int i = 1; i < camel.length() - 1; i++)
            result += (((isCapital(camel.charAt(i)) && !isCapital(camel.charAt(i + 1))) ? " " : "") + camel.charAt(i));
        return result + camel.charAt(camel.length() - 1);
    }


    public static ScreenshotAlert getScreenshotAlert(VISite site)
    {
        ScreenshotAlert alert = new ScreenshotAlert(site);
        alert.setFileName(VISite.testContext.getTestName() + "_fail_" + VISite.RunId);
        return alert;
    }

    private static boolean isCapital(char ch) { return ('A' < ch  && ch < 'Z') || ('А' < ch  && ch < 'Я'); }

    public static By getFrame(Frame frame) throws Exception {
        if (frame == null) return null;
        if (!"".equals(frame.id()))
            return By.id(frame.id());
        if (!"".equals(frame.className()))
            return By.className(frame.className());
        if (!"".equals(frame.xpath()))
            return By.xpath(frame.xpath());
        if (!"".equals(frame.css()))
            return By.cssSelector(frame.css());
        if (!"".equals(frame.linkText()))
            return By.linkText(frame.linkText());
        if (!"".equals(frame.name()))
            return By.name(frame.name());
        if (!"".equals(frame.partialLinkText()))
            return By.partialLinkText(frame.partialLinkText());
        if (!"".equals(frame.tagName()))
            return By.tagName(frame.tagName());
        return null;
    }
    public static By getLocator(FindBy locator) throws Exception {
        if (locator == null) return null;
        if (!"".equals(locator.id()))
            return By.id(locator.id());
        if (!"".equals(locator.className()))
            return By.className(locator.className());
        if (!"".equals(locator.xpath()))
            return By.xpath(locator.xpath());
        if (!"".equals(locator.css()))
            return By.cssSelector(locator.css());
        if (!"".equals(locator.linkText()))
            return By.linkText(locator.linkText());
        if (!"".equals(locator.name()))
            return By.name(locator.name());
        if (!"".equals(locator.partialLinkText()))
            return By.partialLinkText(locator.partialLinkText());
        if (!"".equals(locator.tagName()))
            return By.tagName(locator.tagName());
        return null;
    }
    public static By getLocator(VIFindBy locator) throws Exception {
        if (locator == null) return null;
        if (!"".equals(locator.id()))
            return By.id(locator.id());
        if (!"".equals(locator.className()))
            return By.className(locator.className());
        if (!"".equals(locator.xpath()))
            return By.xpath(locator.xpath());
        if (!"".equals(locator.css()))
            return By.cssSelector(locator.css());
        if (!"".equals(locator.linkText()))
            return By.linkText(locator.linkText());
        if (!"".equals(locator.name()))
            return By.name(locator.name());
        if (!"".equals(locator.partialLinkText()))
            return By.partialLinkText(locator.partialLinkText());
        if (!"".equals(locator.tagName()))
            return By.tagName(locator.tagName());
        return null;
    }
}
