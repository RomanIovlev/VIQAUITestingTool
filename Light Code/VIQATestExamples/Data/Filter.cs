namespace VITestsProject.Data
{
    public class Filter
    {
        public string ShortSearchName;
        public Range CostRange;
        public bool? Wifi;
        public SensorScreenTypes? SensorScreen;
        public string[] ProcessorTypes;
    }

    public enum SensorScreenTypes { да, нет, неважно }
}
