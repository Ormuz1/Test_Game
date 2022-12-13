using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform hammerNormalPosition;
    [SerializeField] private Transform hammerChargedPosition;
    [SerializeField] private WaypointPath hammerWaypoints;
    [SerializeField] private Transform hammerSwingPosition;
    [SerializeField] private Transform hammerObject;
    [SerializeField] private Collider2D hammerHitbox;
    private Rigidbody2D hammerRB;
    [SerializeField] private float hammerChargeTime;
    [SerializeField] private float hammerSwingTime;
    [SerializeField] private float attackRecoveryTime;
    [SerializeField] private float hammerReturnToInitialPositionTime;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private UltEvents.UltEvent OnGroundHit;
    [SerializeField] private UltEvents.UltEvent OnAttackFullyCharged;
    private ContactFilter2D hitboxFilter;
    private PlayerAnimation playerAnimation;
    private bool attackInput;
    private bool isAttacking;
    private bool canAttack;

    private void Awake() {
        playerAnimation = GetComponent<PlayerAnimation>();
        hammerRB = hammerObject.GetComponent<Rigidbody2D>();
        hammerHitbox.enabled = false;
        isAttacking = false;
        canAttack = true;
        hitboxFilter.useLayerMask = true;
        hitboxFilter.layerMask = enemyLayer;
    }

    private void Update() {
        GatherInput();

        if(isAttacking || !canAttack)
        {
            return;
        }

        if(attackInput)
        {
            StartCoroutine(ChargeAttack());
        }
        
    }


    private IEnumerator ChargeAttack()
    {
        playerAnimation.canTurnAround = false;
        isAttacking = true;
        float lerpAmount = Utility.InverseLerp(hammerNormalPosition.position, hammerChargedPosition.position, hammerObject.position);

        for(float timer = lerpAmount * hammerChargeTime; timer < hammerChargeTime; timer += Time.deltaTime)
        {
            if(!attackInput) 
            {
                playerAnimation.canTurnAround = true;
                isAttacking = false;
                StartCoroutine(ReturnWeaponToOriginalPosition(timer));
                yield break;
            }
            hammerRB.MovePosition(Vector3.Lerp(hammerNormalPosition.position, hammerChargedPosition.position, timer / hammerChargeTime));
            hammerObject.rotation = Quaternion.Lerp(hammerNormalPosition.rotation, hammerChargedPosition.rotation, timer / hammerChargeTime);
            yield return null;
        }
        OnAttackFullyCharged.Invoke();
        while (attackInput)
            yield return null;
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        hammerHitbox.enabled = true;
        float dist1 = Vector3.Distance(hammerObject.position, hammerNormalPosition.position);
        float dist2 = Vector3.Distance(hammerNormalPosition.position, hammerSwingPosition.position);
        float totalDist = dist1 + dist2;
        float speed = totalDist / hammerSwingTime;
        float halfAttackTime = hammerSwingTime / 2;
        for(float timer = 0; timer < hammerSwingTime; timer += Time.fixedDeltaTime)
        {
            if(timer < halfAttackTime)
            {
                hammerRB.MovePosition(Vector3.MoveTowards(hammerObject.position, hammerNormalPosition.position, speed * Time.fixedDeltaTime));
                hammerRB.MoveRotation(Quaternion.Lerp(hammerChargedPosition.rotation, hammerNormalPosition.rotation, timer / halfAttackTime));
                yield return new WaitForFixedUpdate();
            }
            else
            {
                hammerRB.MovePosition(Vector3.MoveTowards(hammerObject.position, hammerSwingPosition.position, speed * Time.fixedDeltaTime));
                hammerRB.MoveRotation(Quaternion.Lerp(hammerNormalPosition.rotation, hammerSwingPosition.rotation, (timer - halfAttackTime) / halfAttackTime));
                yield return new WaitForFixedUpdate();
            }

            Enemy[] enemiesHit = GetAllEnemiesHit();
            if(enemiesHit.Length == 0)
                continue;
            foreach (Enemy enemyHit in GetAllEnemiesHit())
            {
                enemyHit.OnAttackHit();
            }
            break;
        }
        if(hammerHitbox.IsTouchingLayers(groundLayer))
        {
            OnGroundHit.Invoke();
        }
        hammerHitbox.enabled = false;
        isAttacking = false;
        canAttack = false;
        yield return new WaitForSeconds(attackRecoveryTime);
        yield return ReturnWeaponToOriginalPosition();
        playerAnimation.canTurnAround = true;
        canAttack = true;
    }

    private Enemy[] GetAllEnemiesHit()
    {
        List<Enemy> enemiesHit = new List<Enemy>();
        List<Collider2D> hitColliders = new List<Collider2D>();
        if (hammerHitbox.enabled && hammerHitbox.OverlapCollider(hitboxFilter, hitColliders) != 0)
        {
            foreach (Collider2D collider in hitColliders)
            {
                Enemy enemyComponent;
                if(!collider.TryGetComponent<Enemy>(out enemyComponent))
                    continue;
                enemiesHit.Add(enemyComponent);
            }
        }
        return enemiesHit.ToArray();
    }

    private IEnumerator ReturnWeaponToOriginalPosition()
    {
        yield return ReturnWeaponToOriginalPosition(hammerReturnToInitialPositionTime);
    }

    private IEnumerator ReturnWeaponToOriginalPosition(float time)
    {
        Vector3 startPosition = hammerObject.position;
        Quaternion startRotation = hammerObject.rotation;
    
        float speed = Vector3.Distance(startPosition, hammerNormalPosition.position) / time;
        for (float timer = 0; timer < time; timer += Time.deltaTime)
        {
            if(isAttacking)
                yield break;
            hammerObject.position = Vector3.MoveTowards(hammerObject.position, hammerNormalPosition.position, speed * Time.deltaTime);    
            hammerObject.rotation = Quaternion.Lerp(startRotation, hammerNormalPosition.rotation, timer / time);
            yield return null;
        }
    }

    private void GatherInput()
    {
        attackInput = Input.GetButton("Fire1");
    }
}
