using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectWindow : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private Sprite levelLockSprite;
    [SerializeField] private Sprite levelUnLockSprite;
    [SerializeField] private Sprite levelNextSprite;

    public static int CurrentLvl { get; private set; }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        int lastLvl = PlayerPrefs.GetInt("LastLvl", 0);
        for (int i = 0; i < buttons.Length; i++)
        {
            InitButton(buttons[i], i, lastLvl);
        }
    }

    private void InitButton(Button button, int i, int lastLvl)
    {
        int n = i;
        button.onClick.AddListener(() => LoadLevel(n));
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();

        Image image = button.GetComponent<Image>();
        if (i < lastLvl)
        {
            image.sprite = levelUnLockSprite;
            return;
        }
        else if (i == lastLvl)
        {
            image.sprite = levelNextSprite;
            return;
        }

        image.sprite = levelLockSprite;
        button.interactable = false;
    }

    public static void LoadLevel(int n)
    {
        CurrentLvl = n;
        GameEntryGameplayCCh.DataLevel = Resources.Load<LevelData>($"Levels/Level_{n}");
        GameSceneManager.LoadGame();
    }

    public void StartLastLevel()
    {
        int lastLvl = Mathf.Clamp(PlayerPrefs.GetInt("LastLvl", 0), 0, buttons.Length - 1);
        LoadLevel(lastLvl);
    }

    public static void CompliteLvl()
    {
        int lastLvl = PlayerPrefs.GetInt("LastLvl", 0);
        if (CurrentLvl == lastLvl)
            PlayerPrefs.SetInt("LastLvl", lastLvl + 1);
    }
}
