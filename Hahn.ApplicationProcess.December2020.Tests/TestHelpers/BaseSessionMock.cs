using System.Threading.Tasks;
using FluentAssertions;
using Hahn.ApplicationProcess.December2020.Data;

namespace Hahn.ApplicationProcess.December2020.Tests.TestHelpers
{
    public abstract class BaseSessionMock<T> : BaseReadOnlySessionMock<T>, ISession
        where T : BaseSessionMock<T>
    {
        public int SaveChangesAsyncCallCount { get; private set; }

        public Task SaveChangesAsync()
        {
            SaveChangesAsyncCallCount++;
            return Task.CompletedTask;
        }

        public T SaveChangesMustHaveBeenCalled()
        {
            SaveChangesAsyncCallCount.Should().Be(1);
            return (T) this;
        }
    }
}