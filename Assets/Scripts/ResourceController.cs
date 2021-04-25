using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class ResourceController : MonoBehaviour
{
    public Button ResourceButton;
    public Image ResourceImage;
    public Text ResourceDescription;
    public Text ResourceUpgradeCost;
    public Text ResourceUnlockCost;

    private ResourceConfig _config;

    private int _level = 1;

    public bool IsUnlocked { get; private set; }

    public void SetConfig(ResourceConfig config)
    {
        _config = config;

        ResourceDescription.text = $"{ _config.Name } Lv. { _level }\n+{ GetOutput().ToString("0") }";
        ResourceUnlockCost.text = $"Unlock Cost\n{_config.UnlockCost}";
        ResourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";

        SetUnlocked(_config.UnlockCost == 0);
    }

    private void SetUnlocked(bool unlocked)
    {
        IsUnlocked = unlocked;
        ResourceImage.color = IsUnlocked ? Color.white : Color.gray;
        ResourceUnlockCost.gameObject.SetActive(!unlocked);
        ResourceUpgradeCost.gameObject.SetActive(unlocked);
    }

    public double GetOutput()
    {
        return _config.Output * _level;
    }

    public double GetUpgradeCost()
    {
        return _config.UpgradeCost * _level;
    }

    public double GetUnlockCost()
    {
        return _config.UnlockCost;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResourceButton.onClick.AddListener(() =>
        {
            if (IsUnlocked)
            {
                UpgradeLevel();
            }
            else
            {
                UnlockResource();
            }
        });
    }

    private void UnlockResource()
    {
        double unlockCost = GetUnlockCost();
        if (GameManager.Instance.TotalGold < unlockCost)
        {
            return;
        }

        SetUnlocked(true);
        GameManager.Instance.ShowNextResources();
        AchievementController.instance.UnlockAchievement(AchievementType.UnlockResource, _config.Name);
    }

    public void UpgradeLevel()
    {
        double upgradeCost = GetUpgradeCost();
        if (GameManager.Instance.TotalGold < upgradeCost)
        {
            return;
        }

        GameManager.Instance.AddGold(-upgradeCost);
        _level++;

        ResourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
        ResourceDescription.text = $"{_config.Name} Lv.{_level}\n+{GetOutput().ToString("0")}";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
