using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    #region [Variables]
    [SerializeField] private Slider m_healthSlider;
    [SerializeField] private Image m_Fill;
    [SerializeField] private Gradient m_healthColor;
    private Transform camTransform;
    private Transform m_transform;
    #endregion

    #region [Awake, LateUpdate]
    private void Awake()
    {
        camTransform = Camera.main.transform;
        m_transform = this.transform;
    }

    void LateUpdate()
    {
        m_transform.LookAt(m_transform.position + camTransform.forward);
    }
    #endregion

    #region [Health Controls]
    public void SetHealthMax(float maxHP)
    {
        m_healthSlider.maxValue = maxHP;
        SetHealth(maxHP);
    }

    public void SetHealth(float value)
    {
        m_healthSlider.value = value;
        m_Fill.color = m_healthColor.Evaluate(m_healthSlider.normalizedValue);
    }
    #endregion
}
