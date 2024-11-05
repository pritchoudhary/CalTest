using UnityEngine;
using System;
using System.Collections;

// Represents a card in the game, managing its state, flip animation, and match behavior.
public class Card : MonoBehaviour, ICard, IMatchable
{
    public event Action<Card> OnFlipped; // Event triggered when the card is flipped
    public string CardID { get; set; } // Unique identifier used to determine matches
    public bool IsMatched { get; set; } = false; // Tracks if this card has already been matched

    private bool isFaceUp = false; // Indicates if the card is face-up
    public bool IsFaceUp => isFaceUp; // Public getter to check if the card is face-up

    // Flips the card to face up, triggering the flip animation and invoking the OnFlipped event.
    public void Flip()
    {
        // Prevent flipping if the card is already matched or currently face-up
        if (IsMatched || isFaceUp) return;

        StartCoroutine(FlipAnimationCoroutine(true)); // Flip to face-up
        OnFlipped?.Invoke(this); // Trigger the OnFlipped event
    }

    // Flips the card back to the original (back-facing) state if it is currently face-up.
    public void FlipBack()
    {
        if (isFaceUp)
        {
            Debug.Log($"Flipping back card: {CardID}"); // Debug message
            StartCoroutine(FlipAnimationCoroutine(false)); // Trigger the flip animation back to face-down
        }
    }

    // Implements the IsMatch method to check if two cards have the same CardID
    public bool IsMatch(ICard otherCard)
    {
        return otherCard != null && CardID == otherCard.CardID; // Check for matching CardID
    }

    // Resets the card to the initial back-facing state
    public void Reset()
    {
        IsMatched = false;
        isFaceUp = false;
        transform.localEulerAngles = new Vector3(0, 180, 0); // Set to back-facing rotation
    }

    // Coroutine to animate the flip action of the card, toggling between face-up and face-down.
    private IEnumerator FlipAnimationCoroutine(bool faceUp)
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotation = faceUp ? 0f : 180f;
        float duration = 0.3f;
        float elapsedTime = 0f;

        // Smoothly rotate the card to the target rotation
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / duration);
            transform.localEulerAngles = new Vector3(0, yRotation, 0);
            yield return null;
        }

        // Explicitly set the final rotation to ensure precision
        transform.localEulerAngles = new Vector3(0, endRotation, 0);
        isFaceUp = faceUp; // Update face-up status
    }

    private void Update()
    {
        // Handle input for both desktop and mobile
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) // For desktop mouse click
        {
            HandleMouseClick();
        }

        if (Input.touchCount > 0) // For mobile touch input
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleTouch(touch.position);
            }
        }
    }

    private void HandleMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform) // Check if the card was clicked
            {
                Flip(); // Trigger the flip action
            }
        }
    }

    private void HandleTouch(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform) // Check if the card was touched
            {
                Flip(); // Trigger the flip action
            }
        }
    }
}
