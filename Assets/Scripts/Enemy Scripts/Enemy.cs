using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float invulnerabilityTime;
    private bool isVulnerable;
    private float invulnerabilityTimer;


    private void Awake() {
        isVulnerable = true;
    }

    private void Update() {
        if (!isVulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            isVulnerable = invulnerabilityTimer <= 0;
        }
    }
    public void OnAttackHit()
    {
        if(!isVulnerable)
            return;
        Debug.Log("I'm hit!", this);
        isVulnerable = false;
        invulnerabilityTimer = invulnerabilityTime;
    }

}
