namespace Temzit.MQTT
{
    public class Device
    {
        public string[] Identifiers { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string SwVersion { get; set; }
    }
}