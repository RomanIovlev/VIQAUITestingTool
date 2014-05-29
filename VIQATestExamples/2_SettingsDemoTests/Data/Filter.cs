using VIQA.Common;

namespace Settings.VITestsProject.Data
{
    public class Filter
    {
        public string ShortSearchName;
        public Range CostRange;
        public bool? Wifi;
        public SensorScreenTypes? SensorScreen;
        public string[] ProcessorTypes;
        public string[] PlatformTypes;

        public override string ToString()
        {
            return
                string.Format(
                    "ShortSearchName: {0}; CostRange: {1}; Wifi: {2}; SensorScreen: {3}; ProcessorTypes: {4}; PlatformTypes: {5}",
                    ShortSearchName,
                    CostRange,
                    Wifi != null ? Wifi.ToString() : "null",
                    SensorScreen != null ? SensorScreen.ToString() : "null",
                    ProcessorTypes.Print(),
                    PlatformTypes.Print());
        }
    }

    public enum SensorScreenTypes { да, нет, неважно }
}
