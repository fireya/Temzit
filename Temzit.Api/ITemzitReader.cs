using System.Threading.Tasks;

namespace Temzit.Api
{
    public interface ITemzitReader
    {
        Task<TemzitActualState> ReadState();
    }
}