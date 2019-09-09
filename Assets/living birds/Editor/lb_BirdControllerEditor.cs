using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(lb_BirdController))]
public class lb_BirdControllerEditor : Editor
{
    
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
    lb_BirdController controller = (lb_BirdController)target;

        if (GUILayout.Button("Spawn Robin"))
        {
            controller.SpawnSpecificBird("lb_robin");
        }
    }
}