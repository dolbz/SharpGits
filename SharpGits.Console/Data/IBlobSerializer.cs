using SharpGits.Console.GitObjects;

namespace SharpGits.Console.Data;

public interface IBlobSerializer
{
    byte[] Serialize(Blob blob);

    Blob Deserialize(byte[] blobBytes);
}