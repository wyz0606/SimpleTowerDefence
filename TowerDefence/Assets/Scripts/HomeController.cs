using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    #region [Variables]
    [SerializeField] private HealthBar m_healthBar;
    [SerializeField] private float m_maxHealth;
    [SerializeField] private float m_cureSpeed;

    private float m_currentHealth;
    #endregion

    #region [OnEnable, InitializeHome]
    private void OnEnable()
    {
        InitializeHome();
    }

    public void InitializeHome()
    {
        m_currentHealth = m_maxHealth;
        m_healthBar.SetHealthMax(m_maxHealth);
    }
    #endregion

    #region [DamageHome]
    public void DamageHome(float damage)
    {
        m_currentHealth -= damage;
        m_healthBar.SetHealth(m_currentHealth);
        if (m_currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region [Update: Self cure]
    // Update is called once per frame
    void Update()
    {
        if (m_currentHealth < m_maxHealth)
        {
            m_currentHealth += m_cureSpeed * Time.deltaTime;
            m_healthBar.SetHealth(m_currentHealth);
        }
    }
    #endregion
}
