using System.Collections;
using UnityEngine;
using System.Linq;

public class ShowHideObstacleRandomaizer : MonoBehaviour
{
    [SerializeField] private ShowObstacleGroop[] groops;
    [SerializeField] private int groopsCount = 2;
    [SerializeField] private float stepDealy = 5f;

    private void Start()
    {
        NextStep();
        StartCoroutine(MainRoutine());
    }

    public IEnumerator MainRoutine ()
    {
        while (true)
        {
            while (GamePause.IsPause)
                yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(stepDealy);            
            NextStep();
        }
    }

    private void NextStep()
    {
        foreach (var groop in groops)
        {
            groop.Hide();
        }

        ShowObstacleGroop[] groopsActive = new ShowObstacleGroop[groopsCount];
        for (int i = 0; i < groopsCount; i++)
        {
            ShowObstacleGroop groopLocal = null;
            for (int q = 0; q < 10; q++)
            {
                groopLocal = groops[Random.Range(0, groops.Length)];
                if (groopsActive.Contains(groopLocal) == false)                
                    break;                
            }

            groopsActive[i] = groopLocal;
        }

        foreach (var groop in groopsActive)
        {
            groop.Show();
        }
    }
}