# Unity Animation Curve Blender 🎨

Hey there! 👋

This is a handy little Unity tool for blending two `AnimationCurve` objects together. Whether you're creating procedural animations, blending tweens, or just experimenting with curves, this utility has got you covered. 🎮  

## ✨ Features
- Blends two curves based on a percentage you specify (e.g., 20% blend).
- Smoothly handles tangents (`inTangent` and `outTangent`) so your animations look natural.
- Supports weighted tangents, preserving all those fine details from the original curves.
- Outputs a single, seamless curve that you can use right away.
- Simple API for easy integration in your Unity projects.

## 🔧 Installation
1. Clone or download the repository.
2. Copy `CurveBlender.cs` to your Unity project's `Scripts` folder.
3. Start blending curves like a pro! 🚀

## ⚠️ Limitations
1. The CurveBlender works best with blendPercentage values between `0.01f` and `0.25f`.
   - Values outside this range may produce unexpected results or overly abrupt transitions.
2. Ensure both curves have properly defined keyframes before blending. Missing or undefined keyframes can lead to inaccurate results.

## 🧑‍💻 How to Use
Here's a quick example of how to use the `CurveBlender`:

```csharp
using UnityEngine;

public class Example : MonoBehaviour
{
    public AnimationCurve Curve1;
    public AnimationCurve Curve2;

    public float BlendPercentage = 0.2f;

    void Start()
    {        
        AnimationCurve blendedCurve = CurveBlender.BlendCurves(curve1, curve2, blendPercentage);

        // Use the blended curve
        Debug.Log("Blended curve created!");
    }
}
```

🖼️ Visual Example
Below is an example of two curves being blended together based on a 20% blend percentage. The result is a seamless transition from one curve to another:

![Example](https://github.com/user-attachments/assets/c4f216d4-0a0b-4921-8183-0ab5dbe12b43)


Upper Left: `Curve 1` (Red), Bottom: `Blended Curve` (Purple), Upper Right: `Curve 2` (Blue)

## 🛠️ API Reference

**BlendCurves**

```csharp
AnimationCurve BlendCurves(AnimationCurve curve1, AnimationCurve curve2, float blendPercentage)
```

`curve1`: The first curve.
`curve2`: The second curve.
`blendPercentage`: A float (0.01f to 0.25f) representing the blend percentage.
**Returns**: A new AnimationCurve that smoothly blends the two input curves.

**CalculateTangents**

```csharp
(float inTangent, float outTangent) CalculateTangents(AnimationCurve curve, float time, float deltaTime = 0.01f)
```

`curve`: The curve to evaluate.
`time`: The time position for the tangent calculation.
`deltaTime`: The offset for calculating tangents (default: 0.01).
**Returns**: The calculated inTangent and outTangent.

**CalculateWeights**

```csharp
(float inWeight, float outWeight) CalculateWeights(AnimationCurve curve, float time)
```

`curve`: The curve to evaluate.
`time`: The time position for the weight calculation.
**Returns**: The calculated inWeight and outWeight.

##🤝 Contributing

Got suggestions, feedback, or just want to say hi? Feel free to reach out from my email or open an issue! 😊

Email: ugur@misclickgames.com

##📜 License

This project is licensed under the MIT License.
