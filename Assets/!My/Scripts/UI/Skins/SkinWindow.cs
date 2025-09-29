using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SkinWindow : WindowUI
{
    [SerializeField] private SkinButton skinVisualPref;
    private List<SkinButton> skinVisuals = new List<SkinButton>();
    private List<SkinButton.SkinInfo> skinFirstView = new List<SkinButton.SkinInfo>();

    [SerializeField] private Transform container;

    [Space]
    [SerializeField] private AchivInfoWindowUI InfoWindowUI;
    [SerializeField] private AudioSource source;

    public override void Open()
    {
        base.Open();

        UpdateSkinContainer();
        FirstViewAll();
    }

    private void UpdateSkinContainer()
    {
        ClearSkinVisuals();

        foreach (var achiviement in SkinsSystem.AllAchiviements)
        {
            if (achiviement.CheckUnlock(true))
            {
                SkinsSystem.UnlockSkin(achiviement.Info.ID);
            }
        }
        AchivInfo[] achivInfos = AchieviementSystem.GetAllAchivInfo(SkinsSystem.AllAchiviements);
        StartCoroutine(UpdateSkinContainerCoroutine(achivInfos));
    }

    private IEnumerator UpdateSkinContainerCoroutine (AchivInfo[] achivInfos)
    {
        foreach (var achiv in achivInfos)
        {
            var skinVisual = Instantiate(skinVisualPref, container);

            skinVisual.Init(achiv);
            skinVisual.OnClickSelectOpen += SelectSkin;
            skinVisual.OnClickInfoOpen += OpenSkinInfoWindow;
            skinVisuals.Add(skinVisual);

            if ((SkinsSystem.IsSkinUnlock(achiv.ID) || AchieviementSystem.IsUnlockAchiv(achiv.ID, true, SkinsSystem.AllAchiviements))
                && IsFirstView(achiv.ID))
            {
                AddListFirstView(skinVisual.CurrentSkinInfo);
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void AddListFirstView(SkinButton.SkinInfo skinInfo)
    {
        skinFirstView.Add(skinInfo);
    }

    private bool IsFirstView(string id)
    {
        return PlayerPrefs.GetInt($"SkinWindowFirstView_{id}", 0) == 0;
    }

    private void FirstViewAll()
    {
        StartCoroutine(FirstViewAllCotoutine());
    }

    private IEnumerator FirstViewAllCotoutine()
    {
        Debug.Log($"FirstViewAllCotoutine skinFirstView.Count {skinFirstView.Count}");
        for (int i = 0; i < skinFirstView.Count; i++)
        {
            Debug.Log($"FirstViewAllCotoutine {skinFirstView[i].id}");
            yield return StartCoroutine(FirstViewCotoutine(skinFirstView[i]));
            yield return new WaitForEndOfFrame();
        }
        skinFirstView.Clear();
    }

    private IEnumerator FirstViewCotoutine(SkinButton.SkinInfo skinInfo)
    {
        PlayerPrefs.SetInt($"SkinWindowFirstView_{skinInfo.id}", 1);
        OpenSkinInfoWindow(skinInfo);
        source.Play();
        yield return new WaitWhile(() => InfoWindowUI.IsOpen);
        yield break;
    }

    private void SelectSkin(SkinButton.SkinInfo skinInfo)
    {
        foreach (var skinButton in skinVisuals)
        {
            skinButton.UpdateSelect(skinButton.CurrentSkinInfo.id == skinInfo.id);
        }
        SkinsSystem.SelectSkin(skinInfo.id);
    }

    private void ClearSkinVisuals()
    {
        skinVisuals.ForEach((x) => Destroy(x.gameObject));
        skinVisuals.Clear();
    }

    public void OpenSkinInfoWindow(SkinButton.SkinInfo skinInfo)
    {
        foreach (var achiviement in SkinsSystem.AllAchiviements)
        {
            if (achiviement.Info.ID == skinInfo.id)
            {
                InfoWindowUI.Init(achiviement.Info, SkinsSystem.AllAchiviements, SkinsSystem.IsSkinUnlock(achiviement.Info.ID));
                InfoWindowUI.Open();
                return;
            }
        }
    }
}
