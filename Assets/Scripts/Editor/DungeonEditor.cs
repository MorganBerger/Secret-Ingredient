using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DungeonGenerator generator = (DungeonGenerator)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Generate Dungeon", GUILayout.Height(30)))
        {
            int randomSeed = Random.Range(1, 100000);
            generator.GenerateDungeon(randomSeed);
        }
        
        if (GUILayout.Button("Clear Dungeon", GUILayout.Height(30)))
        {
            generator.ClearDungeon();
        }
        if (GUILayout.Button("Close Open Exits", GUILayout.Height(30)))
        {
            List<Vector2Int> remainingPositions = generator.GetRemainingPositionsToFill();
            generator.CloseOpenExits(remainingPositions);
        }
    }
}