package VIAnnotations;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/**
 * Created by roman.i on 06.10.2014.
 */
@Retention(RetentionPolicy.RUNTIME)
@Target({ElementType.TYPE, ElementType.FIELD})
public @interface Site {
    public String domain();
    public boolean useCache() default true;
    public boolean demoMode() default false;
    public boolean screenshotAlert() default false;
    public String settingsFromPropertyFile() default "";
}
