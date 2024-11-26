using UnityEngine;

public class CurveVisualizer : MonoBehaviour
{
    public LineRenderer Curve1LineRenderer;
    public LineRenderer Curve2LineRenderer;
    public LineRenderer BlendedCurveLineRenderer;

    public AnimationCurve Curve1;
    public AnimationCurve Curve2;

    [Range(0.01f, 0.25f)]
    public float blendPercentage = 0.1f;

    private void VisualizeCurve(AnimationCurve curve, float startTime, float endTime, LineRenderer lineRenderer)
    {
        var points = 300;
        lineRenderer.positionCount = points;

        for (var i = 0; i < points; i++)
        {
            var t = Mathf.Lerp(startTime, endTime, i / (float)(points - 1)); // Map time from start to end
            var value = curve.Evaluate(t);
            lineRenderer.SetPosition(i, new Vector3(t * 4, value * 4, 0));
        }
    }

    public void Visualize()
    {
        var blendedCurve = CurveBlender.BlendCurves(Curve1, Curve2, blendPercentage);

        // Visualize the entire curve
        VisualizeCurve(Curve1, 0, 1, Curve1LineRenderer);
        VisualizeCurve(Curve2, 0, 1, Curve2LineRenderer);
        VisualizeCurve(blendedCurve, 0, 2, BlendedCurveLineRenderer);
    }
}