using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchivInfoWindowUI : WindowUI
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textInfo;
    private AchivInfo achivInfo;

    public override void Open()
    {
        base.Open();
    }

    public void Init(AchivInfo achiv, Achiviement[] allAchiviements = null, bool? forceTestCondition = null)
    {
        achivInfo = achiv;
        bool contains = forceTestCondition != null ? forceTestCondition.Value : 
            AchieviementSystem.IsUnlockAchiv(achiv.ID, false, allAchiviements);

        image.sprite = achivInfo.GetSprite(contains);
        textInfo.text = achiv.InfoTxt;
    }
}
