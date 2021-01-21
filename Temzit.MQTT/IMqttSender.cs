using System.Threading.Tasks;
using Temzit.Api;

namespace Temzit.MQTT
{
    public interface IMqttSender
    {
        Task Init(TemzitActualState state);
        Task SendData(TemzitActualState state);
    }
}