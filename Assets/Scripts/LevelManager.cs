using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private List<GameObject> Levels;
    private void Awake()
    {
        Instance = this;
    }

    public int GetLevelID() => PlayerPrefs.GetInt("Level");
    private GameObject SetLevel()
    {
        var currentLevel = Levels[PlayerPrefs.GetInt("Level")];
        return currentLevel;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Level") < 2)
        {
            var level = SetLevel();
            Instantiate(level);
        }
        else
        {
            UIManager.Instance.InfoPanel.SetActive(true);
            PlayerPrefs.SetInt("Level",0);
        }
    }
}
