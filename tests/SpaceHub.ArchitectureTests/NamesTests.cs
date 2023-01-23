namespace SpaceHub.ArchitectureTests;

public class NamesTests
{
    [Fact]
    public void Handlers_Should_HaveHandlerSuffix()
    {
        var handlersNames = Types.InCurrentDomain()
            .That()
            .ResideInNamespace(Namespaces.ProjectNamespace(Namespaces.Application))
            .GetTypes()
            .Where(x => x.GetInterfaces().Any(xx => xx.Name.Contains("IRequestHandler")))
            .Select(x => x.Name)
            .ToList();

        handlersNames.All(x => x.EndsWith("Handler")).Should().BeTrue();
    }

    [Fact]
    public void Requests_Should_HaveCommandOrQuerySuffix()
    {
        var requestsNames = Types.InCurrentDomain()
            .That()
            .ResideInNamespace(Namespaces.ProjectNamespace(Namespaces.Application))
            .GetTypes()
            .Where(x => x.GetInterfaces().Any(xx => xx.Name.Contains("IRequest") && !xx.Name.Contains("IRequestHandler")))
            .Select(x => x.Name)
            .ToList();

        requestsNames.All(x => x.EndsWith("Query") || x.EndsWith("Command")).Should().BeTrue();
    }

    [Fact]
    public void Attributes_Should_HaveAttributeSuffix()
    {
        var attributesNames = Types.InCurrentDomain()
            .That()
            .ResideInNamespace(Namespaces.SolutionName)
            .GetTypes()
            .Where(x => x.BaseType is not null && x.BaseType == typeof(Attribute))
            .Select(x => x.Name)
            .ToList();

        attributesNames.All(x => x.EndsWith("Attribute")).Should().BeTrue();
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
    public void DbModels_ShouldHaveModelSuffix()
    {
        var result = Types.InCurrentDomain()
            .That()
            .ResideInNamespace(Namespaces.ProjectNamespace(Namespaces.Infrastructure) + ".Data.Models")
            .Should()
            .HaveNameEndingWith("Model")
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
