using UnityEngine;

public class GameEntryMenuCCh : GameEntryMenu
{
    public override void Init()
    {
        base.Init();
        Debug.Log("GameEntryMenuChR Init");

        InterfaceManager.Init();
        InterfaceManager.OnClickCommand += (cmd) =>
        {
            switch (cmd)
            {
                case InterfaceComand.PlayGame:
                    GameSceneManager.LoadGame();
                    break;
                default:
                    break;
            }
        };

        InterfaceManager.BarMediator.ShowForID("Best", LeaderBoard.GetBestScore());
    }
}