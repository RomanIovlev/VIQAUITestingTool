package ru.viqa.ui_testing.common.utils;

import ru.viqa.ui_testing.common.funcInterfaces.*;

import java.util.*;

/**
 * Created by roman.i on 30.09.2014.
 */
public class LinqUtils {
    public static <T, T1> Collection<T1> select(Iterable<T> list, FuncTT<T, T1> func) throws Exception {
        List<T1> result = new ArrayList<>();
        for(T el : list)
            result.add(func.invoke(el));
        return result;
    }
    public static <T, T1> Collection<T1> select(T[] list, FuncTT<T, T1> func) throws Exception {
        return select(Arrays.asList(list), func);
    }
    public static <T, T1, T2> Collection<T2> selectMap(Map<T, T1> map, FuncTT<Map.Entry<T, T1>, T2> func) throws Exception {
        List<T2> result = new ArrayList<>();
        for(Map.Entry<T, T1> el : map.entrySet())
            result.add(func.invoke(el));
        return result;
    }
    public static <T, T1, T2> Map<T, T2> select(Map<T, T1> map, FuncTT<T1, T2> func) throws Exception {
        Map<T, T2> result = new HashMap<>();
        for(Map.Entry<T, T1> el : map.entrySet())
            result.put(el.getKey(), func.invoke(el.getValue()));
        return result;
    }

    public static <T> Collection<T> where(Iterable<T> list, FuncTT<T, Boolean> func) throws Exception {
        List<T> result = new ArrayList<>();
        for(T el : list)
            if (func.invoke(el))
            result.add(el);
        return result;
    }
    public static <T> Collection<T> where(T[] list, FuncTT<T, Boolean> func) throws Exception {
        return where(Arrays.asList(list), func);
    }
    public static <T, T1> Map<T, T1> where(Map<T, T1> map, FuncTT<Map.Entry<T, T1>, Boolean> func) throws Exception {
        Map<T, T1> result = new HashMap<>();
        for(Map.Entry<T, T1> el : map.entrySet())
            if (func.invoke(el))
                result.put(el.getKey(), el.getValue());
        return result;
    }

    public static <T> void foreach(Iterable<T> list, ActionT<T> action) throws Exception {
        for(T el : list)
            action.invoke(el);
    }
    public static <T> void foreach(T[] list, ActionT<T> action) throws Exception {
        foreach(Arrays.asList(list), action);
    }
    public static <T, T1> void foreach(Map<T, T1> map, ActionT<Map.Entry<T, T1>> action) throws Exception {
        for(Map.Entry<T, T1> entry : map.entrySet())
            action.invoke(entry);
    }
    public static <T> T first(Iterable<T> list)
    {
        for(T el : list)
            return el;
        return null;
    }
    public static <T> T first(T[] list) {
        return first(Arrays.asList(list));
    }
    public static <T, T1> T1 first(Map<T, T1> map) {
        for (Map.Entry<T, T1> el : map.entrySet())
            return el.getValue();
        return null;
    }
    public static <T> T first(Iterable<T> list, FuncTT<T, Boolean> func) throws Exception {
        for(T el : list)
            if (func.invoke(el))
                return el;
        return null;
    }
    public static <T> T first(T[] list, FuncTT<T, Boolean> func) throws Exception {
        return first(Arrays.asList(list), func);
    }
    public static <T, T1> T1 first(Map<T, T1> map, FuncTT<T, Boolean> func) throws Exception {
        for (Map.Entry<T, T1> el : map.entrySet())
            if (func.invoke(el.getKey()))
                return el.getValue();
        return null;
    }

    public static <T> T last(Iterable<T> list)
    {
        T result = null;
        for(T el : list)
            result = el;
        return result;
    }
    public static <T> T last(T[] list) {
        return last(Arrays.asList(list));
    }
    public static <T> T last(Iterable<T> list, FuncTT<T, Boolean> func) throws Exception {
        T result = null;
        for(T el : list)
            if (func.invoke(el))
                result = el;
        return result;
    }
    public static <T> T last(T[] list, FuncTT<T, Boolean> func) throws Exception {
        return last(Arrays.asList(list), func);
    }

}
