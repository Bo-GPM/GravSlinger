using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public static LevelManager instance;
    [HideInInspector] public GameObject player;
    [HideInInspector] public List<GameObject> objectsToDestoryAfterDeath = new List<GameObject>();
    [HideInInspector] public PlayerController playerController;
    
    [Header("Debug")] 
    [SerializeField] private Text debugText;
    
    [Header("UI")] 
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text lifeText;
    [SerializeField] private Text HPText;
    [SerializeField] private Text CoinText;
    
    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int playerMaxHP;
    private int playerCurrentHP;
    [SerializeField] private int playerMaxLives;
    private int playerCurrentLives;
    private int playerCurrentCoins;


    [Header("CheckPoint")] 
    [SerializeField] private GameObject checkPointParent;
    private GameObject[] checkPointsArray;    //Call storeChildrenToArray() to initialize
    private int currentCheckpoint = 0;

    [Header("Enemy")] 
    [SerializeField] private GameObject enemySpawnerParent;
    private GameObject[] enemySpawnerArray;
        
    private void Awake()
    {
        instance = this;
        
    }

    void Start()
    {
        LevelInitialization();
    }

    // Update is called once per frame
    void Update()
    {
        DebugFunctions();
        UpdateUIText();
        DeathCheck();
    }

    private void DeathCheck()
    {
        if (playerCurrentHP <= 0)
        {
            if (playerCurrentLives <= 0)
            {
                // Reload scene to restart the game
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            }
            else
            {
                ResetGameToLastCheckPoint();
            }
        }
    }

    private void UpdateUIText()
    {
        lifeText.text = $"Life: {playerCurrentLives}";
        HPText.text = $"HP: {playerCurrentHP}";
        CoinText.text = $"Coins: {playerCurrentCoins}";
    }
    public void DebugFunctions()
    {
        string outputText = new string("");
        outputText += $"Debug Info: \n";
        outputText += $"Player Vel: {player.GetComponent<Rigidbody2D>().velocity} \n";
        outputText += $"Current CP: {currentCheckpoint} \n";
        outputText += $"Current Gold: {playerCurrentCoins}\n";
        
        debugText.text = outputText;
        
        // Some Hacks
        // Press R to get go back to last checkpoint
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGameToLastCheckPoint();
        }
        
        // Reload Scene (emulate perm death)
        if (Input.GetKeyDown(KeyCode.T))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            playerController.abilityAwakenIndex++;
        }
    }

    public void LevelInitialization()
    {
        // TODO: Start the game, initialize necessary things
        // 1. Import checkpoints to array, initialize coin, life, HP
        checkPointsArray = StoreAllChildrenToArray(checkPointParent);
        playerCurrentCoins = 1;
        playerCurrentLives = playerMaxLives;
        playerCurrentHP = playerMaxHP;
        AddCoins(0);
        UpdateHP(0);
        UpdateLife(0);
        
        // 2. Instantiate Player
        player = Instantiate(playerPrefab, checkPointsArray[currentCheckpoint].transform.position, quaternion.identity);
        playerController = player.GetComponent<PlayerController>();
        
        // 3. Spawn enemies
        enemySpawnerArray = StoreAllChildrenToArray(enemySpawnerParent);
        SpawnAllEnemies();
    }
    public void ResetGameToLastCheckPoint()
    {
        // TODO: revert game to the last checkpoint state
        // Use currentCheckpoint var
        player.transform.position = checkPointsArray[currentCheckpoint].transform.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        UpdateLife(-1);
        playerCurrentHP = playerMaxHP;
        UpdateHP(0);
        
        // Destroy all enemies and bullets
        foreach (GameObject tempObj in objectsToDestoryAfterDeath)
        {
            Destroy(tempObj);
        }
        objectsToDestoryAfterDeath.Clear();
        
        // Then respawn all of the enemies
        SpawnAllEnemies();
    }

    private void SpawnAllEnemies()
    {
        foreach (GameObject tempObj in enemySpawnerArray)
        {
            tempObj.GetComponent<EnemySpawner>().respawnObject();
        }
    }
    public void UpdateLife(int tempLife)
    {
        playerCurrentLives += tempLife;
    }
    
    public void UpdateHP(int tempHP)
    {
        playerCurrentHP += tempHP;
    }

    public void AddCoins(int tempCoin)
    {
        playerCurrentCoins += tempCoin;
        // Debug.LogError($"Coins: {playerCurrentCoins}");
        // Debug.LogWarning($"Coin Event is triggered, tempCoin is: {tempCoin}, current Coins: {playerCurrentCoins}");
    }
    
    // Checkpoint related
    public void ActivateCheckPoint(GameObject thisCheckPoint)
    {
        // 1. Find the index of this checkpoint
        int checkPointIndex = 0;
        for (int i = 0; i < checkPointsArray.Length; i++)
        {
            // Debug.Log($"this Checkpoint name: {thisCheckPoint.name}, checkpoint in array: {checkPointsArray[i].name}");
            if (thisCheckPoint.name == checkPointsArray[i].name)
            {
                checkPointIndex = i;
                // Debug.Log($"We Found the Index!, It's {i}");
            }
            // else
            // {
            //     Debug.LogError($"Checkpoint not found in array! Current array is {checkPointsArray}");
            // }
        }
        // 2. check if this is next checkpoint, if so, update checkpoint index
        if (checkPointIndex > currentCheckpoint)
        {
            currentCheckpoint = checkPointIndex;
        }
        
        // 3. Heal player
        playerCurrentHP = playerMaxHP;
        UpdateHP(0);
    }

    public GameObject[] StoreAllChildrenToArray(GameObject parentGameObject)
    {
        int childrenCount = parentGameObject.transform.childCount;
        GameObject[] tempArray = new GameObject[childrenCount];

        for (int i = 0; i < childrenCount; i++)
        {
            tempArray[i] = parentGameObject.transform.GetChild(i).gameObject;
        }

        // Debug.Log($"ChildrenCount: {childrenCount}");
        // foreach (GameObject child in tempArray)
        // {
        //     Debug.Log(child.name);
        // }

        return tempArray;
    }
}
