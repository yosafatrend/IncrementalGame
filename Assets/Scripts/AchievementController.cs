using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour
{
    //pemanggilan script bersifat manager
    private static AchievementController _instance = null;
    public static AchievementController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AchievementController>();
            }
            return _instance;
        }
    }

    [SerializeField] private Transform _popupTransform;
    [SerializeField] private Text _popupText;
    [SerializeField] private float _popUpShowDuration = 3f;
    [SerializeField] private List<AchievementData> _achievementList;

    private float _popUpShowDurationCounter;

    // Update is called once per frame
    void Update()
    {
        if (_popUpShowDurationCounter > 0)
        {
            //kurangi durasi ketika popup durasi lebih dari 0
            _popUpShowDurationCounter -= Time.unscaledDeltaTime;
            //Lerp adalah fungsi linear interpolation, digunakan untuk mengubah value secara perlahan
            _popupTransform.localScale = Vector3.LerpUnclamped(_popupTransform.localScale, Vector3.one, 0.5f);
        }
        else
        {
            _popupTransform.localScale = Vector2.LerpUnclamped(_popupTransform.localScale, Vector3.right, 0.5f);
        }
    }

    public void UnlockAchievement(AchievementType type, string value)
    {
        //Mencari data achievement
        AchievementData achievement = _achievementList.Find(a => a.Type == type && a.Value == value);
        if (achievement != null && !achievement.IsUnlocked)
        {
            achievement.IsUnlocked = true;
            ShowAchievementPopup(achievement);
        }
    }

    private void ShowAchievementPopup(AchievementData achievement)
    {
        _popupText.text = achievement.Title;
        _popUpShowDurationCounter = _popUpShowDuration;
        _popupTransform.localScale = Vector2.right;
    }
}

[System.Serializable]
public class AchievementData
{
    public string Title;
    public AchievementType Type;
    public string Value;
    public bool IsUnlocked;
}

public enum AchievementType
{
    UnlockResource
}
