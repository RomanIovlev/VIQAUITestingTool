namespace VITestsProject.Data
{
    public class Range
    {
        public int From { get; set; }
        public int To;

        public Range(int from, int to)
        {
            From = from;
            To = to;
        }

        public override string ToString()
        {
            return string.Format("(From: {0}; To: {1})", From, To);
        }
    }
}
