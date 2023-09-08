namespace SharpGits.Console.Data;

public class Workspace
{
    private readonly string rootDirectory;

    public Workspace(string rootDirectory)
    {
        this.rootDirectory = rootDirectory;
    }

    public IEnumerable<string> ListFiles()
    {
        return Directory.EnumerateFiles(rootDirectory);
    }
}