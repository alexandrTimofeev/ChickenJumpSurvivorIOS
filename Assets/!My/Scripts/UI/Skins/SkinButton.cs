using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkinButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI txtInfo;
    [SerializeField] private Button buttonSelect;

    [Space]
    [SerializeField] private Sprite selectSprite;
    [SerializeField] private Sprite unselectSprite;

    private bool isSelected;
    private bool isUnlock;

    private SkinInfo skinInfo;

    public SkinInfo CurrentSkinInfo => skinInfo;
    public event Action<SkinInfo> OnClickSelectOpen;
    public event Action<SkinInfo> OnClickInfoOpen;

    public void Init(AchivInfo achivInfo)
    {
        Init(achivInfo.ID, achivInfo.InfoTxt, achivInfo.spriteYes, achivInfo.spriteNo);
    }

    public void Init(string id, string info, Sprite spriteYes, Sprite spriteNo)
    {
        isSelected = PlayerPrefs.GetString("SkinSelect", "none") == id;
        isUnlock = SkinsSystem.IsSkinUnlock(id);

        skinInfo = new SkinInfo()
        {
            id = id,
            info = info,
            spriteYes = spriteYes,
            spriteNo = spriteNo,
            isSelected = isSelected,
            isUnlock = isUnlock
        };

        UpdateSelectView();
        txtInfo.text = info;
    }

    public void UpdateSelect(bool isSelect)
    {
        UpdateSelect(isUnlock, isSelect);
    }

    public void UpdateSelect (bool isUnlock, bool isSelect)
    {
        this.isSelected = isSelect; 
        this.isUnlock = isUnlock;
        PlayerPrefs.SetInt($"SkinUnlock_{skinInfo.id}", isUnlock ? 1 : 0);
        UpdateSelectView();
    }

    private void UpdateSelectView(bool? currentIsUnlock = null, bool? currentIsSelect = null)
    {
        bool isSelectedLocal = currentIsSelect == null ? isSelected : currentIsSelect.Value; 
        bool isUnlockLocal = currentIsUnlock == null ? isUnlock : currentIsUnlock.Value;

        image.sprite = isUnlockLocal ? skinInfo.spriteYes : skinInfo.spriteNo;
        buttonSelect.gameObject.SetActive(isUnlockLocal);
        //buttonSelect.image.sprite = isSelectedLocal ? selectSprite : unselectSprite;
        buttonSelect.interactable = !isSelectedLocal;
    }

    public void ClickSelect ()
    {
        OnClickSelectOpen?.Invoke(skinInfo);
    }

    public void OpenViewInfo()
    {
        OnClickInfoOpen?.Invoke(skinInfo);
    }

    [System.Serializable]
    public class SkinInfo
    {
        public string id;
        public string info;
        public Sprite spriteYes;
        public Sprite spriteNo;
        public bool isSelected;
        public bool isUnlock;
    }
}
