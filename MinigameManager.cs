using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] Transform MinigameParent;
    [SerializeField] GameObject currentMinigame;

    float minigameInterval;
    bool allowRepeats;
    float timeLastSpawned;
    List<GameObject> visitedMinigames = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(awaitLevelSpawn());
    }

    IEnumerator awaitLevelSpawn()
    {
        yield return new WaitUntil(LevelManager.isLevelSpawned);
        SpawnMinigame();
    }

    List<GameObject> GetMinigames()
    {
        try
        {
            minigameInterval = LevelManager.inst.currentLevel.minigameInterval;
            allowRepeats = LevelManager.inst.currentLevel.allowMinigamesRepeats;
            return LevelManager.inst.currentLevel.minigames;
        }
        catch
        {
            minigameInterval = 1;
            allowRepeats = false;
            return new List<GameObject>();
        }

    }

    void SpawnMinigame()
    {
        //destroy existing minigame
        if(currentMinigame != null)
        {
            Destroy(currentMinigame);
        }

        //retrieve list of minigames
        List<GameObject> minigames = GetMinigames();
        if(minigames == null) return;

        //remove visited minigames
        if(visitedMinigames.Count > 0) 
        {
            minigames = minigames.Except(visitedMinigames).ToList();
        }
        //randomly select a game from the available games
        GameObject selectedGame;
        Debug.Log($"selecting minigame from {minigames.Count} choices");
        selectedGame = minigames[Random.Range(0,minigames.Count)];
   
        //clear visited list when all games are
        Debug.Log($"{visitedMinigames.Count} games visited");
        if(visitedMinigames.Count == GetMinigames().Count-1)
        {
            visitedMinigames.Clear();
            Debug.Log($"all games visited, clearing list");
        }
        if(!allowRepeats && GetMinigames().Count > 1)
        {
            visitedMinigames.Add(selectedGame);
        }
        //spawn the mini game
        currentMinigame = Instantiate(selectedGame, MinigameParent);
        timeLastSpawned = Time.time;
        Debug.Log(minigameInterval);

        Invoke("SpawnMinigame",minigameInterval);
    }
}
