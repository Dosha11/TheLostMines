using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
         if(Instance == null) Instance = this;
         else Destroy(this);
    }

    public void SceneLoader(int scene)
    {
        if(scene < 2)
        {
            if(DontDestroyOblect.Instance != null)
            {
                Destroy(DontDestroyOblect.Instance.gameObject);
            }
        }
        else
        {
            Save.Instance.Saves();
        }
        SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        Debug.Log("f");
        Application.Quit();
    }

    public void OnPaus()
    {
        Time.timeScale = 0;
    }

    public void ClosePaus()
    {
        Time.timeScale = 1;
    }
}
