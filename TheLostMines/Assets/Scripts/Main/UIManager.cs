using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _plotsWindow;
    [SerializeField] private GameObject _window;

    private int _countPlots = 0;

    public void ChengePlot()
    {
        if (_countPlots < _plotsWindow.Count - 1)
        {
            _countPlots++;
            _plotsWindow[_countPlots].SetActive(true);
        }
        else
        {
            _countPlots = 0;
            for (int i = 1; i < _plotsWindow.Count; i++)
            {
                _plotsWindow[i].SetActive(false);
            }
            _window.SetActive(false);
        }
    }

    public void Loader(int scene)
    {
        GameManager.Instance.SceneLoader(scene);
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
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
}
