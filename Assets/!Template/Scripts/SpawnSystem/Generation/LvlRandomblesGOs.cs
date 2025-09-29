using UnityEngine;

public class LvlRandomblesGOs : MonoBehaviour, ILvlRandomable
{
    [SerializeField] private GameObject[] gameObjects;

    public void Randomaze(System.Random random)
    {
        foreach (var go in gameObjects)
        {
            go.SetActive(false);
        }
        GameObject actGO = gameObjects[random.Next(gameObjects.Length)];
        actGO.SetActive(true);
        if (actGO.TryGetComponent(out ILvlRandomable randomable))
        {
            randomable.Randomaze(random);
        }
    }
}