using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Temzit.Api;

namespace Temzit.MQTT
{
    public class MqttSender : IMqttSender
    {
        private readonly IMqttClient _mqttClient;
        private readonly IMqttClientOptions _options;

        public MqttSender()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                .WithTcpServer("192.168.88.250", 1883)
                .Build();
        }

        public async Task Init(TemzitActualState state)
        {
            await _mqttClient.ConnectAsync(_options, CancellationToken.None);

            await SendSensorConfig(nameof(TemzitActualState.InsideTemperature), "Внутренняя температура", "temperature",
                "°C");
            await SendSensorConfig(nameof(TemzitActualState.OutsideTemperature), "Уличная температура",
                "temperature", "°C");
            
            await SendSensorConfig(nameof(TemzitActualState.InputPower), "Потребление", "power", "kW");
            await SendSensorConfig(nameof(TemzitActualState.OutputPower), "Выходная мощность", "power", "kW");
            await SendSensorConfig(nameof(TemzitActualState.InHeatTemperature), "Обратка", "temperature", "°C");
            await SendSensorConfig(nameof(TemzitActualState.OutHeatTemperature), "Подача", "temperature", "°C");
            await SendSensorConfig(nameof(TemzitActualState.COP), "СОР", "power_factor", "");
        }

        public async Task SendData(TemzitActualState state)
        {
            await _mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic("zigbee2mqtt/temzit")
                .WithPayload(JsonSerializer.Serialize(state))
                .Build(), CancellationToken.None);
        }

        private async Task SendSensorConfig(string objectId, string name, string deviceClass, string unit)
        {
            var device = new Device
            {
                Identifiers = new[] {"kotel_1"},
                Manufacturer = "Temzit",
                Model = "ТН Темзит",
                Name = "Тепловой насос Темзит",
                SwVersion = "0.1"
            };
            var sensor = new TemzitSensor
            {
                Device = device,
                DeviceClass = deviceClass,
                Name = name,
                StateTopic = "zigbee2mqtt/temzit",
                JsonAttributesTopic = "zigbee2mqtt/temzit",
                UnitOfMeasurement = unit,
                UniqueId = $"temzit_{objectId}",
                ValueTemplate = $"{{{{ value_json.{objectId} }}}}"
            };

            var jsonString = JsonSerializer.Serialize(sensor,
                new JsonSerializerOptions {PropertyNamingPolicy = new SnakeCaseNamingPolicy()});

            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"homeassistant/sensor/temzit/{objectId}/config")
                .WithPayload(jsonString)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await _mqttClient.PublishAsync(message, CancellationToken.None);
        }
    }
}