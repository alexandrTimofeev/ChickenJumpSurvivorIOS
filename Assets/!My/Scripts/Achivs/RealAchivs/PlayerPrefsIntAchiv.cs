using UnityEngine;

public class PlayerPrefsIntAchiv : AchievementBehaviour
{
    [SerializeField] private string prefsTitle;
    [SerializeField] private int prefsDefult;

    [Space]
    [SerializeField] private CompereType compereType = CompereType.Equal;
    [SerializeField] private int prefsTarget;

    public override bool IsConditionSuccess => CompareProcess.TestCompare(PlayerPrefs.GetInt(prefsTitle, prefsDefult), prefsTarget, compereType);
}
