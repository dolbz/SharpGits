using SharpGits.Console.GitObjects;

namespace SharpGits.Tests.Utilities;

public static class RandomBlobGenerator
{
    public static Blob GenerateBlob(int minLength, int maxLength)
    {
        var rand = new Random();

        var blobContent = new byte[minLength + (int)(rand.NextDouble() * (maxLength - minLength))];
        rand.NextBytes(blobContent);

        return new Blob
        {
            Content = blobContent
        };
    }
}