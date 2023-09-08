using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;
using NUnit.Framework;
using SharpGits.Console.GitObjects;
using SharpGits.Tests.Extensions;
using SharpGits.Tests.Utilities;

namespace SharpGits.Tests.DatabaseTests.BlobSerializerTests;

public class GivenABlobToSerialize : WhenTestingBlobSerializer
{
    protected override ComposedTest ComposeTest() =>
      TestComposer
        .Given(ThereIsARandomBlobToBeSerialized)
        .When(TheBlobIsSerialized)
        .Then(ObjectTypeIsEncodedInOutput)
        .And(AsciiEncodedContentLengthAfterObjectType)
        .And(ThereIsANullByteAfterObjectTypeAndContentLength)
        .And(TheContentTakesTheRestOfTheSpace);

    private Blob theBlob;

    private byte[] serializedResult;
    private int contentLength;
    private string contentLengthString;

    [Given]
    public void ThereIsARandomBlobToBeSerialized()
    {
        // Random blob of length 50 - 250 bytes
        theBlob = RandomBlobGenerator.GenerateBlob(minLength: 50, maxLength: 250);
        contentLength = theBlob.Content.Length;
        contentLengthString = contentLength.ToString();
    }

    [When]
    public void TheBlobIsSerialized()
    {
        serializedResult = Serializer.Serialize(theBlob);
    }

    [Then]
    public void ObjectTypeIsEncodedInOutput()
    {
        var blobTypeBytes = serializedResult.Take(5);
        Assert.That(blobTypeBytes, Is.EquivalentTo("blob ".AsBytes()));
    }

    [Then]
    public void AsciiEncodedContentLengthAfterObjectType()
    {

        var contentLengthSegment = serializedResult.Skip(5).Take(contentLengthString.Length);

        Assert.That(contentLengthSegment, Is.EquivalentTo(contentLengthString.AsBytes()));
    }

    [Then]
    public void ThereIsANullByteAfterObjectTypeAndContentLength()
    {
        Assert.That(serializedResult[5 + contentLengthString.Length], Is.EqualTo(0x0));
    }

    [Then]
    public void TheContentTakesTheRestOfTheSpace()
    {
        var contentSegment = serializedResult.Skip(6 + contentLengthString.Length);

        Assert.That(contentSegment, Is.EquivalentTo(theBlob.Content));
    }
}