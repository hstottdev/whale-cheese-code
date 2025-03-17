using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager inst;
    [SerializeField] AudioClip levelMusic;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] Transform mainGameParent;

    [SerializeField] GameObject grid;

    public Level currentLevel;

    public static int currentlevelIndex;

    [SerializeField] List<Level> levelPrefabs;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI levelNameText;
    [SerializeField] TextMeshProUGUI levelNumberText;

    [Header("Reset")]
    [SerializeField] GameObject poofParticle;
    [SerializeField] GameObject resettingCard;
    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
    }

    void Start()
    {
        AudioManager.SetMusic(levelMusic);
        SpawnLevel();
        SetLevelStartText();
    }

    // Update is called once per frame
    void Update()
    {
        if(ActionManager.inst.actionsRemaining <= 0)
        {
            ShowLoseScreen();
        }
    }

    public void ShowLoseScreen()
    {
        loseScreen.SetActive(true);
    }

    public void ToggleGrid()
    {
        grid.SetActive(!grid.activeInHierarchy);
    }

    public void RestartLevel()
    {
        GameObject player = PlayerController.inst.gameObject;
        player.SetActive(false);
        ActionManager.playActions.Clear();
        Instantiate(poofParticle,player.transform.position, Quaternion.identity);
        resettingCard.SetActive(true);
        AudioManager.PlaySound("popping1",Random.Range(0.8f,1.2f));
        AudioManager.SetMusic(null);
        Invoke("ReloadScene",2);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowWinScreen()
    {
        AudioManager.PlaySound("cat",0.6f);
        AudioManager.SetMusic(null);
        winScreen.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void NextLevel()
    {
        LevelManager.currentlevelIndex++;

        if(LevelManager.currentlevelIndex < LevelManager.inst.levelPrefabs.Count)
        {
            ReloadScene();
        }
        else
        {
            MainMenu();
        }


    }

    void SpawnLevel()
    {
        if(levelPrefabs.Count > 0)
        {
            GameObject spawnedLevel = Instantiate(levelPrefabs[currentlevelIndex].gameObject,mainGameParent);
            currentLevel = spawnedLevel.GetComponent<Level>();
        }
    }

    void SetLevelStartText()
    {
        if (levelNameText != null) 
        {
            levelNameText.text = currentLevel.levelName;
        }

        if(levelNumberText != null) 
        {
            levelNumberText.text = currentLevel.levelNumberName;
        }

    }

    public static bool isLevelSpawned()
    {
        if(inst == null)
        {
            return false;
        } 
        else if(inst.currentLevel == null)
        {
            return false;
        } 
        return true;
    }
}
