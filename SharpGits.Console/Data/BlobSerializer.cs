using System.Text;
using SharpGits.Console.GitObjects;

namespace SharpGits.Console.Data;

public class BlobSerializer : IBlobSerializer
{
    public byte[] Serialize(Blob blob)
    {
        var blobHeader = "blob ";
        var blobLengthString = blob.Content.Length.ToString();

        var totalLength = blobHeader.Length + blobLengthString.Length + 1 + blob.Content.Length;

        var blobList = new List<byte>(totalLength);
        var asciiEncoding = Encoding.ASCII;

        blobList.AddRange(asciiEncoding.GetBytes(blobHeader));
        blobList.AddRange(asciiEncoding.GetBytes(blobLengthString));
        blobList.Add(0x00);
        blobList.AddRange(blob.Content);

        return blobList.ToArray();
    }

    public Blob Deserialize(byte[] blobBytes)
    {
        var blobHeader = new string(System.Text.Encoding.ASCII.GetChars(blobBytes, 0, 5));

        if (blobHeader != "blob ")
        {
            throw new ArgumentException($"Invalid git blob passed in to {nameof(BlobSerializer)}", nameof(blobBytes));
        }

        var blobContent = blobBytes.SkipWhile(x => x != 0x0).Skip(1).ToArray();

        return new Blob
        {
            Content = blobContent
        };
    }
}
