namespace VITestsProject.Data
{
    public class Product
    {
        public string Name;
        public int Cost;
        public bool Wifi;
        public bool SensorScreen;
        public string ProcessorType;
        public string Test { set; get; }

        public Product(string name) { Name = name; }
    }
}
