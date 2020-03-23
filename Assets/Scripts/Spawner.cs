using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum GizmoType { Never, SelectedOnly, Always }

    public Boid prefab;
    public float spawnRadius = 1;
    public int spawnCount = 32;
    public Color colour;
    public GizmoType showSpawnRegion;

    void Awake()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Boid boid = Instantiate(prefab);
            boid.transform.position = transform.position + Random.insideUnitSphere * spawnRadius;
            boid.transform.forward = Random.onUnitSphere;
            boid.Color = colour;
        }
    }

    private void OnDrawGizmos()
    {
        if (showSpawnRegion == GizmoType.Always)
            DrawGizmos();
    }

    void OnDrawGizmosSelected()
    {
        if (showSpawnRegion == GizmoType.SelectedOnly)
            DrawGizmos();
    }

    private void DrawGizmos()
    {
        Gizmos.color = new Color(colour.r, colour.g, colour.b, 0.25f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
}
