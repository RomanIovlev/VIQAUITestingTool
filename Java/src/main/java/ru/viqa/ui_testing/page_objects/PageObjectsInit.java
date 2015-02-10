package ru.viqa.ui_testing.page_objects;

import ru.viqa.ui_testing.annotations.*;
import ru.viqa.ui_testing.elements.baseClasses.*;
import ru.viqa.ui_testing.elements.complexElements.*;
import ru.viqa.ui_testing.elements.interfaces.*;
import ru.viqa.ui_testing.elements.simpleElements.*;
import org.openqa.selenium.By;
import org.openqa.selenium.support.FindBy;

import java.lang.reflect.Field;
import java.util.HashMap;
import java.util.Map;

import static java.lang.String.format;
import static ru.viqa.ui_testing.common.utils.LinqUtils.*;
import static ru.viqa.ui_testing.common.utils.ReflectionUtils.*;
import static ru.viqa.ui_testing.common.utils.StringUtils.LineBreak;
import static ru.viqa.ui_testing.annotations.AnnotationsUtil.*;

/**
 * Created by roman.i on 10.12.2014.
 */
public class PageObjectsInit {
    public static void initPagesCascade(VISite site) throws Exception {
        try {
            initSubElements(site);
            foreach(getFields(site, VIPage.class), page -> {
                VIPage instance = (VIPage) page.getType().newInstance();
                instance.setSite(site);
                Page pageAttr = page.getAnnotation(Page.class);
                if (pageAttr == null)
                    instance.setEmptyPage();
                else
                    instance.fillFromPageAttribute(pageAttr);
                pageAttr = page.getType().getAnnotation(Page.class);
                if (pageAttr != null)
                    instance.updatePageAttribute(pageAttr);
                initSubElements(instance);
                page.set(site, instance);
            });
        } catch (Exception ex) {
            throw new Exception("Error in Pages Cascade Initialization:" + LineBreak + ex.getMessage()); }
    }

    public static void initSubElements(VIElement root) throws Exception {
        foreach(getFields(root, IVIElement.class), val -> setVIElement(val, root));
    }

    private static void setVIElement(Field viElement, VIElement root) throws Exception {
        try {
            if (isClass(viElement, VISite.class) || viElement.getAnnotation(NotPageObject.class) != null) return;
            Class<?> type = viElement.getType();
            VIElement instance = createInstance(viElement, root);
            instance.setName(getElementName(viElement));
            createContext(viElement, instance, root);
            setHaveValueData(type, instance);
            setClickableElementData(type, instance);
            viElement.set(root, instance);
            initSubElements(instance);
        } catch (Exception ex) {
            throw new Exception(format("Error in setVIElement for field '%s'", viElement.getName()) + LineBreak + ex.getMessage()); }
    }

    private static VIElement createInstance(Field viElement, VIElement root) throws Exception {
        try {
            VIElement instance = (VIElement) getFieldValue(viElement, root);
            if (instance == null)
                instance = getVIElementInstance(viElement.getType());
            instance.setSite(root.getSite());
            return instance;
        } catch (Exception ex) {
            throw new Exception(format("Error in createInstance for field '%s'", viElement.getName()) + LineBreak + ex.getMessage()); }
    }

    private static VIElement getVIElementInstance(Class type) throws Exception {
        try {
            if (!type.isInterface())
                return (VIElement) type.newInstance();
            Class viType = first(map, el -> el == type);
            if (viType != null)
                return (VIElement) viType.newInstance();
            throw  VISite.Alerting.throwError("Unknown interface: " + type +
                    "Add relation interface -> class in VIElement.InterfaceTypeMap");
        } catch (Exception ex) {
            throw new Exception(format("Error in getVIElementInstance for type '%s'", type.getName()) +
                    LineBreak + ex.getMessage()); }
    }


    private static void createContext(Field viElement, VIElement instance, VIElement root) throws Exception {
        try {
            instance.Context.add(root.Context);
            By frameBy = getFrame(viElement.getType().getDeclaredAnnotation(Frame.class));
            if (frameBy != null)
                instance.Context.add(ContextType.Frame, frameBy);
            By locatorBy = null;
            String locatorGroup = root.getSite().getLocatorVersion();
            if (locatorGroup != null) {
                VIFindBy viFindBy = viElement.getAnnotation(VIFindBy.class);
                if (viFindBy != null && locatorGroup.equals(viFindBy.group()))
                    locatorBy = getLocator(viFindBy);
            }
            if (locatorBy == null)
                locatorBy = getLocator(viElement.getAnnotation(FindBy.class));
            if (locatorBy != null)
                instance.setLocator(locatorBy);
            if (root.getPrivateLocator() != null)
                instance.Context.add(ContextType.Locator, root.getPrivateLocator());
        } catch (Exception ex) {
            throw new Exception(format("Error in createContext for type '%s'", viElement.getName()) +
                    LineBreak + ex.getMessage()); }
    }

    private static void setHaveValueData(Class viElement, VIElement instance) {}

    private static void setClickableElementData(Class<?> viElement, VIElement instance) throws Exception {
        try {
            if (!isClass(viElement, Clickable.class))
                return;
            ClickLoadsPage openPageAnnotation = viElement.getDeclaredAnnotation(ClickLoadsPage.class);
            if (openPageAnnotation != null)
                ((Clickable) instance).clickOpensPage = openPageAnnotation.pageName();
        } catch (Exception ex) {
            throw new Exception(format("Error in setClickableElementData for type '%s'", viElement.getName()) +
                    LineBreak + ex.getMessage()); }
    }
    public static Map<Class, Class> map;

    public static void interfaceTypeMapInit() throws Exception {
        try {
            map = new HashMap<>();
            map.put(IVIElement.class,   VIElement.class);
            map.put(IButton.class,      Button.class);
            map.put(IClickable.class,   Clickable.class);
            map.put(ILink.class,        Link.class);
            map.put(ISelector.class,    Selector.class);
            map.put(IText.class,        Text.class);
            map.put(ITextArea.class,    TextArea.class);
            map.put(ITextField.class,   TextField.class);
            map.put(IDropdown.class,    Dropdown.class);
            map.put(ITable.class,       Table.class);
            map.put(ICheckbox.class,    Checkbox.class);
            map.put(IRadioButtons.class, RadioButtons.class);
            map.put(ICheckList.class,   Checklist.class);
            map.put(ITextList.class,   TextList.class);
        } catch (Exception ex) {
            throw new Exception("Error in getInterfaceTypeMap" +
                    LineBreak + ex.getMessage()); }
    }
}
