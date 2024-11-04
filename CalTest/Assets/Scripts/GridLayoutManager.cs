using UnityEngine;

public class GridLayoutManager : MonoBehaviour
{
    public int rows = 3; // Number of rows in the grid
    public int columns = 4; // Number of columns in the grid
    public float spacing = 1.5f; // Spacing between each card in world units
    public float depthOffset = -1.5f; // Depth offset for positioning cards in Z-axis

    // Calculates and returns the 3D position of a card based on its index in the grid
    public Vector3 GetCardPosition(int index)
    {
        int row = index / columns;
        int col = index % columns;

        float xPosition = col * spacing;
        float yPosition = -row * spacing; // Move downwards for each row
        float zPosition = depthOffset; // Set a consistent Z position

        return new Vector3(xPosition, yPosition, zPosition); // Position in 3D space
    }
}
