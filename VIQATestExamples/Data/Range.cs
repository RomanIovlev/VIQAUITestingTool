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
    }
}
