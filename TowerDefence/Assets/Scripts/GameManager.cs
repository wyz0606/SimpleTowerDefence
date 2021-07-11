using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region [Variables]
    public static GameManager Instance;
    [SerializeField] private GameObject m_home;
    [SerializeField] private GameObject m_gameOverPage;
    [SerializeField] private GameObject m_gameSpawners;
    public List<PoolManager> m_pools;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        m_gameSpawners.SetActive(false);
        m_gameOverPage.SetActive(false);
    }

    #region [GameStart]
    /// <summary>
    /// When the game starts, enable the home and the spawners
    /// Disable the game over canvas
    /// </summary>
    public void GameStart()
    {
        m_home.SetActive(true);
        m_gameOverPage.SetActive(false);
        m_gameSpawners.SetActive(true);
    }
    #endregion

    #region [GameOver]
    /// <summary>
    /// When the game is over, enable the game over canvas
    /// Disable the spawners and reset the PoolManagers
    /// </summary>
    public void GameOver()
    {
        m_gameOverPage.SetActive(true);
        m_gameSpawners.SetActive(false);

        foreach (var pool in m_pools)
        {
            pool.ResetPool();
        }
    }
    #endregion
}
