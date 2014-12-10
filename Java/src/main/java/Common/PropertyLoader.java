package Common;

import java.io.IOException;
import java.util.Properties;

/**
 * Created by Алиса on 22.11.2014.
 */
public class PropertyLoader {
    private Properties props;

    public PropertyLoader(String propetyFile) throws IOException {
        props = new Properties();
        props.load(PropertyLoader.class.getResourceAsStream(PROP_FILE));
    }
    private final String PROP_FILE = "/properties";

    public String get(String name) throws IOException {
        return (name != null) ? props.getProperty(name) : "";
    }
}
