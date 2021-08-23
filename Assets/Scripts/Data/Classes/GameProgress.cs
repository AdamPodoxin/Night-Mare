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
}
