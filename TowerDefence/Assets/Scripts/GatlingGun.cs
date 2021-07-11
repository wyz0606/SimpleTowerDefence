using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingGun : MonoBehaviour
{
    #region [Variables]
    [Header("Controls")]
    public Transform go_baseRotation;
    public Transform go_barrel;
    public Transform go_base;

    [Header("Gun settings")]
    public float barrelRotationSpeed;
    public float firingRange;
    public float firingFrequency;
    public float firingDamage;
    public LayerMask fireLayerMask;

    [Header("Effects")]
    public ParticleSystem muzzelFlash;
    public ParticleSystem impactEffect;

    // target the gun will aim at
    Transform go_target;
    Transform m_transform;

    // Used to start and stop the turret firing
    bool canFire = false;

    // Used to initialize turrent
    bool isReady = false;

    // Used to calculate fire frequency
    float timer = 0;

    // Used to track current rotation speed
    float currentRotationSpeed;
    #endregion

    #region [Start, OnEnable, InitializeAnimation]
    void Start()
    {
        m_transform = this.transform;
    }

    private void OnEnable()
    {
        go_base.localPosition = new Vector3(0, 25, 0);
        StartCoroutine(InitializeAnimation());
    }

    IEnumerator InitializeAnimation()
    {
        while (go_base.localPosition.y > 0)
        {
            float posY = go_base.localPosition.y - Time.deltaTime * 100;

            go_base.localPosition = new Vector3(0, posY, 0);

            yield return null;
        }

        go_base.localPosition = Vector3.zero;
        isReady = true;
    }
    #endregion

    #region [Update]
    void Update()
    {
        if (isReady)
        {
            FindEnemy();
            AimAndFire();
            SetImpactEffect();
        }
    }
    #endregion

    #region [Gizmos]
    void OnDrawGizmosSelected()
    {
        // Draw a red sphere at the transform's position to show the firing range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, firingRange);
    }
    #endregion

    #region [Fire Control]
    void FindEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(m_transform.position, firingRange, fireLayerMask);

        if (hitColliders.Length > 0)
        {
            bool isEnemyNearby = false;
            float nearestDistance = float.MaxValue;
            Transform target = null;
            foreach (var enemy in hitColliders)
            {
                if (enemy.gameObject.activeSelf)
                {
                    isEnemyNearby = true;
                    canFire = true;

                    float distance = (enemy.transform.position - m_transform.position).sqrMagnitude;

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        target = enemy.transform;
                    }
                }
            }

            if (isEnemyNearby)
            {
                if (go_target == null || !go_target.gameObject.activeSelf)
                {
                    if (target != null)
                    {
                        go_target = target;
                        timer = 0;
                    }
                }
            }
            else
            {
                go_target = null;
                canFire = false;
            }
        }
        else
        {
            go_target = null;
            canFire = false;
        }
    }

    void AimAndFire()
    {
        // Gun barrel rotation
        go_barrel.transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);

        // if can fire turret activates
        if (canFire)
        {
            // start rotation
            currentRotationSpeed = barrelRotationSpeed;

            // aim at enemy
            Vector3 baseTargetPostition = new Vector3(go_target.position.x, this.transform.position.y, go_target.position.z);

            Vector3 baseDirectionToTarget = (go_target.position - transform.position).normalized;
            baseDirectionToTarget = new Vector3(baseDirectionToTarget.x, 0, baseDirectionToTarget.z);

            Quaternion targetRotation = Quaternion.LookRotation(baseDirectionToTarget);
            go_baseRotation.transform.rotation = Quaternion.RotateTowards(go_baseRotation.transform.rotation, targetRotation, Time.deltaTime * currentRotationSpeed);

            // start particle system 
            if (Quaternion.Angle(targetRotation, go_baseRotation.transform.rotation) < 30)
            {
                DamageEnemy();
                if (!muzzelFlash.isPlaying)
                    muzzelFlash.Play();
            }
            else
            {
                if (muzzelFlash.isPlaying)
                    muzzelFlash.Stop();
            }
        }
        else
        {
            // slow down barrel rotation and stop
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0, 10 * Time.deltaTime);

            // stop the particle system
            if (muzzelFlash.isPlaying)
            {
                muzzelFlash.Stop();
            }

            timer = 0;
        }
    }

    void DamageEnemy()
    {
        timer += Time.deltaTime;

        if (timer > (1 / Mathf.Max(float.MinValue, firingFrequency)))
        {
            go_target.GetComponent<EnemyController>().DamageEnemy(firingDamage);
            timer = 0;
        }
    }
    #endregion

    #region [VFX Control]
    void SetImpactEffect()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(go_barrel.position, go_barrel.forward, out hitInfo, 100, fireLayerMask))
        {
            impactEffect.transform.position = hitInfo.point;
            impactEffect.transform.forward = hitInfo.normal;
        }

        if (canFire)
        {
            if (!impactEffect.isPlaying)
                impactEffect.Play();
        }
        else
        {
            if (impactEffect.isPlaying)
                impactEffect.Stop();
        }
    }
    #endregion
}