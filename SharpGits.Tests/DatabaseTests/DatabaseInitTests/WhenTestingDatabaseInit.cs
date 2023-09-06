using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;
using LeapingGorilla.Testing.NUnit.Composable;
using NUnit.Framework;
using SharpGits.Console.Data;

namespace SharpGits.Tests.DatabaseTests.DatabaseInitTests;

public class WhenTestingDatabaseInit : ComposableTestingTheBehaviourOf
{
    protected override ComposedTest ComposeTest() =>
      TestComposer
        .Given(ThereIsADirectoryToInitANewGitRepo)
        .When(DatabaseIsInitialized)
        .Then(DirectoriesHaveBeenCreated)
        .And(HeadFileIsCreatedWithMainRef);

    private string repoDirectory;

    [Given]
    public void ThereIsADirectoryToInitANewGitRepo()
    {
        var rootTempPath = Path.GetTempPath();
        var tempRepoDir = Guid.NewGuid().ToString();

        repoDirectory = Path.Combine(rootTempPath, tempRepoDir);
        Directory.CreateDirectory(repoDirectory);
    }

    [When]
    public void DatabaseIsInitialized()
    {
        Database.Init(repoDirectory);
    }

    [Then]
    public void DirectoriesHaveBeenCreated()
    {
        var gitDir = Path.Combine(repoDirectory, ".git");
        var refsDir = Path.Combine(gitDir, "refs");
        var objectsDir = Path.Combine(gitDir, "objects");

        Assert.That(Directory.Exists(gitDir));
        Assert.That(Directory.Exists(refsDir));
        Assert.That(Directory.Exists(objectsDir));
    }

    [Then]
    public void HeadFileIsCreatedWithMainRef()
    {
        var headFile = Path.Combine(repoDirectory, ".git", "HEAD");

        Assert.That(File.Exists(headFile));

        var headFileContent = File.ReadAllText(headFile);

        Assert.That(headFileContent, Is.EqualTo("ref: refs/heads/main\n"));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        Directory.Delete(repoDirectory, recursive: true);
    }
}