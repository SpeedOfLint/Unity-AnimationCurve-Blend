using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CurveVisualizer))]
public class CurveVisualizerEditor : Editor
{
    private CurveVisualizer _visualizer;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _visualizer = (CurveVisualizer)target;

        if (GUILayout.Button("Visualize"))
        {
            _visualizer.Visualize();
        }
    }
}