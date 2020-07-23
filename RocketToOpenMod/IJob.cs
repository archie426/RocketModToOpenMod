using System.Threading.Tasks;

namespace RocketToOpenMod
{
    public interface IJob
    {
        Task Do();
    }
}