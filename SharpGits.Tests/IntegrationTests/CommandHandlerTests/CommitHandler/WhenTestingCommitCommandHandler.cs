using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Composable;
using NUnit.Framework;
using SharpGits.Console.CommandHandlers;
using SharpGits.Console.Repository;
using SharpGits.Tests.Utilities;

namespace SharpGits.Tests.IntegrationTests.CommandHandlerTests;

public abstract class WhenTestingCommitCommandHandler : ComposableTestingTheBehaviourOf
{
    [Dependency]
    public GitRepo Repo { get; set; }

    public string RepoDir { get; set; }

    [ItemUnderTest]
    public CommitCommandHandler CommitHandler { get; set; }

    protected override void CreateManualDependencies()
    {
        RepoDir = RepoHelper.CreateTemporaryRepo();
        Repo = new GitRepo(RepoDir);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        Directory.Delete(RepoDir, recursive: true);
    }
}
