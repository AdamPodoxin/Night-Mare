[System.Serializable]
public class GameProgress
{
    public string seed = "NSEED";
    public bool hasReadPrompts = false;

    public GameProgress()
    {
        seed = "NSEED";
        hasReadPrompts = false;
    }

    public GameProgress(string seed, bool hasReadPrompts)
    {
        this.seed = seed;
        this.hasReadPrompts = hasReadPrompts;
    }
}
