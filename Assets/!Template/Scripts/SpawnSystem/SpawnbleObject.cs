using System.Collections;
using UnityEngine;

public class SpawnbleObject : MonoBehaviour
{
    public string IDCreater { get; private set; }

    [Space]
    [SerializeField] private GameObject spawnVFX;
    [SerializeField] private GameObject destroyVFX;
    private bool isDes;

    public void Init (string id)
    {
        IDCreater = id;
        InstSpawnVFX();
    }

    public void InstDestroyVFX()
    {
        if (destroyVFX && isDes == false)
            Destroy(Instantiate(destroyVFX, transform.position, transform.rotation), 10f);
    }

    public void InstSpawnVFX()
    {
        if (spawnVFX)
            Destroy(Instantiate(spawnVFX, transform.position, transform.rotation), 10f);
    }

    public void DoDes (bool des)
    {
        isDes = des;
    }
}