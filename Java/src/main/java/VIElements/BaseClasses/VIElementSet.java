package VIElements.BaseClasses;

import Common.*;
import Common.Pairs.Pairs;
import SiteClasses.VISite;
import VIAnnotations.*;
import VIElements.Interfaces.*;
import org.openqa.selenium.By;
import org.openqa.selenium.support.FindBy;

import java.lang.reflect.*;
import java.util.List;

import static Common.Utils.ReflectionUtils.*;
import static Common.Utils.ReflectionUtils.getFields;
import static Common.Utils.LinqUtils.*;
import static VIAnnotations.AnnotationsUtil.*;
import static VIElements.BaseClasses.VIElement.getInterfaceTypeMap;

/**
 * Created by 12345 on 28.09.2014.
 */
public class VIElementSet extends Named {
    private VISite _site;
    public VISite getSite() { return _site;}
    protected boolean isSiteSet() { return _site != null; }
    public void setSite(VISite site) { _site = site;}
    public Pairs<ContextType, By> Context = new Pairs<>();
    protected By _locator;
    public static VISite DefaultSite;
    public boolean haveLocator() { return _locator != null; }

    private void setVIElement(Field viElement) throws Exception {
        Class<?> type = viElement.getType();
        VIElement instance = createInstance(viElement);
        instance.setName(getElementName(viElement));
        createContext(viElement, instance);
        setHaveValueData(type, instance);
        setClickableElementData(type, instance);
        viElement.set(this, instance);
        instance.initSubElements();
    }

    private VIElement createInstance(Field viElement) throws Exception {
        VIElement instance = (VIElement) getField(viElement, this);
        if (instance == null)
            instance = getVIElementInstance(viElement.getType());
        instance.setSite(getSite());
        return instance;
    }

    private VIElement getVIElementInstance(Class type) throws Exception {
        if (!type.isInterface())
            return (VIElement) type.newInstance();
        Class viType = first(getInterfaceTypeMap(), el -> el == type);
        if (viType != null)
            return (VIElement) viType.newInstance();
        throw  VISite.Alerting.throwError("Unknown interface: " + type + "Add relation interface -> class in VIElement.InterfaceTypeMap");
    }


    private void createContext(Field viElement, VIElement instance) throws Exception {
        instance.Context.add(Context);
        By frameBy = getFrame(viElement.getType().getDeclaredAnnotation(Frame.class));
        if (frameBy != null)
            instance.Context.add(ContextType.Frame, frameBy);
        By locatorBy = null;
        String locatorGroup = getSite().locatorGroup;
        if (locatorGroup != null) {
            VIFindBy viFindBy = viElement.getAnnotation(VIFindBy.class);
            if (viFindBy != null && locatorGroup.equals(viFindBy.group()))
                locatorBy = getLocator(viFindBy);
        }
        if (locatorBy == null)
            locatorBy = getLocator(viElement.getAnnotation(FindBy.class));
        if (locatorBy != null)
            instance.setLocator(locatorBy);
        if (_locator != null)
            instance.Context.add(ContextType.Locator, _locator);
    }

    private void setHaveValueData(Class viElement, VIElement instance) {}

    private void setClickableElementData(Class viElement, VIElement instance) {}

    public void initSubElements() throws Exception {
        foreach(getFields(this, IVIElement.class), this::setVIElement);
    }

    public void fillFrom(Object obj) throws Exception {
        if (obj == null) return;
        List<Field> fields = getFields(obj, String.class);
        foreach(getFields(this, IHaveValue.class), element -> {
            Field fieldWithName = first(fields, field ->
                AnnotationsUtil.getElementName(field)
                    .equals(AnnotationsUtil.getElementName(element)));
            if (fieldWithName != null) {
                String value = (String) getField(fieldWithName, obj);
                if (value != null)
                    ((IHaveValue) getField(element, this)).setValue(value);
            }
        });
    }
}
