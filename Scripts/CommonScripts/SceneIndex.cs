using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneNumbers", menuName = "SceneNumber", order = 65)]
public class SceneIndex : ScriptableObject
{

    [SerializeField] private List<SceneHash> scenes;
    
    public List<SceneHash> Scenes
    {
        get
        {
            return scenes;
        }
    }
    public int FindSceneIndex(string sceneName)
    {


        foreach (var scene in Scenes)
        {
            if (scene.sceneName.Equals(sceneName))
            {
                return scene.sceneIndex;
            }
        }
        
        Debug.LogError("Scene does not exist");
        return -1;
    }


}
[Serializable]
public struct SceneHash
{
    public string sceneName;
    public int sceneIndex;
}
