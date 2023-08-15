using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;
using SharpGits.Console.GitObjects;
using SharpGits.Tests.Extensions;

namespace SharpGits.Tests.DatabaseTests.BlobSerializerTests;

public class GivenThereIsASerializedBlobToDeserialize : WhenTestingBlobSerializer
{
    protected override ComposedTest ComposeTest() =>
      TestComposer
        .Given(ThereIsASerializedBlob)
        .When(TheBlobIsDeserialized)
        .Then(TheBlobContentIsAsExpected);

    private byte[] serializedBlob;

    private Blob deserializedBlob;

    [Given]
    public void ThereIsASerializedBlob()
    {
        // Git Blob representing a file containing "Hello World\n"
        //                               b     l     o     b  <spc>    1     2 <null>    H     e     l     l     o  <spc>    W     o     r     l     d    \n
        serializedBlob = new byte[] { 0x62, 0x6c, 0x6f, 0x62, 0x20, 0x31, 0x32, 0x00, 0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x57, 0x6f, 0x72, 0x6c, 0x64, 0x0a };
    }

    [When]
    public void TheBlobIsDeserialized()
    {
        deserializedBlob = Serializer.Deserialize(serializedBlob);
    }

    [Then]
    public void TheBlobContentIsAsExpected()
    {
        Assert.That(deserializedBlob.Content, Is.EquivalentTo("Hello World\n".AsBytes()));
    }
}