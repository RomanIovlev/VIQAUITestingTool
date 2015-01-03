package ru.viqa.ui_testing.page_objects;

import ru.viqa.ui_testing.annotations.Frame;
import ru.viqa.ui_testing.annotations.Page;
import ru.viqa.ui_testing.annotations.VIFindBy;
import ru.viqa.ui_testing.elements.baseClasses.ContextType;
import ru.viqa.ui_testing.elements.baseClasses.Selector;
import ru.viqa.ui_testing.elements.baseClasses.VIElement;
import ru.viqa.ui_testing.elements.complexElements.Checklist;
import ru.viqa.ui_testing.elements.complexElements.Dropdown;
import ru.viqa.ui_testing.elements.complexElements.RadioButtons;
import ru.viqa.ui_testing.elements.interfaces.*;
import ru.viqa.ui_testing.elements.simpleElements.*;
import org.openqa.selenium.By;
import org.openqa.selenium.support.FindBy;

import java.lang.reflect.Field;
import java.util.HashMap;
import java.util.Map;

import static ru.viqa.ui_testing.common.utils.LinqUtils.first;
import static ru.viqa.ui_testing.common.utils.LinqUtils.foreach;
import static ru.viqa.ui_testing.common.utils.ReflectionUtils.getField;
import static ru.viqa.ui_testing.common.utils.ReflectionUtils.getFields;
import static ru.viqa.ui_testing.common.utils.ReflectionUtils.isClass;
import static ru.viqa.ui_testing.common.utils.StringUtils.LineBreak;
import static ru.viqa.ui_testing.annotations.AnnotationsUtil.getElementName;
import static ru.viqa.ui_testing.annotations.AnnotationsUtil.getFrame;
import static ru.viqa.ui_testing.annotations.AnnotationsUtil.getLocator;

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
        foreach(getFields(root, IVIElement.class), (val) -> setVIElement(val, root));
    }

    private static void setVIElement(Field viElement, VIElement root) throws Exception {
        if (isClass(viElement, VISite.class)) return;
        Class<?> type = viElement.getType();
        VIElement instance = createInstance(viElement, root);
        instance.setName(getElementName(viElement));
        createContext(viElement, instance, root);
        setHaveValueData(type, instance);
        setClickableElementData(type, instance);
        viElement.set(root, instance);
        initSubElements(instance);
    }

    private static VIElement createInstance(Field viElement, VIElement root) throws Exception {
        VIElement instance = (VIElement) getField(viElement, root);
        if (instance == null)
            instance = getVIElementInstance(viElement.getType());
        instance.setSite(root.getSite());
        return instance;
    }

    private static VIElement getVIElementInstance(Class type) throws Exception {
        if (!type.isInterface())
            return (VIElement) type.newInstance();
        Class viType = first(getInterfaceTypeMap(), el -> el == type);
        if (viType != null)
            return (VIElement) viType.newInstance();
        throw  VISite.Alerting.throwError("Unknown interface: " + type + "Add relation interface -> class in VIElement.InterfaceTypeMap");
    }


    private static void createContext(Field viElement, VIElement instance, VIElement root) throws Exception {
        instance.Context.add(root.Context);
        By frameBy = getFrame(viElement.getType().getDeclaredAnnotation(Frame.class));
        if (frameBy != null)
            instance.Context.add(ContextType.Frame, frameBy);
        By locatorBy = null;
        String locatorGroup = root.getSite().locatorGroup;
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
    }

    private static void setHaveValueData(Class viElement, VIElement instance) {}

    private static void setClickableElementData(Class viElement, VIElement instance) {}

    public static Map<Class, Class> getInterfaceTypeMap() {
        Map<Class, Class> map = new HashMap<>();
        map.put(IVIElement.class, VIElement.class);
        map.put(IButton.class, Button.class);
        map.put(IClickable.class, Clickable.class);
        map.put(ILink.class, Link.class);
        map.put(ISelector.class, Selector.class);
        map.put(IText.class, Text.class);
        map.put(ITextArea.class, TextArea.class);
        map.put(ITextField.class, TextField.class);
        map.put(IDropDown.class, Dropdown.class);
        map.put(ITable.class, Table.class);
        map.put(ICheckbox.class, Checkbox.class);
        map.put(IRadioButtons.class, RadioButtons.class);
        map.put(ICheckList.class, Checklist.class);
        return map;
    }
}
