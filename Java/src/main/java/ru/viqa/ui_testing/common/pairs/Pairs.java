package ru.viqa.ui_testing.common.pairs;

import ru.viqa.ui_testing.common.funcInterfaces.ActionT;
import ru.viqa.ui_testing.common.funcInterfaces.FuncTT;

import java.util.*;

import static java.lang.String.format;

/**
 * Created by 12345 on 30.09.2014.
 */
public class Pairs<TValue1, TValue2> extends ArrayList<Pair<TValue1, TValue2>> {
    public Pairs() { }

    public Pairs(List<Pair<TValue1, TValue2>> pairs)
    {
        if (pairs == null) return;
        for (Pair<TValue1, TValue2> element : pairs)
            this.add(element);
    }

    public Pairs(TValue1 value1, TValue2 value2, Collection<Pair<TValue1, TValue2>> pairs)
    {
        if (pairs != null)
            for (Pair<TValue1, TValue2> element : pairs)
                this.add(element);
        add(value1, value2);
    }

    public Pairs<TValue1, TValue2> add(TValue1 value1, TValue2 value2) { this.add(new Pair(value1, value2)); return this;}
    public Pairs<TValue1, TValue2> add(Pairs<TValue1, TValue2> pairs) throws Exception { pairs.foreach(this::add); return this; }

    public void AddNew(TValue1 value1, TValue2 value2) {
        clear();
        add(new Pair(value1, value2));
    }


    public void foreach(ActionT<Pair<TValue1, TValue2>> action) throws Exception {
        for (Pair<TValue1, TValue2> element : this)
            action.invoke(element);
    }

    public String print() { return Print("; ", "%s: %s"); }
    public String Print(String separator) { return Print(separator, "%s: %s"); }
    public String Print(String separator, String pairFormat)
    {
        List<String> StrList = new ArrayList<>();
        for (Pair<TValue1, TValue2> pair : this)
            StrList.add(format(pairFormat, pair.Value1, pair.Value2));
        return (this.size() > 0) ? String.join(separator, StrList) : "";
    }

    public static <T, TValue1, TValue2> Pairs<TValue1, TValue2> toPairs(Iterable<T> list, FuncTT<T, TValue1> selectorValue1, FuncTT<T, TValue2> selectorValue2) throws Exception {
        Pairs<TValue1, TValue2> Pairs = new ru.viqa.ui_testing.common.pairs.Pairs<>();
        for (T element : list)
            Pairs.add(selectorValue1.invoke(element), selectorValue2.invoke(element));
        return Pairs;
    }
}