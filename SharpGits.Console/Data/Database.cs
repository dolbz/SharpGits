using System.IO.Compression;
using System.Security.Cryptography;
using SharpGits.Console.GitObjects;

namespace SharpGits.Console.Data;

public class Database
{
    private readonly string databasePath;
    private readonly IBlobSerializer blobSerializer;

    public Database(string workspacePath, IBlobSerializer blobSerializer)
    {
        databasePath = Path.Combine(workspacePath, ".git");
        this.blobSerializer = blobSerializer;
    }

    public static void Init(string workspacePath)
    {
        var gitDir = Path.Combine(workspacePath, ".git");

        if (Directory.Exists(gitDir))
        {
            return;
        }

        Directory.CreateDirectory(gitDir);

        var refsDir = Path.Combine(gitDir, "refs");
        var objectssDir = Path.Combine(gitDir, "objects");

        Directory.CreateDirectory(refsDir);
        Directory.CreateDirectory(objectssDir);

        var headFile = Path.Combine(gitDir, "HEAD");
        File.WriteAllText(headFile, "ref: refs/heads/main\n");
    }

    public void StoreObject(GitObject obj)
    {
        switch (obj)
        {
            case Blob blob:
                StoreSerializedObject(blobSerializer.Serialize(blob));
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void StoreSerializedObject(byte[] obj)
    {
        var objectHash = HashSerializedObject(obj);
        var subFolder = objectHash[0].ToString("x2");
        var fileName = string.Concat(objectHash.Skip(1).Select(x => x.ToString("x2")));

        var objectPath = Path.Combine(databasePath, "objects", subFolder, fileName);

        var fileInfo = new FileInfo(objectPath);
        fileInfo.Directory.Create();

        using var fileStream = File.OpenWrite(objectPath);
        using (var compressedStream = new ZLibStream(fileStream, CompressionMode.Compress))
        {
            compressedStream.Write(obj, 0, obj.Length);
        }

        fileStream.Close();
    }

    private byte[] HashSerializedObject(byte[] obj)
    {
        var sha1Digester = SHA1.Create();
        var hashResult = sha1Digester.ComputeHash(obj);
        return hashResult;
    }
}