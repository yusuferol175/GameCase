using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] public GameObject StartPanel;
    [SerializeField] public GameObject LevelCompPanel;
    [SerializeField] public GameObject GamePanel;
    [SerializeField] public GameObject InfoPanel;
    [SerializeField] public TMP_Text LevelText;
    [SerializeField] public TMP_Text ComplitedlevelText;
    [SerializeField] public TMP_Text ShotInfoText;
    [SerializeField] public TMP_Text FpsText;
    [SerializeField] public TMP_Text CurrentLevelText;
    [SerializeField] public TMP_Text NextLevelText;
    [SerializeField] public TMP_Text PercentText;


    [SerializeField] private RectTransform _barRectTransform;

    private float _maxBarWidth = 612f;
    private Vector2 _initialPosition;

    public void UpdateBar(float percentage)
    {
        var currentBarWidth = _maxBarWidth * percentage / 100f;
        var newSize = new Vector2(currentBarWidth, _barRectTransform.sizeDelta.y);
        _barRectTransform.sizeDelta = newSize;
        _barRectTransform.anchoredPosition = _initialPosition + new Vector2(currentBarWidth / 2f, 0f);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void NextLevelButton()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        SceneManager.LoadScene("Game");
    }

    private void Awake()
    {
        _initialPosition = _barRectTransform.anchoredPosition;
        Instance = this;
    }
}