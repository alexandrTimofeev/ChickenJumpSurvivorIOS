using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class GameEntryPoint
{
    private static readonly List<object> _onceInstances = new();
    private static readonly List<object> _gameplayInstances = new();
    private static readonly List<object> _menuInstances = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void FirstInit()
    {
        GameSceneManager.Init();
        Register<GameEntryOnce>(action => action?.Invoke(), _onceInstances);
        Register<GameEntryGameplay>(action => GameSceneManager.OnGameLoad += action, _gameplayInstances);
        Register<GameEntryMenu>(action => GameSceneManager.OnMenuLoad += action, _menuInstances);

        GameSceneManager.OnGameLoad += () => Time.timeScale = 1f;
        GameSceneManager.OnMenuLoad += () => Time.timeScale = 1f;
    }

    private static void Register<TBase>(Action<Action> subscribeToEvent, List<object> instanceList)
    {
        var baseType = typeof(TBase);

        var types = baseType.Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract);

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            instanceList.Add(instance);

            var initMethod = type.GetMethod("Init", BindingFlags.Public | BindingFlags.Instance);
            if (initMethod != null)
            {
                Action action = () => initMethod.Invoke(instance, null);
                subscribeToEvent(action);
            }
        }

        // Статический Init базового класса
        var baseInit = baseType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
        if (baseInit != null)
        {
            Action baseAction = () => baseInit.Invoke(null, null);
            subscribeToEvent(baseAction);
        }
    }
}

public abstract class GameEntryGameplay
{
    public virtual void Init()
    {
        Debug.Log("Base GameEntryGameplay Init");
    }
}

public abstract class GameEntryMenu
{
    public virtual void Init()
    {
        Debug.Log("Base GameEntryMenu Init");
    }
}

public abstract class GameEntryOnce
{
    public virtual void Init()
    {
        Debug.Log("Base GameEntryOnce Init");
    }
}