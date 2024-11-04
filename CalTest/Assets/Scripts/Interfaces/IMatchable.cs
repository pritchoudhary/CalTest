// Interface for matchable behavior, allowing cards to verify if they match another card.
public interface IMatchable
{
    bool IsMatch(ICard otherCard);  // Checks if this card matches another card
}
