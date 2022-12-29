using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceHub.ArchitectureTests;

public static class Namespaces
{
    public const string SolutionName = "SpaceHub";

    public const string Domain = "Domain";
    public const string Application = "Application";
    public const string Infrastructure = "Infrastructure";
    public const string Contracts = "Contracts";
    public const string WebServer = "Web.Server";
    public const string WebClient = "Web.Client";

    public static string ProjectNamespace(string projectName) => SolutionName + "." + projectName;
}
