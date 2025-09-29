using System;
using UnityEngine;
using UnityEngine.UI;

public class AchivVisual : MonoBehaviour
{
    [SerializeField] private Image image;
    private AchivInfo achivInfo;
    private bool contains;

    public event Action<AchivInfo> OnClickOpen;

    public void Init (AchivInfo achivInfo)
    {
        this.achivInfo = achivInfo;

        contains = AchieviementSystem.IsUnlockAchiv(achivInfo.ID, true);
        image.sprite = achivInfo.GetSprite(contains);
    }   

    public void OpenViewInfo()
    {
        OnClickOpen?.Invoke(achivInfo);
    }
}