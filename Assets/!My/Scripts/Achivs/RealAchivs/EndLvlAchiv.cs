using UnityEngine;

public class EndLvlAchiv : AchievementBehaviour
{
    public int level;
    public string LastLvlPrefsName = "LastLvl";
    public override bool IsConditionSuccess {
        get
        {
            return PlayerPrefs.GetInt(LastLvlPrefsName, 0) > level;
        }
    }
}
