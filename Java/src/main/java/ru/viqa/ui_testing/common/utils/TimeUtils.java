package ru.viqa.ui_testing.common.utils;

import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created by roman.i on 19.11.2014.
 */
public class TimeUtils {
    public static String nowTime(String format) { return new SimpleDateFormat(format).format(new Date()); }
    public static String nowTime() { return nowTime("yyMMddHHmmss"); }
}
