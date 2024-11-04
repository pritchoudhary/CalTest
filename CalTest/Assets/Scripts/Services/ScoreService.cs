//Manages the scoring logic of the game, including combo calculations
public class ScoreService : IScoreService
{
    public int Score { get; private set; } //Current score of the player
    public int ComboMultiplier { get; private set; } = 1; //Multiplier for consecutive matches

    //Adds score based on the current combo multiplier
    public void AddMatchScore()
    {
        Score += 10 * ComboMultiplier;
        ComboMultiplier++;
    }

    //Resets combo multiplier after a mismatch
    public void ResetCombo()
    {
        ComboMultiplier = 1;
    }

    //Resets the entire score, typically will be used when restarting the game
    public void ResetScore()
    {
        Score = 0;
        ComboMultiplier = 1;
    }
}
