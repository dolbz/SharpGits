using SharpGits.Console.Verbs;
using SharpGits.Console.Repository;

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
        throw new NotImplementedException();
    }
}