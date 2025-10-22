using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : Singleton<GameManager>
{
    protected override bool isGlobal => true;

    public PlayerController player;
    public bool canMoveLeft = true;
    public bool canMoveRight = true;
    public bool canJump = true;
    public LevelBase currentLevel;

    public Esc esc;

    public void OnSceneLoaded()
    {
        player = FindObjectOfType<PlayerController>();
        currentLevel = FindObjectOfType<LevelBase>();
        esc = FindObjectOfType<Esc>();
        currentLevel?.InitLevel();
    }

    public void LoadNextScene(Action onLoaded)
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadSceneAsync(nextIndex, onLoaded));
        }
    }

    private IEnumerator LoadSceneAsync(int buildIndex, Action onLoaded)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(buildIndex);
        while (!asyncOp.isDone)
        {
            yield return null;
        }
        onLoaded?.Invoke();
    }
}
