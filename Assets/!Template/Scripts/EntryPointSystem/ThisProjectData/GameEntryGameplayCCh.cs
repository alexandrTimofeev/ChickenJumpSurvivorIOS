using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameEntryGameplayCCh : GameEntryGameplay
{
    private IInput input;
    private GlobalMover globalMover;
    private Spawner spawner;
    private GrappableObjectMediator objectMediator;
    private GameStateManager stateManager;
    private ScoreSystem scoreSystem;
    private SpawnManager spawnManager;
    private ResourceSystem resourceSystem;
    private GameSessionManagerCCh gameManager;

    public static GameSessionDataContainer DataContainer { get; private set; }

    private PlayerChCF player;
    private TimerStarter timerStarter;
    private GameTimer gameTimer;

    public static LevelData DataLevel;
    public static AchiviementMediator achiviementMediator;

    public override void Init()
    {
        //base.Init();
        Debug.Log("GameEntryGameplayCCh Init");

        InitializeReferences();
        InitializeLevel();
        InitializeInterface();
        InitializeAudio();
        SetupGameManager();
        SetupScoreSystem();
        SetupSpawnerAndProgressBar();
        SetupInterfaceEvents();
        SetupHealthContainer();
        SetupSpawnManager();
        SetupGameStateEvents();
        SetupPlayer();
        SetupWinLoseCondition();
        SetupBonuses();
        SetupAchivmients();
        SetupSkins();

        stateManager.SetState(GameState.None);
    }

    private void InitializeReferences()
    {
        input = InputFabric.GetOrCreateInpit();
        globalMover = Object.FindObjectOfType<GlobalMover>();
        spawner = Object.FindObjectOfType<Spawner>();

        objectMediator = new GrappableObjectMediator();
        achiviementMediator = new AchiviementMediator();
        GrapCollider.Mediator = objectMediator;
        stateManager = new GameStateManager();
        scoreSystem = new ScoreSystem();
        spawnManager = new SpawnManager();
        resourceSystem = new ResourceSystem();
        gameManager = (new GameObject("GameManager")).AddComponent<GameSessionManagerCCh>();

        DataContainer = Resources.Load<GameSessionDataContainer>("GameSessionDataContainer").Clone();
        if (DataLevel == null)
            DataLevel = Resources.Load<LevelData>($"Levels/{DataContainer.StandartLevelData}");

        timerStarter = (new GameObject()).AddComponent<TimerStarter>();
        if(timerStarter != null && DataLevel != null)
            timerStarter.Play(DataLevel.Timer);
        gameTimer = timerStarter.Timer;

        timerStarter.IsUnTimeScale = true;
    }

    private void InitializeLevel()
    {
        if (DataLevel == null)
            return;

        Transform levelTr = GameObject.Find("Level").transform;
        if (DataLevel.LevelPrefab)
            GameObject.Instantiate(DataLevel.LevelPrefab, levelTr);

        GameObject.Find("Background").GetComponent<SpriteRenderer>().sprite = DataLevel.Background;
    }

    private void InitializeInterface()
    {
        InterfaceManager.Init();
        InterfaceManager.BarMediator.ShowForID("Score", 0);
    }

    private void InitializeAudio()
    {
        AudioManager.Init();
        AudioManager.PlayMusic();

        DataContainer.OnChangeSpeedGame += (speed) => AudioManager.SetSpeedMusic(((speed - 1f) * 0.5f) + 1f);
    }

    private void SetupScoreSystem()
    {
        scoreSystem.OnScoreChange += (score, point) =>
        {
            InterfaceManager.BarMediator.ShowForID("Score", score);
        };

        //scoreSystem.OnAddScore += InterfaceManager.CreateScoreFlyingText;
        //scoreSystem.OnRemoveScore += InterfaceManager.CreateScoreFlyingText;
    }

    private void SetupSpawnerAndProgressBar()
    {
        if (spawner == null)
            return;

        spawner.OnInstructionProgress += (id, progress) =>
        {
            InterfaceManager.BarMediator.ShowForID("Progress", progress);
        };
    }

    private void SetupInterfaceEvents()
    {
        InterfaceManager.OnClose += (window) =>
        {
            if (window is PauseWindowUI)
                stateManager.BackState();
        };

        InterfaceManager.OnOpen += (window) =>
        {
            if (window is PauseWindowUI)
                stateManager.SetState(GameState.Pause);
        };

        if(gameTimer != null)
            gameTimer.OnTick += (s) => InterfaceManager.BarMediator.ShowForID("Timer", s);
    }

    private void SetupHealthContainer()
    {
        DataContainer.HealthContainer.OnChangeValue += (life) => InterfaceManager.BarMediator.ShowForID("Life", life);
        DataContainer.HealthContainer.OnChangeValue += (life) => InterfaceManager.BarMediator.SetMaxForID("Life", DataContainer.HealthContainer.ClampRange.y);
        DataContainer.HealthContainer.UpdateValue();
    }

    private void SetupSpawnManager()
    {
        var settings = Resources.Load<SpawnerSettings>("Spawn/SpawnerSettings");
        spawnManager.Init(spawner, settings);

        if(spawner)
            spawnManager.OnChangeSpeed += spawner.SetSpeed;
        spawnManager.OnChangeSpeed += (speed) => DataContainer.SpeedGame = speed;
        if(globalMover)
            spawnManager.OnChangeSpeed += globalMover.SetSpeedCoef;
    }

    private void SetupGameStateEvents()
    {
        TutorialWindowUI window = InterfaceManager.CreateAndShowWindow<TutorialWindowUI>();
        window.OnClose += (win) => stateManager.SetState(GameState.Game);
        window.Init(input);

        stateManager.OnWin += () =>
        {
            RecordData recordData = LeaderBoard.GetScore($"score_{LevelSelectWindow.CurrentLvl}");
            InterfaceManager.ShowWinWindow(scoreSystem.Score, (recordData != null ? recordData.score : 0));
            LeaderBoard.SaveScore($"score_{LevelSelectWindow.CurrentLvl}", scoreSystem.Score);
            LevelSelectWindow.CompliteLvl();
        };

        stateManager.OnLose += () =>
        {
            //InterfaceManager.ShowLoseWindow(scoreSystem.Score, LeaderBoard.GetBestScore());
            //LeaderBoard.SaveScore($"default", scoreSystem.Score);
            //LevelSelectWindow.CompliteLvl();
            RecordData recordData = LeaderBoard.GetScore($"score_{LevelSelectWindow.CurrentLvl}");
           InterfaceManager.ShowLoseWindow(scoreSystem.Score, recordData != null ? recordData.score : 0);
        };

        stateManager.OnStateChange += (state) =>
        {
            GamePause.SetPause(state != GameState.Game);
            AudioManager.PassFilterMusic(state != GameState.Game);            

            if (state == GameState.Win || state == GameState.Lose)
            {
                player.gameObject.SetActive(false);
                AudioManager.StopMusic();
                if(gameTimer != null)
                    gameTimer.Stop();
            }
        };
    }

    private void SetupPlayer()
    {
        player = Object.FindObjectOfType<PlayerChCF>();
        player.Init(input);
        player.OnDamage += (dmgCont) =>
        {
            DataContainer.HealthContainer.RemoveValue(1);
        };

        player.OnGroundUpdate += (plane) =>
        {
            scoreSystem.AddScore(DataContainer.AnGroundBonus, player.transform.position + (Vector3.down * 1f));
            InterfaceManager.CreateScoreFlyingText(DataContainer.AnGroundBonus, player.transform.position + (Vector3.down * 1f), 0.05f);
        };
        //DataContainer.OnChangeSpeedGame += (speed) => player.SetSpeedKof(speed);
    }

    private void SetupWinLoseCondition()
    {
        if(gameTimer != null)
            gameTimer.OnComplete += () => stateManager.SetState(GameState.Win);

        DataContainer.HealthContainer.OnDownfullValue += (_) =>
        {
            stateManager.SetState(GameState.Lose);
        };
    }

    private void SetupBonuses()
    {
        objectMediator.Subscribe<AddScoreGrapAction>((beh, grapOb) =>
        {
            scoreSystem.AddScore(beh.AddScore);
            InterfaceManager.CreateScoreFlyingText(beh.AddScore, grapOb.transform.position, 0.005f);
        });

        objectMediator.Subscribe<AddLifeGrapAction>((beh, grapOb) =>
        {
            DataContainer.HealthContainer.AddValue(beh.AddLife);
        });

        objectMediator.Subscribe<SlowMotionGrapAction>((beh, grapOb) =>
        {
            player.SlowMotion(beh.Duration);
        });

        objectMediator.Subscribe<InvictibleGrapAction>((beh, grapOb) =>
        {
            player.Invictible(beh.Duration);
        });
    }

    private void SetupGameManager()
    {
        gameManager.Init();
        gameManager.OnPhaseChange += (phase) =>
        {
            spawnManager.NextPhase();
        };
    }

    private void SetupAchivmients()
    {
        achiviementMediator.AddAchiviementForEndLevel<GameObject>("Grounds", (grounds) =>
        {
            if (grounds == null || grounds.Count > 0)
            {
                if (grounds.Count >= 6)
                    AchieviementSystem.ForceUnlock("UseAllPlatform");
            }
            else
            {
                AchieviementSystem.ForceUnlock("NotUsePlatform");
            }
        });

        player.OnGroundUpdate += (go) =>
        {
            achiviementMediator.AddInList("Grounds", go, false);
        };

        achiviementMediator.AddAchiviementForEndLevel("NotGrapEggs", true, (isNotGrap) =>
        {
            if (isNotGrap)
                AchieviementSystem.ForceUnlock("NotGrapEggs");
        });
        player.OnGrap += (egg) =>
        {
            achiviementMediator.ChangeStateAchiviementForEndLevel("NotGrapEggs", false);
        };

        achiviementMediator.AddAchiviementForEndLevel("GrapAllEggs", true, (isGrapAll) =>
        {
            if (isGrapAll)
                AchieviementSystem.ForceUnlock("GrapAllEggs");
        });
        spawner.OnObjectTimerDestroy += (spOb) =>
        {
            if(spOb.GetComponent<GrapObject>().IsActive)
                achiviementMediator.ChangeStateAchiviementForEndLevel("GrapAllEggs", false);
        };

        stateManager.OnWin += () =>
        {
            achiviementMediator.InvokeEndLevel();
        };
    }
    private void SetupSkins()
    {
        player.SetSkin(SkinsSystem.GetCurrentSkin());

        achiviementMediator.AddAchiviementForEndLevel("NoDamage", true, (isNoDamage) =>
        {
            if (isNoDamage)
            {
                SkinsSystem.UnlockSkin("NoDamage");
                if (DataLevel.ID == "Last")
                    SkinsSystem.UnlockSkin("NoDamageLast");
            }            
        });
        player.OnDamage += (egg) =>
        {
            achiviementMediator.ChangeStateAchiviementForEndLevel("NoDamage", false);
        };
    }
}