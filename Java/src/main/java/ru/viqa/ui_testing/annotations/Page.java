package ru.viqa.ui_testing.annotations;

import ru.viqa.ui_testing.page_objects.PageCheckType;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/**
 * Created by roman.i on 06.10.2014.
 */
@Retention(RetentionPolicy.RUNTIME)
@Target({ElementType.TYPE, ElementType.FIELD})
public @interface Page {
    public String url() default "";
    public String title() default "";
    public boolean isHomePage() default false;

    public PageCheckType urlCheckType() default PageCheckType.NotSet;
    public PageCheckType titleCheckType() default PageCheckType.NotSet;
}
