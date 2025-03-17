using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] AudioClip menuMusic;
    [SerializeField] string gameplaySceneName = "Gameplay";

    void Start()
    {
        AudioManager.SetMusic(menuMusic);
    }
    public void Play(int levelIndex)
    {
        LevelManager.currentlevelIndex = levelIndex;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
