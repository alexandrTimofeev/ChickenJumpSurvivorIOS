using UnityEngine;
using System;

[Serializable]
public abstract class AchievementBehaviour
{
    public enum AchivProgressType { Boolean, Int, Float };

    [Header("Base Info")]
    public string IDBehaviour;
    public AchivProgressType ProgressType;
    public float EndValue;

    /// Условие успеха — достижение выполнено
    public abstract bool IsConditionSuccess { get; }

    /// Уже получено?
    public bool IsUnlocked => PlayerPrefs.GetInt($"AchievementUnlocked_{IDBehaviour}", 0) == 1;

    /// Очистить (сбросить) состояние достижения
    public void Clear()
    {
        PlayerPrefs.SetInt($"AchievementUnlocked_{IDBehaviour}", 0);
        PlayerPrefs.SetFloat($"AchievementValue_{IDBehaviour}", 0f);
        PlayerPrefs.Save();
    }

    /// Получить текущее значение прогресса
    public float GetValue() => PlayerPrefs.GetFloat($"AchievementValue_{IDBehaviour}", 0f);

    /// Установить новое значение
    public void SetValue(float value)
    {
        PlayerPrefs.SetFloat($"AchievementValue_{IDBehaviour}", value);
        PlayerPrefs.Save();
        CheckUnlock();
    }

    /// Добавить к значению (инкремент)
    public void AddValue(float add) => SetValue(GetValue() + add);

    /// Попытаться выдать достижение
    public bool CheckUnlock(bool isTestCondition = false)
    {
        if (!IsUnlocked && (isTestCondition == false || IsConditionSuccess))
        {
            ForceUnlock();
            return true;
        }
        return IsUnlocked;
    }

    public void ForceUnlock()
    {
        if (IsUnlocked)
            return;

        PlayerPrefs.SetInt($"AchievementUnlocked_{IDBehaviour}", 1);
        PlayerPrefs.Save();
    }
}

public class SimpleAchievementBehaviour : AchievementBehaviour
{
    public override bool IsConditionSuccess => IsUnlocked;
}