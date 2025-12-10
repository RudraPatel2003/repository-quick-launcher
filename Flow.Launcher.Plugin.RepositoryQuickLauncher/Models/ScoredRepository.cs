namespace Flow.Launcher.Plugin.RepositoryQuickLauncher.Models;

public class ScoredRepository
{
    public Repository Repository { get; set; }
    public int Score { get; set; }

    public ScoredRepository(Repository repository, int score)
    {
        Repository = repository;
        Score = score;
    }
}
