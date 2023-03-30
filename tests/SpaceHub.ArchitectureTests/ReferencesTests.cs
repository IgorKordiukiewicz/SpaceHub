namespace SpaceHub.ArchitectureTests;

public class ReferencesTests
{
    [Theory]
    [InlineData(Namespaces.Contracts, new[] {
        Namespaces.Domain,
        Namespaces.Application,
        Namespaces.Infrastructure,
        Namespaces.WebServer,
        Namespaces.WebClient })]
    [InlineData(Namespaces.Domain, new[] { 
        Namespaces.Application, 
        Namespaces.Infrastructure, 
        Namespaces.WebServer, 
        Namespaces.WebClient })]
    [InlineData(Namespaces.Application, new[] {
        Namespaces.Infrastructure,
        Namespaces.WebServer, 
        Namespaces.WebClient })]
    [InlineData(Namespaces.Infrastructure, new[] { 
        Namespaces.WebServer, 
        Namespaces.WebClient })]
    [InlineData(Namespaces.WebServer, new[] { 
        Namespaces.WebClient })]
    [InlineData(Namespaces.WebClient, new[] { 
        Namespaces.Domain, 
        Namespaces.Application, 
        Namespaces.Infrastructure, 
        Namespaces.WebServer })]
    public void Project_ShouldNot_ReferenceOtherProjects(string project, string[] otherProjects)
    {
        var dependencies = otherProjects.Select(Namespaces.ProjectNamespace).ToArray();

        var result = Types.InCurrentDomain()
            .That()
            .ResideInNamespace(Namespaces.ProjectNamespace(project))
            .ShouldNot()
            .HaveDependencyOnAny(dependencies)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
