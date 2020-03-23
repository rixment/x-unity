using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject 
{    
    public PairFloat speed;

    [Range(0.5f, 3.0f)]
    public float perceptionRadius = 2.0f;
    public float avoidanceRadius = 1;
    public float maxSteerForce = 3;

    [Header ("Flock Grouping")]
    public PairFloat weightAlignment;
    public PairFloat weightCohesion;
    public PairFloat weightSeparation;
    
    [Header ("Collisions")]
    public float avoidCollisionWeight = 10;
    public float collisionAvoidDst = 5;

    [Header("Global Behaviour")]
    [Range(0.1f, 1.0f)]
    public float multiplierAlignment = 1.0f;
    [Range(0.1f, 1.0f)]
    public float multiplierCohesion = 1.0f;
    [Range(0.1f, 1.0f)]
    public float multiplierSeparation = 1.0f;
}
