using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Базовый класс всех захватываемых объектов.
/// </summary>
[CreateAssetMenu(fileName = "GrappableObjectBehaviour", menuName = "SGames/GrapSystem/GrappableObjectBehaviour")]
public class GrappableObjectBehaviour : ScriptableObject
{
    public string ID;
    [SerializeReference]
    public List<GrappableObjectBehaviourAction> behaviourActions;
}

[System.Serializable]
public abstract class GrappableObjectBehaviourAction
{
    public string Title;
}
