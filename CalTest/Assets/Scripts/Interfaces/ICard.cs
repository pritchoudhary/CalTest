// Defines the core behavior of a card in the game, including matching and flipping.
public interface ICard : IMatchable
{
    string CardID { get; set; }     // Unique identifier for matching
    bool IsMatched { get; set; }    // Status indicating if the card is matched
    void Flip();                    // Initiates the flip animation
    void Reset();                   // Resets the card state
}
