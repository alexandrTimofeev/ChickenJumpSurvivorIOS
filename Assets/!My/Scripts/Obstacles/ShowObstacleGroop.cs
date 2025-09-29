using System.Collections;
using UnityEngine;
using System.Linq;

public class ShowObstacleGroop : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles;

    public void Show() => obstacles.ToList().ForEach((o) => o.GetComponent<IObstacleShowHide>().Show());
    public void Hide() => obstacles.ToList().ForEach((o) => o.GetComponent<IObstacleShowHide>().Hide());
}