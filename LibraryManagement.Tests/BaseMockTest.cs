using Moq;

namespace LibraryManagement.Tests;

public abstract class BaseMockTest : IDisposable
{
    private readonly MockRepository mocks = new(MockBehavior.Strict);

    protected Mock<T> Strict<T>() where T : class => mocks.Create<T>();
    protected Mock<T> Stub<T>() where T : class => mocks.Create<T>(MockBehavior.Loose);

    public void Dispose() => mocks.VerifyAll();
}