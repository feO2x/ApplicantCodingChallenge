using System;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Data
{
    public interface ISession : IAsyncDisposable
    {
        Task SaveChangesAsync();
    }
}