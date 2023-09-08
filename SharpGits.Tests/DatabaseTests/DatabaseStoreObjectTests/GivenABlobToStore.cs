using System.IO.Compression;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;
using NSubstitute;
using NUnit.Framework;
using SharpGits.Console.GitObjects;

namespace SharpGits.Tests.DatabaseTests.DatabaseStoreObjectTests;

public class GivenABlobToStore : WhenTestingDatabase
{
    private Blob blobToSerialize;
    private byte[] serializedBlobBytes = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd };

    // a7723c7441bf4fe39917a8a23670ca1886aabcf1 is the SHA1 hash of the bytes above ^
    // This demonstrates that the stored bytes are the ones returned by the (mocked in this case) BlobSerializer
    private string BlobHashPath => Path.Combine(WorkspacePath, ".git/objects/a7/723c7441bf4fe39917a8a23670ca1886aabcf1");

    protected override ComposedTest ComposeTest() =>
      TestComposer
        .Given(ARepoExists)
        .And(ThereIsABlobToBeSerialized)
        .And(TheBlobSerializerKnowsHowToSerializeTheBlob)
        .When(TheBlobIsStored)
        .Then(TheBlobIsSerialized)
        .And(TheBlobFileIsCreatedInTheRepo)
        .And(TheBlobFileCanBeZlibDecompressedToTheSerializedBytes);


    [Given]
    public void ThereIsABlobToBeSerialized()
    {
        blobToSerialize = new Blob();
    }

    [Given]
    public void TheBlobSerializerKnowsHowToSerializeTheBlob()
    {
        BlobSerializer
          .Serialize(blobToSerialize)
          .Returns(serializedBlobBytes);
    }

    [When]
    public void TheBlobIsStored()
    {
        Database.StoreObject(blobToSerialize);
    }

    [Then]
    public void TheBlobIsSerialized()
    {
        BlobSerializer
          .Received(1)
          .Serialize(blobToSerialize);
    }

    [Then]
    public void TheBlobFileIsCreatedInTheRepo()
    {
        Assert.That(File.Exists(BlobHashPath));
    }

    [Then]
    public void TheBlobFileCanBeZlibDecompressedToTheSerializedBytes()
    {
        using var fileStream = File.OpenRead(BlobHashPath);
        using var decompressionStream = new ZLibStream(fileStream, CompressionMode.Decompress);

        byte[] decompressedBytes = new byte[serializedBlobBytes.Length];
        var bytesRead = decompressionStream.Read(decompressedBytes);

        Assert.That(bytesRead, Is.EqualTo(serializedBlobBytes.Length));
        Assert.That(decompressionStream.ReadByte(), Is.EqualTo(-1));
        Assert.That(decompressedBytes, Is.EquivalentTo(serializedBlobBytes));
    }
}