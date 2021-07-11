using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    #region [Variables]
    [SerializeField] private NavMeshAgent m_agent;
    [SerializeField] float m_maxHealth;
    [SerializeField] float m_damage;
    [SerializeField] private HealthBar m_healthBar;

    private float m_currentHealth;
    private HomeController m_target;
    #endregion

    #region [InitializeEnemy]
    public void InitializeEnemy(HomeController target)
    {
        m_target = target;
        m_currentHealth = m_maxHealth;
        m_healthBar.SetHealthMax(m_maxHealth);
        m_agent.ResetPath();
        m_agent.SetDestination(target.transform.position);
    }
    #endregion

    #region [Update]
    private void Update()
    {
        //This is important because when the enemy is enabled, its remainingDistance is 0
        //We should avoid DamageTarget() and EnemyDie being called at this point
        if (m_agent.remainingDistance == 0)
            return;

        if (m_agent.remainingDistance < m_agent.stoppingDistance && m_agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            DamageTarget();
            EnemyDie();
        }
    }
    #endregion

    #region [Damage Enemy]
    private void EnemyDie()
    {
        gameObject.SetActive(false);
    }

    public void DamageEnemy(float damage)
    {
        m_currentHealth -= damage;
        m_healthBar.SetHealth(m_currentHealth);
        if (m_currentHealth < 0)
        {
            EnemyDie();
        }
    }
    #endregion

    #region [Damage Target]
    private void DamageTarget()
    {
        m_target.DamageHome(m_damage);
    }
    #endregion
}
