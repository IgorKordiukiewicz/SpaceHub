using MediatR;

namespace SpaceHub.ArchitectureTests;

public class ModifiersTests
{
    [Fact]
    public void Handlers_ShouldBeSealed()
    {
        var result = Types.InCurrentDomain()
            .That().ResideInNamespace(Namespaces.ProjectNamespace(Namespaces.Application))
            .And().ImplementInterface(typeof(IRequestHandler<,>))
            .Should().BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
