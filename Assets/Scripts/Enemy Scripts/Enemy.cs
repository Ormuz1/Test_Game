using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public UltEvents.UltEvent OnHit;
    [SerializeField] private float invulnerabilityTime;
    private bool isVulnerable;
    private float invulnerabilityTimer;


    private void Awake() {
        isVulnerable = true;
    }
}
