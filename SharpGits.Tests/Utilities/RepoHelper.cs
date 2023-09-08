using SharpGits.Console.Data;

namespace SharpGits.Tests.Utilities;

public class RepoHelper
{
    public static string CreateTemporaryRepo(bool initialized = true)
    {
        var rootTempPath = Path.GetTempPath();
        var tempRepoDir = Guid.NewGuid().ToString();

        var repoDirectory = Path.Combine(rootTempPath, tempRepoDir);
        Directory.CreateDirectory(repoDirectory);

        if (initialized)
        {
            // Note: this will only work after the Database.Init method has been implemented after session 1.
            // But this helper class only exists from session 2 onwards so that's ok!
            Database.Init(repoDirectory);
        }

        return repoDirectory;
    }
}