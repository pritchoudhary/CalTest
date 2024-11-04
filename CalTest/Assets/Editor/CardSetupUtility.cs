using UnityEditor;
using UnityEngine;

public class CardSetupUtility : Editor
{
    [MenuItem("Tools/Setup All Cards")]
    public static void SetupAllCards()
    {
        // Path to the folder containing the card models
        string path = "Assets/Resources/PlayingCards";

        // Find all model files (.fbx) in the specified path
        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { path });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject cardModel = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (cardModel != null)
            {
                // Create an instance of the card model to modify and save as a prefab
                GameObject cardInstance = (GameObject)PrefabUtility.InstantiatePrefab(cardModel);

                // Set the initial rotation to (0, 0, 0) to ensure it faces backward
                Transform cardTransform = cardInstance.GetComponent<Transform>();
                cardTransform.localRotation = Quaternion.Euler(0, 180, 0);
                cardTransform.localScale = Vector3.one;

                // Add the Card script if it's not already attached
                if (cardInstance.GetComponent<Card>() == null)
                {
                    cardInstance.AddComponent<Card>();
                }

                // Add a Box Collider if not already present and set the specified properties
                BoxCollider boxCollider = cardInstance.GetComponent<BoxCollider>();
                if (boxCollider == null)
                {
                    boxCollider = cardInstance.AddComponent<BoxCollider>();
                }

                // Set the collider Center and Size as per the provided values
                boxCollider.center = new Vector3(-0.0009458531f, 0, 0.001505613f);
                boxCollider.size = new Vector3(0.07535303f, 0.1f, 0.01708531f);

                // Generate a unique prefab path for each card
                string prefabPath = $"Assets/Prefabs/{cardModel.name}.prefab";

                // Save the configured card as a prefab in the specified path
                PrefabUtility.SaveAsPrefabAsset(cardInstance, prefabPath);

                // Destroy the temporary instance after saving the prefab
                DestroyImmediate(cardInstance);

                Debug.Log($"Setup completed for: {cardModel.name}");
            }
        }

        // Refresh the AssetDatabase to show new prefabs in the Project window
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("All card models have been set up and saved as individual prefabs.");
    }
}
