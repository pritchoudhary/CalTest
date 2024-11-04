// Interface for managing game scoring logic, including combo calculations.
public interface IScoreService
{
    int Score { get; }
    int ComboMultiplier { get; }
    void AddMatchScore();
    void ResetCombo();
    void ResetScore();
}
