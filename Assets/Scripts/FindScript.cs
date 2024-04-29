using UnityEngine;
using System;
using System.Collections.Generic;

public class FindScript : MonoBehaviour
{
    // The script type you want to search for
    public enum Type 
    {
        PipesContainerManager,
        PipeCollisionDetector,
        PipeGrabFreeTransformer,
        WallCollisionDetector
    }
    public string scriptType;

    void Start()
    {
        // Call the method to search for game objects with the specified script
        SearchGameObjectsWithScript();
    }

    void SearchGameObjectsWithScript()
    {
        // List to hold references to game objects with the specified script
        List<GameObject> gameObjectsWithScript = new List<GameObject>();

        // Find all game objects in the scene
        GameObject[] allGameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        // Iterate through each game object
        foreach (GameObject go in allGameObjects)
        {
            // Check if the game object has the specified script attached
            if (go.GetComponent(scriptType) != null)
            {
                // If the script is found, add the game object to the list
                gameObjectsWithScript.Add(go);
            }
        }

        // Output the list of game objects with the specified script
        foreach (GameObject go in gameObjectsWithScript)
        {
            Debug.Log("Found game object with script: " + go.name);
        }
    }
}
