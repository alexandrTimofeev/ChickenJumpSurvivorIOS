using System.Collections;
using UnityEngine;

public class CircleSpawner : Spawner
{
    [Space]
    [SerializeField] private CircleZone2D circleZone;

    public override GameObject Spawn(string id, GameObject prefab)
    {
        float posAngle = Random.Range(0f, 360f);
        Vector3 position = circleZone.GetWorldPosition(posAngle, 0f);

        while (Physics2D.OverlapCircle(circleZone.GetWorldPosition(posAngle, -0.35f), 1f) != null)
        {
            posAngle = Random.Range(0f, 360f);
            position = circleZone.GetWorldPosition(posAngle, 0f);
        }

        GameObject ngo = SpawnPocess(id, prefab, position);
        CirclePositionTracker2D tracker2D = ngo.GetComponent<CirclePositionTracker2D>();
        tracker2D.circle = circleZone;
        tracker2D.SetPosition(posAngle);
        tracker2D.UpdatePosition();

        return ngo;
    }

    public override void OnDrawGizmos()
    {
        
    }
}