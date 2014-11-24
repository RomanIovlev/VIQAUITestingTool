package Common.Pairs;

/**
 * Created by 12345 on 30.09.2014.
 */
public class Pair<TValue1, TValue2> {
    public TValue1 Value1;
    public TValue2 Value2;

    public Pair(TValue1 value1, TValue2 value2)
    {
        Value1 = value1;
        Value2 = value2;
    }

    public boolean Equals(Pair<TValue1, TValue2> other)
    {
        if (other == null) return false;
        if (this == other) return true;
        return Value1 == other.Value1 && Value2 == other.Value2;
    }

    @Override
    public int hashCode() { return Value1.hashCode() ^ Value2.hashCode(); }

    @Override
    public boolean equals(Object o) {
        if (o == null) return false;
        if (!(o instanceof PairString)) return false;
        Pair<TValue1, TValue2> pairo = (Pair<TValue1, TValue2>) o;
        return this.Value1.equals(pairo.Value1) &&
                this.Value2.equals(pairo.Value2);
    }
}
