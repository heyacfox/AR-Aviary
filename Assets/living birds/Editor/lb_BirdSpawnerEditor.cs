using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(lb_BirdSpawner))]
public class lb_BirdSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        lb_BirdSpawner controller = (lb_BirdSpawner)target;

        if (GUILayout.Button("Spawn Robin"))
        {
            controller.SpawnBird("lb_robin");
        }
        if (GUILayout.Button("Spawn Blue Jay"))
        {
            controller.SpawnBird("lb_blueJay");
        }
        if (GUILayout.Button("Spawn Chickadee"))
        {
            controller.SpawnBird("lb_chickadee");
        }
        if (GUILayout.Button("Spawn Sparrow"))
        {
            controller.SpawnBird("lb_sparrow");
        }
        if (GUILayout.Button("Spawn Cardinal"))
        {
            controller.SpawnBird("lb_cardinal");
        }
    }
}