using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Linq;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Vector2 area;
    [SerializeField] private float destroyWait = 12f;
    private float speed = 1f;

    [Space]
    [SerializeField] private GameObject deliteVFXPref;

    private List<Coroutine> _spawnCoroutines = new List<Coroutine>();
    private List<SpawnbleObject> spawnedObjects = new List<SpawnbleObject>();
    public Action<string, float> OnInstructionProgress;

    public Action<SpawnbleObject> OnObjectTimerDestroy;
    public Action<SpawnbleObject> OnObjectSpawn;

    public void SetPhase(SpawnPhase phase)
    {
        foreach (var c in _spawnCoroutines)
            StopCoroutine(c);
        _spawnCoroutines.Clear();

        foreach (var instr in phase.Instructions)
        {
            Coroutine c = StartCoroutine(SpawnRoutine(instr));
            _spawnCoroutines.Add(c);
        }
    }

    private IEnumerator SpawnRoutine(SpawnInstruction instr)
    {
        int specalNextSpawn = instr.spawnSpecialCount;
        SpawnableItem specalSpawn = instr.spawnSpecial;
        while (true)
        {
            float wait = Random.Range(instr.SpawnInterval.x, instr.SpawnInterval.y);
            yield return new WaitForSeconds(wait / speed);
            while (GamePause.IsPause)
                yield return new WaitForEndOfFrame();

            var item = instr.GetRandomItem();
            if (specalNextSpawn == 0 && specalSpawn.Prefab != null)
            {
                item = specalSpawn;
                specalNextSpawn = instr.spawnSpecialCount;
            }
            if (item?.Prefab == null) continue;

            GameObject prefab = item.Prefab;
            GameObject ngo = Spawn(instr.id, prefab);

            if (ngo != null)
            {
                switch (instr.spawnAnimation)
                {
                    case SpawnAnimationType.None:
                        break;
                    case SpawnAnimationType.Scale:
                        ngo.transform.localScale = Vector3.zero;
                        ngo.transform.DOScale(1f, instr.durarion);
                        break;
                    default:
                        break;
                }

                OnInstructionProgress?.Invoke(instr.id, (float)(instr.spawnSpecialCount - specalNextSpawn) / (float)instr.spawnSpecialCount);
            }
            specalNextSpawn--;
        }
    }

    public virtual GameObject Spawn(string id, GameObject prefab)
    {
        float x = Random.Range(-area.x / 2f, area.x / 2f);
        float y = Random.Range(-area.y / 2f, area.y / 2f);

        GameObject ngo = SpawnPocess(id, prefab, transform.position + new Vector3(x, y, 0f));

        return ngo;
    }

    protected GameObject SpawnPocess(string id, GameObject prefab, Vector3 position)
    {
        GameObject ngo = Instantiate(prefab, position, Quaternion.identity);

        if(spawnedObjects.Contains(null))
            spawnedObjects.RemoveAll(null);

        SpawnbleObject spawnbleObject = ngo.GetComponent<SpawnbleObject> ();
        if (spawnbleObject == null)
            spawnbleObject = ngo.AddComponent<SpawnbleObject>();
        StartCoroutine(DestroySpawnbleObjct(spawnbleObject, destroyWait));

        spawnbleObject.Init(id);

        spawnedObjects.Add(spawnbleObject);
        OnObjectSpawn?.Invoke(spawnbleObject);

        return ngo;
    }

    private IEnumerator DestroySpawnbleObjct (SpawnbleObject spawnble, float destroyWait)
    {
        yield return new WaitForSeconds(destroyWait);
        if (spawnble != null && spawnble.gameObject.activeInHierarchy)
        {
            spawnble.InstDestroyVFX();
            OnObjectTimerDestroy?.Invoke(spawnble);
            Destroy(spawnble.gameObject);
        }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, area);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    /// <summary>
    /// Удаляет случайный процент объектов из списка.
    /// </summary>
    /// <param name="procent">Значение от 0 до 1 (например, 0.25 удалит 25% объектов)</param>
    public void ClearRandomObstacles(float percent, params string[] ids)
    {
        percent = Mathf.Clamp01(percent);

        // Удаление null-элементов
        spawnedObjects.RemoveAll(obj => obj == null);

        // Фильтрация по ids (если они переданы)
        List<SpawnbleObject> eligibleObjects = ids != null && ids.Length > 0
            ? spawnedObjects.Where(obj => ids.Contains(obj.IDCreater)).ToList()
            : new List<SpawnbleObject>(spawnedObjects);

        if (eligibleObjects.Count == 0) return;

        int countToRemove = Mathf.FloorToInt(eligibleObjects.Count * percent);
        if (countToRemove <= 0) return;

        // Выбор случайных объектов
        List<SpawnbleObject> toRemove = eligibleObjects.OrderBy(_ => Random.value).Take(countToRemove).ToList();

        foreach (var obj in toRemove)
        {
            if (obj != null)
            {
                if (deliteVFXPref != null)
                {
                    GameObject vfx = Instantiate(deliteVFXPref, obj.transform.position, obj.transform.rotation);
                    Destroy(vfx, 10f);
                }

                spawnedObjects.Remove(obj);
                DestroySpawnbleObjct(obj, 0f);
            }
        }
    }
}