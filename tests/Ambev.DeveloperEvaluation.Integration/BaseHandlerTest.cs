using Ambev.DeveloperEvaluation.Integration.Fixtures;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration
{
    public abstract class BaseHandlerTest : IClassFixture<ServiceLocatorFixture>
    {
        protected readonly ServiceLocatorFixture _fixture;

        public BaseHandlerTest(ServiceLocatorFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
