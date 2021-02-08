using System;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Data
{
    public abstract class EfReadOnlySession : IAsyncDisposable
    {
        protected EfReadOnlySession(DatabaseContext context) => Context = context;

        protected DatabaseContext Context { get; }

        public ValueTask DisposeAsync() => Context.DisposeAsync();
    }
}