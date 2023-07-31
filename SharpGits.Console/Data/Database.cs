namespace SharpGits.Console.Data;

public class Database
{
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
}