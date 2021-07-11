using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region [Variables]
    [SerializeField] private GameObject m_itemPrefab;
    [SerializeField] private int m_initialItems;
    private List<GameObject> m_itemList;
    #endregion

    #region [Awake, InitializePool]
    private void Awake()
    {
        m_itemList = new List<GameObject>();
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < m_initialItems; i++)
        {
            GameObject item = Instantiate(m_itemPrefab);
            item.SetActive(false);
            m_itemList.Add(item);
        }
    }
    #endregion

    #region [GetPooledObject]
    public GameObject GetPooledObject()
    {
        GameObject output = null;
        foreach (var item in m_itemList)
        {
            if (!item.activeInHierarchy)
            {
                output = item;
                break;
            }
        }

        if (output == null)
        {
            GameObject newItem = Instantiate(m_itemPrefab);
            newItem.SetActive(false);
            m_itemList.Add(newItem);
            output = newItem;
        }

        return output;
    }
    #endregion

    #region [ResetPool]
    public void ResetPool()
    {
        for (int i = m_itemList.Count - 1; i >= 0; i--)
        {
            if (i < m_initialItems)
            {
                m_itemList[i].SetActive(false);
            }
            else
            {
                Destroy(m_itemList[i]);
                m_itemList.RemoveAt(i);
            }
        }
    }
    #endregion
}
