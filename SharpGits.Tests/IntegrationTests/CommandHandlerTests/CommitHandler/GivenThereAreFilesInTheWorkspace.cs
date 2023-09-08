using System.IO.Compression;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;
using SharpGits.Console.Data;
using SharpGits.Console.Verbs;
using SharpGits.Tests.Extensions;

namespace SharpGits.Tests.IntegrationTests.CommandHandlerTests;

public class GivenThereAreFilesInTheWorkspace : WhenTestingCommitCommandHandler
{
    protected override ComposedTest ComposeTest() =>
        TestComposer
            .Given(ThereAreFilesInTheWorkspaceWithKnownBlobHashes)
            .When(CommitIsRequested)
            .Then(TheExpectedGitObjectPathsAreCreated)
            .Then(TheFileContentsContainExpectedBlobs);

    private string file1Path;
    private string file2Path;
    private string file1BlobPath;
    private string file2BlobPath;

    private string file1Contents = "File 1\n";
    private string file2Contents = "File 2\n";

    [Given]
    public void ThereAreFilesInTheWorkspaceWithKnownBlobHashes()
    {
        file1Path = Path.Combine(RepoDir, "file1");
        file2Path = Path.Combine(RepoDir, "file2");

        File.WriteAllText(file1Path, file1Contents);
        File.WriteAllText(file2Path, file2Contents);

        file1BlobPath = Path.Combine(RepoDir, ".git", "objects", "50", "fcd26d6ce3000f9d5f12904e80eccdc5685dd1");
        file2BlobPath = Path.Combine(RepoDir, ".git", "objects", "44", "75433e279a71203927cbe80125208a3b5db560");
    }

    [When]
    public void CommitIsRequested()
    {
        CommitHandler.HandleCommand(new CommitOptions());
    }

    [Then]
    public void TheExpectedGitObjectPathsAreCreated()
    {
        Assert.That(File.Exists(file1BlobPath));
        Assert.That(File.Exists(file2BlobPath));
    }

    [Then]
    public void TheFileContentsContainExpectedBlobs()
    {
        var blobSerializer = new BlobSerializer();

        using var file1Stream = File.OpenRead(file1BlobPath);
        using var decompressionStream1 = new ZLibStream(file1Stream, CompressionMode.Decompress);

        byte[] decompressedBytes1 = new byte[100];
        decompressionStream1.Read(decompressedBytes1);

        var blob1 = blobSerializer.Deserialize(decompressedBytes1);

        using var file2Stream = File.OpenRead(file2BlobPath);
        using var decompressionStream2 = new ZLibStream(file2Stream, CompressionMode.Decompress);

        byte[] decompressedBytes2 = new byte[100];
        decompressionStream2.Read(decompressedBytes2);

        var blob2 = blobSerializer.Deserialize(decompressedBytes2);

        Assert.That(blob1.Content, Is.EquivalentTo(file1Contents.AsBytes()));
        Assert.That(blob2.Content, Is.EquivalentTo(file2Contents.AsBytes()));
    }
}