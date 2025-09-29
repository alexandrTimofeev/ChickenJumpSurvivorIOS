using System.Collections;
using UnityEngine;

public class GameEntryOnceChR : GameEntryOnce
{
    public override void Init()
    {
        base.Init();

        WindowUI.OnClickRestart += GameSceneManager.LoadGame;
        WindowUI.OnClickMenu += GameSceneManager.LoadMenu;
        WindowUI.OnClickNextLvl += GameSceneManager.LoadGameNextLvl;

        GameSceneManager.OnGameLoad += () => GamePause.SetPause(false);
        GameSceneManager.OnMenuLoad += () => GamePause.SetPause(false);

        //GameEntryGameplayChGun.DataLevel = null;
    }
}