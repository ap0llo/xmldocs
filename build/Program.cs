using System.Collections.Generic;
using Cake.AzurePipelines.Module;
using Cake.Core;
using Cake.DotNetLocalTools.Module;
using Cake.Frosting;
using Grynwald.SharedBuild;

return new CakeHost()
    // Usage of AzurePipelinesModule temporarily commented out since it does not yet support Cake 3.0
    //.UseModule<AzurePipelinesModule>()
    .UseModule<LocalToolsModule>()
    .InstallToolsFromManifest(".config/dotnet-tools.json")
    .UseSharedBuild<BuildContext>()
    .Run(args);


public class BuildContext : DefaultBuildContext
{

    public override IReadOnlyCollection<IPushTarget> PushTargets { get; } = new IPushTarget[]
    {
        new PushTarget(
            type: PushTargetType.AzureArtifacts,
            feedUrl: "https://pkgs.dev.azure.com/ap0llo/OSS/_packaging/PublicCI/nuget/v3/index.json",
            isActive: context => context.Git.IsMasterBranch || context.Git.IsReleaseBranch
        ),
        new PushTarget(
            PushTargetType.NuGetOrg,
            "https://api.nuget.org/v3/index.json",
            context => context.Git.IsReleaseBranch
        )
    };

    public BuildContext(ICakeContext context) : base(context)
    { }
}
