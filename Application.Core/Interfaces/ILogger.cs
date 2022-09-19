using System.Threading.Tasks;

namespace Application.Core.Interfaces;

public interface ILogger
{
    public Task Info(string data);
    public Task Warn(string data);
    public Task Error(string data);
}