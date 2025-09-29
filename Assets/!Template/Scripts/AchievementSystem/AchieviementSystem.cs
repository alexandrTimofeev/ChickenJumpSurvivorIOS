using System;
using System.Collections;
using UnityEngine;
using System.Linq;

public static class AchieviementSystem
{
    private static Achiviement[] allAchiviements => AchievimentsData.AchivFromResource.AllAchiviements;

    public static bool IsUnlockAchiv (string id, bool testCondition = false, Achiviement[] allAchiviements = null)
    {
        if (allAchiviements == null)
            allAchiviements = AchieviementSystem.allAchiviements;

        return allAchiviements.Any((x) => x.Info.ID == id &&
        (x.IsUnlocked || (testCondition && x.CheckUnlock(true))));// || x.achievementBehaviour.IsConditionSuccess));
    }

    public static void ForceUnlock (string id)
    {
        foreach (var achiviement in allAchiviements)
        {
            if(achiviement.Info.ID == id)
            achiviement.ForceUnlock();
        }
    }

    public static AchivInfo[] GetAllAchivInfo()
    {
        AchivInfo[] achivInfos = new AchivInfo[allAchiviements.Length];
        for (int i = 0; i < achivInfos.Length; i++)
        {
            achivInfos[i] = allAchiviements[i].Info;
        }
        return achivInfos;
    }

    public static AchievimentsData LoadFromResource(string title) => Resources.Load<AchievimentsData>(title);
    public static Achiviement[] AllAchiviementsFromResource(string title) => LoadFromResource(title).AllAchiviements;

    public static AchivInfo[] GetAllAchivInfo(Achiviement[] achiviements)
    {
        AchivInfo[] achivInfos = new AchivInfo[achiviements.Length];
        for (int i = 0; i < achivInfos.Length; i++)
        {
            achivInfos[i] = achiviements[i].Info;
        }
        return achivInfos;
    }
}