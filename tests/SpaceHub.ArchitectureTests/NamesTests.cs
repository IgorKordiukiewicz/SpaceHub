using MediatR;

namespace SpaceHub.ArchitectureTests;

public class NamesTests
{
    [Fact]
    public void Handlers_Should_HaveHandlerSuffix()
    {
        var result = Types.InCurrentDomain()
            .That().ResideInNamespace(Namespaces.ProjectNamespace(Namespaces.Application))
            .And().ImplementInterface(typeof(IRequestHandler<,>))
            .Should().HaveNameEndingWith("Handler")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Requests_Should_HaveCommandOrQuerySuffix()
    {
        var result = Types.InCurrentDomain()
            .That().ResideInNamespace(Namespaces.ProjectNamespace(Namespaces.Application))
            .And().ImplementInterface(typeof(IRequest<>)).Or().ImplementInterface(typeof(IRequest))
            .Should().HaveNameEndingWith("Query").Or().HaveNameEndingWith("Command")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Attributes_Should_HaveAttributeSuffix()
    {
        var result = Types.InCurrentDomain()
            .That()
            .ResideInNamespace(Namespaces.SolutionName)
            .And().Inherit(typeof(Attribute))
            .Should().HaveNameEndingWith("Attribute")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ViewModels_ShouldHaveVMSuffix()
    {
        var result = Types.InCurrentDomain()
            .That()
            .ResideInNamespace(Namespaces.ProjectNamespace(Namespaces.Contracts) + ".ViewModels")
            .Should()
            .HaveNameEndingWith("VM")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Errors_ShouldHaveErrorSuffix()
    {
        var result = Types.InCurrentDomain()
            .That()
            .ResideInNamespace(Namespaces.ProjectNamespace(Namespaces.Application) + ".Errors")
            .Should()
            .HaveNameEndingWith("Error")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
