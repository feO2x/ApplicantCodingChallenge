using System;
using System.Threading.Tasks;
using FluentAssertions;

namespace Hahn.ApplicationProcess.December2020.Tests.TestHelpers
{
    public abstract class BaseReadOnlySessionMock<T> : IAsyncDisposable
        where T : BaseReadOnlySessionMock<T>
    {
        public int DisposeAsyncCallCount { get; private set; }

        public ValueTask DisposeAsync()
        {
            DisposeAsyncCallCount++;
            return ValueTask.CompletedTask;
        }

        public T MustHaveBeenDisposed()
        {
            DisposeAsyncCallCount.Should().BeGreaterOrEqualTo(1);
            return (T) this;
        }
    }
}