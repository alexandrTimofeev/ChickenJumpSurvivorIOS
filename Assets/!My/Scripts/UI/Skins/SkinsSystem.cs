using System;
using UnityEngine;

public static class SkinsSystem
{
    private static AchievimentsData achivFromResource;
    public static AchievimentsData AchivFromResource
    {
        get
        {
            if (achivFromResource == null)
                achivFromResource = Resources.Load<AchievimentsData>("AchievimentsDataSkins");
            return achivFromResource;
        }
    }

    private static Achiviement[] allAchiviements = null;
    public static Achiviement[] AllAchiviements
    {
        get
        {
            if (allAchiviements == null)
                allAchiviements = AchivFromResource.AllAchiviements;
            return allAchiviements;
        }
    }

    public static void SelectSkin(string id)
    {
        PlayerPrefs.SetString("SkinSelect", id);
    }

    public static void UnlockSkin(string id)
    {
        PlayerPrefs.SetInt($"SkinUnlock_{id}", 1);
    }

    public static bool IsSkinUnlock(string id)
    {
        return (id == "" || id == "none") || PlayerPrefs.GetInt($"SkinUnlock_{id}", 0) == 1;
    }

    public static bool IsSkinConditionUnlock (string id, bool testCondition = false)
    {
        if (id == "" || id == "none")
            return true;

        foreach (var achiviement in AllAchiviements)
        {
            if (achiviement.Info.ID == id && achiviement.CheckUnlock(testCondition))
            {
                return true;
            }
        }

        return false;
    }

    public static Sprite GetCurrentSkin()
    {
        string id = PlayerPrefs.GetString("SkinSelect", "none");
        foreach (var achiviement in AllAchiviements)
        {
            if (achiviement.Info.ID == id)
            {
                return achiviement.Info.spriteYes;
            }
        }
        return null;
    }
}
