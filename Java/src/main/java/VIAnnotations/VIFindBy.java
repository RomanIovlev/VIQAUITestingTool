package VIAnnotations;

import org.openqa.selenium.support.FindBy;

import java.lang.annotation.*;

/**
 * Created by 12345 on 07.11.2014.
 */
@Retention(RetentionPolicy.RUNTIME)
@Target({ElementType.TYPE, ElementType.FIELD})
public @interface VIFindBy {
    public String using() default "";
    public String id() default "";
    public String name() default "";
    public String className() default "";
    public String css() default "";
    public String tagName() default "";
    public String linkText() default "";
    public String partialLinkText() default "";
    public String xpath() default "";

    public String group();
}
