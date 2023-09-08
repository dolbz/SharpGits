using SharpGits.Console.Data;

namespace SharpGits.Console.Repository;

public class GitRepo
{
    public Database Database { get; set; }
    public Workspace Workspace { get; set; }

    public GitRepo(string repoDirectory)
    {
        this.Database = new Database(repoDirectory, new BlobSerializer());
        this.Workspace = new Workspace(repoDirectory);
    }
}