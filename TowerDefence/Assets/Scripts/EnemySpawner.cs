using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region [Variables]
    [SerializeField] private PoolManager m_enemyPool;
    [SerializeField] private HomeController m_target;
    [SerializeField] private float m_spawnDelay; //This is the delay between the waves of spawning
    public List<Transform> m_SpawnPos;
    #endregion

    #region [OnEnable, OnDisable]
    void OnEnable()
    {
        InvokeRepeating("SpawnEnemies", 1.0f, m_spawnDelay);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    #endregion

    #region [Spawn Enemies]
    private void SpawnEnemies()
    {
        foreach (var pos in m_SpawnPos)
        {
            GameObject enemy = m_enemyPool.GetPooledObject();

            enemy.transform.position = pos.position;
            enemy.SetActive(true);

            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.InitializeEnemy(m_target);
        }
    }
    #endregion
}
