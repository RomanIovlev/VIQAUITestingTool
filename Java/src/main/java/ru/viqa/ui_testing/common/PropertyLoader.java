package ru.viqa.ui_testing.common;

import ru.viqa.ui_testing.common.funcInterfaces.ActionT;

import java.io.IOException;
import java.util.Properties;

import static java.lang.String.format;
import static ru.viqa.ui_testing.page_objects.VISite.Alerting;

/**
 * Created by Алиса on 22.11.2014.
 */
public class PropertyLoader {
    private Properties props;
    private static Properties lastLoadedProps;

    public PropertyLoader(String propetyFile) throws IOException {
        try {
            props = new Properties();
            if (propetyFile.charAt(0) != '/')
                propetyFile = "/" + propetyFile;
            props.load(PropertyLoader.class.getResourceAsStream(propetyFile));
        } catch (IOException ex) { throw new IOException("Unable to load properties: " + ex.getMessage()); }
    }

    public String get(String name) throws IOException {
        return getProp(name, props);
    }

    private static String getProp(String name, Properties props) throws IOException {
        String result = "";
        if  (name != null && !name.equals(""))
            result = props.getProperty(name);
        return  result != null ? result : "";
    }

    public static String getProperty(String name) throws Exception {
        if (lastLoadedProps != null)
            return getProp(name, lastLoadedProps);
        else
            throw Alerting.throwError(format("Can't get property '%s'. Properties not loaded", name));
    }

    public void fillAction(ActionT<String> action, String propName) throws Exception {
        String prop = get(propName);
        if (prop != null && !prop.equals(""))
            action.invoke(prop);
    }
}
