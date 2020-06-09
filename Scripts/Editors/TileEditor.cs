using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editors
{
#if UNITY_EDITOR
    public enum Tiles
    {
        Desert = 0,
        Pasture = 1,
        Hills = 2,
        Mountains = 3,
        Fields = 4,
        Forest = 5
    }

    public class TileEditor : EditorWindow
    {
        public int selectedIndex = 0;
        public int tileSize = 3;
        public GameObject parentObject;
        private List<GameObject> objectList = new List<GameObject>(20);
        private List<string> stringList = new List<string>(20);
        Vector2 scrollPos;

        [MenuItem("Window/Custom Editors/Tile Editor")]
        public static void ShowWindow()
        {
            GetWindow<TileEditor>("Tile Editor");
        }

        private void Awake()
        {
            for (int i = 0; i < 20; i++)
            {
                objectList.Add(null);
                stringList.Add("Empty Type");
            }
        }
        private void OnGUI()
        {
            GUILayout.Label("Add Tile", EditorStyles.boldLabel);
            parentObject = (GameObject) EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);

            selectedIndex = EditorGUILayout.Popup(selectedIndex, stringList.GetRange(0, tileSize).ToArray());
            if (GUILayout.Button("Add to Scene"))
                AddTile(selectedIndex);
            GUILayout.Label("Tile Types", EditorStyles.boldLabel);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
            for (int i = 0; i < tileSize; i++)
            {
                AddTileType(i);
            }


            if (GUILayout.Button("Add New Type"))
            {
                if(tileSize < 20)
                    tileSize++;
            }
            

            EditorGUILayout.EndScrollView();
        }

        private void AddTileType(int i)
        {
            EditorGUILayout.BeginVertical("GroupBox");
            stringList[i] = EditorGUILayout.TextField("Tile Type", stringList[i]);
            objectList[i] = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", objectList[i], typeof(GameObject), true);
            //objectList.Add(tmpObject);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add to Scene"))
                AddTile(i);
            if (GUILayout.Button("Remove Type"))
                RemoveGroup(i);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void RemoveGroup(int i)
        {
            objectList.RemoveAt(i);
            stringList.RemoveAt(i);
            tileSize--;
        }

        private void AddTile(int index)
        {
            if(!parentObject)
            {
                parentObject = new GameObject("TileMap");
            }

            GameObject tileObject;
            if (!objectList[index])
                return;

            tileObject = Instantiate(objectList[index], new Vector3(0f, 0f, 0f), Quaternion.identity);
            tileObject.transform.SetParent(parentObject.transform);
            /*        switch (tile)
                {
                    case Tiles.Fields:

                        break;
                    case Tiles.Forest:
                        tileObject = Instantiate((GameObject) forestTile, new Vector3(0f, 0f, 0f), Quaternion.identity);
                        break;
                    case Tiles.Hills:
                        tileObject = Instantiate((GameObject) hillsTile, new Vector3(0f, 0f, 0f), Quaternion.identity);
                        break;
                    case Tiles.Mountains:
                        tileObject = Instantiate((GameObject) mountainTile, new Vector3(0f, 0f, 0f), Quaternion.identity);
                        break;
                    case Tiles.Pasture:
                        tileObject = Instantiate((GameObject) pastureTile, new Vector3(0f, 0f, 0f), Quaternion.identity);
                        break;
                    case Tiles.Desert:
                    default:
                        tileObject = Instantiate((GameObject) desertTile, new Vector3(0f, 0f, 0f), Quaternion.identity);
                        break;
                }*/


        }
    }
    #endif
}