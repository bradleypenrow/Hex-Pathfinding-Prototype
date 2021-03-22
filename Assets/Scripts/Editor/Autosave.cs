using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

//This script autosaves the project when the PLAY button is pressed in Unity Editor

[InitializeOnLoad]
public class Autosave
{
    static Autosave()
    {
        EditorApplication.playmodeStateChanged += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                Debug.Log("Auto-saving all open scenes...");
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }
        };
    }
}