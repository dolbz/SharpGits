using SharpGits.Console.Verbs;
using SharpGits.Console.Repository;
using SharpGits.Console.GitObjects;

namespace SharpGits.Console.CommandHandlers;

public class CommitCommandHandler
{
    private readonly GitRepo gitRepo;

    public CommitCommandHandler(GitRepo gitRepo)
    {
        this.gitRepo = gitRepo;
    }

    public int HandleCommand(CommitOptions commitOptions)
    {
        var files = gitRepo.Workspace.ListFiles();
        foreach (var file in files)
        {
            var fileBytes = File.ReadAllBytes(file);
            var blob = new Blob
            {
                Content = fileBytes
            };
            gitRepo.Database.StoreObject(blob);
        }
        return 0;
    }
}