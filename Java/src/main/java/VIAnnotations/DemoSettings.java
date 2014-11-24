package VIAnnotations;

import SiteClasses.HighlightSettings;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/**
 * Created by roman.i on 06.10.2014.
 */
@Retention(RetentionPolicy.RUNTIME)
@Target({ElementType.TYPE, ElementType.FIELD})
public @interface DemoSettings {
    public String bgColor() default "yellow";
    public String frameColor() default "red";
    public int timeoutInSec() default 1;
}
