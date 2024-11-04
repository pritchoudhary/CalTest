using UnityEngine;
using System.Collections;
using System;

// Represents a card in the game, managing its state, flip animation, and match behavior.
// Implements the ICard and IMatchable interfaces to define modular card behavior.
public class Card : MonoBehaviour, ICard, IMatchable
{
    public event Action<Card> OnFlipped; // Event triggered when the card is flipped
    public event Action<Card, Card> OnMatched; // Event triggered when a match is found

    public string CardID { get; set; } // Unique identifier used to determine matches
    public bool IsMatched { get; set; } // Tracks if this card has already been matched

    private bool isFaceUp = false; // Indicates if the card is face-up
    public bool IsFaceUp => isFaceUp;

    // Flips the card, changing its orientation and raising the OnFlipped event
    public void Flip()
    {
        StartCoroutine(FlipAnimationCoroutine()); // Starts flip animation
        OnFlipped?.Invoke(this); // Triggers flip event
    }

    // Coroutine for flip animation, rotating the card 180 degrees on the Y-axis
    private IEnumerator FlipAnimationCoroutine()
    {
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 180f;

        // Smoothly rotates the card over 0.5 seconds
        for (float t = 0; t < 1; t += Time.deltaTime / 0.5f)
        {
            float yRotation = Mathf.Lerp(startRotation, endRotation, t);
            transform.eulerAngles = new Vector3(0, yRotation, 0);
            yield return null;
        }

        transform.eulerAngles = new Vector3(0, endRotation % 360, 0);
        isFaceUp = !isFaceUp; // Toggles face-up status
    }

    // Checks if the card matches another card based on CardID
    public bool IsMatch(ICard otherCard)
    {
        bool match = CardID == otherCard.CardID;
        if (match) OnMatched?.Invoke(this, (Card)otherCard); // Triggers match event if match is found
        return match;
    }

    // Resets the card to back-facing and unmatched state
    public void Reset()
    {
        IsMatched = false;
        isFaceUp = false;
        transform.eulerAngles = Vector3.zero; // Sets back-facing rotation
    }
}
