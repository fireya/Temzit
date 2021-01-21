namespace Temzit.MQTT
{
    public class TemzitSensor
    {
        public Device Device { get; set; }
        public string DeviceClass { get; set; }
        public string JsonAttributesTopic { get; set; }
        public string Name { get; set; }
        public string StateTopic { get; set; }
        public string UniqueId { get; set; }
        public string UnitOfMeasurement { get; set; }
        public string ValueTemplate { get; set; }
    }
}