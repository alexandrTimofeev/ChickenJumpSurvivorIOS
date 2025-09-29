using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HorizontalPatternGenerator", menuName = "SGames/Generation/PatternData")]
public class HorizontalPatternGenerationData : ScriptableObject
{
    public List<GameObject> patterns;
    public int seedKey = 12345;
    public float distance = 10f;
    public Vector3 startPoint = Vector3.zero;
}