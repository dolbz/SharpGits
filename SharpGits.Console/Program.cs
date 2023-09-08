using CommandLine;
using SharpGits.Console.Data;
using SharpGits.Console.CommandHandlers;
using SharpGits.Console.Repository;
using SharpGits.Console.Verbs;

Parser parser = new(s => { s.AutoVersion = false; });

return parser
    .ParseArguments<InitOptions, CommitOptions>(args)
    .MapResult(
        (InitOptions opts) => RunInit(opts),
        (CommitOptions opts) => RunCommit(opts),
        errs => NotParsed(errs, args));

static int RunInit(InitOptions options)
{
    // TODO allow dir to be passed in
    var dirToInit = Directory.GetCurrentDirectory();
    Database.Init(dirToInit);
    return 0;
}

static int RunCommit(CommitOptions options)
{
    var commitHandler = new CommitCommandHandler(new GitRepo(Directory.GetCurrentDirectory()));
    return commitHandler.HandleCommand(options);
}

static int NotParsed(IEnumerable<Error> errors, string[] args)
{
    Console.WriteLine("Invalid option selected");
    return 1;
}