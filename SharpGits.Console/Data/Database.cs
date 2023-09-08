using SharpGits.Console.GitObjects;

namespace SharpGits.Console.Data;

public class Database
{
    public Database(string workspacePath, IBlobSerializer blobSerializer)
    {
    }

    public static void Init(string workspacePath)
    {
        // TODO Create git database directory and appropriate subdirs.
        throw new NotImplementedException();
    }

    public void StoreObject(GitObject obj)
    {
        throw new NotImplementedException();
    }
}