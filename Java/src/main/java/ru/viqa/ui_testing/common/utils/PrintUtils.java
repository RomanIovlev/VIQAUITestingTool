package ru.viqa.ui_testing.common.utils;

import java.util.Arrays;

import static ru.viqa.ui_testing.common.utils.ReflectionUtils.getField;
import static ru.viqa.ui_testing.common.utils.ReflectionUtils.getFields;
import static ru.viqa.ui_testing.common.utils.LinqUtils.*;
import static java.lang.String.format;

/**
 * Created by roman.i on 30.09.2014.
 */
public class PrintUtils {
    public static String print(Iterable<String> list) throws Exception { return print(list, ", ", "%s"); }
    public static String print(Iterable<String> list, String separator) throws Exception { return print(list, separator, "%s"); }
    public static String print(Iterable<String> list, String separator, String format) throws Exception {
        return (list != null) ? String.join(separator, select(list, el -> format(format, el))) : "";
    }
    public static String print(String[] list) throws Exception { return print(list, ", ", "%s"); }
    public static String print(String[] list, String separator) throws Exception { return print(list, separator, "%s"); }
    public static String print(String[] list, String separator, String format) throws Exception {
        return print(Arrays.asList(list), separator, format);
    }
    public static String printFields(Object obj) throws Exception { return printFields(obj, "; "); }
    public static String printFields(Object obj, String separator) throws Exception {
        String className = obj.getClass().getSimpleName();
        String params = print(select(getFields(obj, String.class),
            field -> field.getName() + ": '" + getField(field, obj) + "'"), separator, "%s");
        return format("%s(%s)", className, params);
    }
}
