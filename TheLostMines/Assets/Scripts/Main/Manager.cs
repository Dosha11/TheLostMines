using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    [SerializeField] private Slider _levelSlider;
    [SerializeField] private Slider _levelSliderInventori;
    [SerializeField] private Slider _thristSlider;
    [SerializeField] private Text _coinText;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _levelTextProgress;
    [SerializeField] private GameObject _winWindow;

    public Button IteractionButton;
    public bool Niht;
 
    private int _coin = 400;
    private int _level = 1;
    private float _levelProgress = 0;
    private int _targetLevelProgress = 100;
    private int _thrist = 16;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(Thrist());
    }

    public void NullLevel()
    {
        _coin = 400;
        _level = 1;
        _levelProgress = 0;
        _targetLevelProgress = 100;
        _thrist = 16;
    }

    public void Loading(int level, float levelProgress, int targetLevelProgress, int thrist, int coin)
    {
        _level = level;
        _levelText.text = _level.ToString();
        _levelProgress = levelProgress;
        _levelSlider.value = _levelProgress;
        _levelSliderInventori.value = _levelProgress;
        _targetLevelProgress = targetLevelProgress;
        _levelSlider.maxValue = _targetLevelProgress;
        _levelSliderInventori.maxValue = _targetLevelProgress;
        _thrist = thrist;
        _thristSlider.value = _thrist;
        _coin = coin;
        _coinText.text = _coin.ToString();
    }

    public IEnumerator Thrist()
    {
        while (true)
        {
            if(_thrist > 0)
            {
                yield return new WaitForSeconds(30);
                _thrist--;
                _thristSlider.value = _thrist;
            }
        }
    }

    public void LoadGame()
    {
        Save.Instance.Load();
        GameManager.Instance.SceneLoader(1);
    }

    public void SaveGame()
    {
        Save.Instance.Saves();
    }

    public void AddWhater()
    {
        _thrist += 4;
        _thristSlider.value = _thrist;
    }

    public void AddLevel(float count)
    {
        _levelProgress += count;
        if(_levelProgress >= _targetLevelProgress)
        {
            _level++;
            CaveManager.Instance.CheckStatus();
            _levelText.text = "уровень " + _level.ToString();
            _levelProgress -= _targetLevelProgress;
            _targetLevelProgress += (_targetLevelProgress / 100) * 175;
            _levelSlider.maxValue = _targetLevelProgress;
            _levelSliderInventori.maxValue = _targetLevelProgress;
        }
        _levelTextProgress.text = _levelProgress.ToString() + "/" + _targetLevelProgress.ToString();
        _levelSlider.value = _levelProgress;
        _levelSliderInventori.value = _levelProgress;
    }

    public void ChangeCoin(int count)
    {
        _coin += count;
        _coinText.text = "P " + _coin.ToString();
        if(_coin >= 100000 && _level >= 4)
        {
            _winWindow.SetActive(true);
        }
    }

    public void OnIteractionButton(ItemType type)
    {
        IteractionButton.GetComponent<Image>().sprite = Resources.Load<Sprite>(type.ToString());
        IteractionButton.gameObject.SetActive(true);
    }

    public void CloseIteractionButton()
    {
        IteractionButton.onClick.RemoveAllListeners();
        IteractionButton.gameObject.SetActive(false);
    }

    public int GetCoin()
    {
        return _coin;
    }

    public int GetLevel()
    {
        return _level;
    }

    public float GetLevelProgress()
    {
        return _levelProgress;
    }

    public int GetTargetLevelProgress()
    {
        return _targetLevelProgress;
    }

    public int GetThrist()
    {
        return _thrist;
    }
}
