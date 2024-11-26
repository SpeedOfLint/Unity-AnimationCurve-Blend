using UnityEngine;

/// <summary>
/// A utility class for blending two AnimationCurves with customizable blend percentages.
/// Handles tangent and weight calculations to create smooth transitions.
/// </summary>
public class CurveBlender
{
    /// <summary>
    /// Blends two AnimationCurves together based on a given blend percentage.
    /// </summary>
    /// <param name="curve1">The first AnimationCurve to blend.</param>
    /// <param name="curve2">The second AnimationCurve to blend.</param>
    /// <param name="blendPercentage">A float (min 0.01 to max 0.25f) representing the blend percentage.</param>
    /// <returns>A new AnimationCurve resulting from the blend operation.</returns>
    public static AnimationCurve BlendCurves(AnimationCurve curve1, AnimationCurve curve2, float blendPercentage)
    {
        // Clamp blend percentage
        blendPercentage = Mathf.Clamp(blendPercentage, 0.01f, 0.25f);

        AnimationCurve blendedCurve = new AnimationCurve();

        // Calculate blend times
        var curve1Keys = curve1.keys;
        var curve2Keys = curve2.keys;

        var curve1StartTime = curve1Keys[0].time;
        var curve1EndTime = curve1Keys[curve1Keys.Length - 1].time;
        var blendStartTime = curve1EndTime - (curve1EndTime - curve1StartTime) * blendPercentage;

        var curve2StartTime = curve2Keys[0].time;
        var curve2EndTime = curve2Keys[curve2Keys.Length - 1].time;
        var blendEndTime = curve2StartTime + (curve2EndTime - curve2StartTime) * blendPercentage;

        // Add keyframes from curve1 outside the blend range
        foreach (var key in curve1Keys)
        {
            if (key.time < blendStartTime)
                blendedCurve.AddKey(CopyKeyframe(key));
        }

        // Add the blend start keyframe
        var curve1BlendValue = curve1.Evaluate(blendStartTime);
        var blendStartKey = new Keyframe(blendStartTime, curve1BlendValue);

        // Calculate tangents and weights for the blend start keyframe
        (blendStartKey.inTangent, blendStartKey.outTangent) = CalculateTangents(curve1, blendStartTime);
        (blendStartKey.inWeight, blendStartKey.outWeight) = CalculateWeights(curve1, blendStartTime);
        // Set the blend start keyframe to use the in weight so it transitions smoothly
        blendStartKey.weightedMode = WeightedMode.In;
        blendedCurve.AddKey(blendStartKey);

        // Add the blend end keyframe
        var curve2BlendValue = curve2.Evaluate(blendEndTime);
        var blendEndKey = new Keyframe(1 + blendEndTime, curve2BlendValue);

        // Calculate tangents and weights for the blend end keyframe
        (blendEndKey.inTangent, blendEndKey.outTangent) = CalculateTangents(curve2, blendEndTime);
        (blendEndKey.inWeight, blendEndKey.outWeight) = CalculateWeights(curve2, blendEndTime);
        // Set the blend end keyframe to use the out weight so it transitions smoothly
        blendEndKey.weightedMode = WeightedMode.Out;
        blendedCurve.AddKey(blendEndKey);

        // Add keyframes from curve2 outside the blend range
        foreach (var key in curve2Keys)
        {
            if (key.time > blendEndTime)
            {
                var adjustedTime = 1 + key.time; // Shift curve2's time
                var adjustedKey = CopyKeyframe(key);

                adjustedKey.time = adjustedTime;

                blendedCurve.AddKey(adjustedKey);
            }
        }

        return blendedCurve;
    }

    /// <summary>
    /// Copies all properties of a Keyframe, including weighted tangents.
    /// </summary>
    private static Keyframe CopyKeyframe(Keyframe original)
    {
        var copy = new Keyframe(original.time, original.value, original.inTangent, original.outTangent)
        {
            weightedMode = original.weightedMode,
            inWeight = original.inWeight,
            outWeight = original.outWeight
        };

        return copy;
    }

    /// <summary>
    /// Calculates the tangents for a given time on an AnimationCurve.
    /// </summary>
    private static (float inTangent, float outTangent) CalculateTangents(AnimationCurve curve, float time, float deltaTime = 0.01f)
    {
        var prevTime = time - deltaTime;
        var nextTime = time + deltaTime;

        var prevValue = curve.Evaluate(prevTime);
        var nextValue = curve.Evaluate(nextTime);
        var currentValue = curve.Evaluate(time);

        var inTangent = (currentValue - prevValue) / (time - prevTime);
        var outTangent = (nextValue - currentValue) / (nextTime - time);

        return (inTangent, outTangent);
    }

    /// <summary>
    /// Calculates the weights for a given time on an AnimationCurve.
    /// </summary>
    private static (float inWeight, float outWeight) CalculateWeights(AnimationCurve curve, float time)
    {
        var keys = curve.keys;

        var prevKey = (Keyframe?)null;
        var nextKey = (Keyframe?)null;

        foreach (var key in keys)
        {
            if (key.time <= time)
                prevKey = key;

            else if (key.time > time)
            {
                nextKey = key;
                break;
            }
        }

        if (prevKey == null || nextKey == null)
            return (1f, 1f); // Default weight

        var t = Mathf.InverseLerp(prevKey.Value.time, nextKey.Value.time, time);
        var inWeight = Mathf.Lerp(prevKey.Value.outWeight, nextKey.Value.inWeight, t);
        var outWeight = Mathf.Lerp(prevKey.Value.outWeight, nextKey.Value.inWeight, t);

        return (inWeight, outWeight);
    }
}
