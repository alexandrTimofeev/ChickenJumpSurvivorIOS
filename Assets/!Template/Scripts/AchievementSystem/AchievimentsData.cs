using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievimentsData", menuName = "SGames/Achieviments")]
public class AchievimentsData : ScriptableObject
{
    private static AchievimentsData achivFromResource;
    public static AchievimentsData AchivFromResource
    {
        get
        {
            if (achivFromResource == null)
                achivFromResource = Resources.Load<AchievimentsData>("AchievimentsData");
            return achivFromResource;
        }
    }

    [SerializeField] private Achiviement[] allAchiviements = new Achiviement[0];
    public Achiviement[] AllAchiviements => allAchiviements;
}


[System.Serializable]
public class AchivInfo
{
    public string ID;
    public string Title;
    public string InfoTxt;
    public Sprite spriteYes;
    public Sprite spriteNo;

    public Sprite GetSprite(bool contains)
    {
        return contains ? spriteYes : spriteNo;
    }
}

[System.Serializable]
public class Achiviement
{
    public AchivInfo Info;
    [SerializeReference] private AchievementBehaviour achievementBehaviour;

    public bool CheckUnlock(bool isTestCondition = false)
    {
        return achievementBehaviour == null ? true : achievementBehaviour.CheckUnlock(isTestCondition);
    }

    public void ForceUnlock ()
    {
        if (achievementBehaviour == null)
            return;
        achievementBehaviour.ForceUnlock();
    }

    public bool IsUnlocked => achievementBehaviour == null ? true : achievementBehaviour.IsUnlocked;
}