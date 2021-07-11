using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class TurrentSpawner : MonoBehaviour
{
    #region [Variables]
    [SerializeField] private PoolManager m_turrentPool;
    [SerializeField] private Camera m_cam;
    [SerializeField] private LayerMask m_targetLayerMask;
    [SerializeField] private NavMeshSurface m_navMeshSurface;
    [SerializeField] private TextMeshProUGUI m_turrentText;
    [SerializeField] private int m_maxTurrents;

    private int remainingTurrents;
    #endregion

    #region [OnEnable]
    private void OnEnable()
    {
        remainingTurrents = m_maxTurrents;
        SetUI();
    }
    #endregion

    #region [Update]
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100, m_targetLayerMask))
            {
                if (hitInfo.collider.CompareTag("DefenceArea") && remainingTurrents > 0)
                {
                    GameObject turrent = m_turrentPool.GetPooledObject();
                    turrent.transform.position = hitInfo.point;
                    turrent.SetActive(true);

                    remainingTurrents -= 1;
                    SetUI();

                    m_navMeshSurface.BuildNavMesh();
                }

                if (hitInfo.collider.CompareTag("Turrent"))
                {
                    GameObject turrent = hitInfo.collider.transform.parent.gameObject;
                    turrent.SetActive(false);
                    remainingTurrents += 1;
                    SetUI();

                    m_navMeshSurface.BuildNavMesh();
                }
            }
        }
    }
    #endregion

    #region [SetUI]
    void SetUI()
    {
        m_turrentText.text = "Remaining Turrents: " + remainingTurrents;
    }
    #endregion
}
