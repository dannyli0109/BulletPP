using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator mapGenerator = (MapGenerator)target;
        if (GUILayout.Button("Generate Object"))
        {
            mapGenerator.GenerateMap();
        }

        if (GUILayout.Button("Clear tiles"))
        {
            mapGenerator.ClearTileMap();
        }
    }
}
