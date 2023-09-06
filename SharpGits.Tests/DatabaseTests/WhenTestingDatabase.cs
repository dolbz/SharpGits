using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Composable;
using NUnit.Framework;
using SharpGits.Console.Data;

namespace SharpGits.Tests.DatabaseTests;

public abstract class WhenTestingDatabase : ComposableTestingTheBehaviourOf
{
    [Dependency]
    public IBlobSerializer BlobSerializer { get; set; }

    [Dependency]
    public string WorkspacePath { get; set; }

    [ItemUnderTest]
    public Database Database { get; set; }


    [Given]
    public void ARepoExists()
    {
        Database.Init(WorkspacePath);
    }

    protected override void CreateManualDependencies()
    {
        WorkspacePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        Directory.Delete(WorkspacePath, recursive: true);
    }
}