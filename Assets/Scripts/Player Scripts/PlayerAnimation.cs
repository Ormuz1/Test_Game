using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(SpriteRenderer))]
public class PlayerAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    private int playerLastLookDirection;
    public bool canTurnAround = true;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        canTurnAround = true;
    }
    void Start()
    {
        playerLastLookDirection = playerController.playerLookDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if (canTurnAround && playerController.playerLookDirection != playerLastLookDirection)
        {
            playerLastLookDirection = playerController.playerLookDirection;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            FlipAllChildren(transform);
        }
    }

    private void FlipAllChildren(Transform obj)
    {

        foreach (Transform child in obj)
        {
            FlipTransform(child);
            if (obj.childCount > 0)
                FlipAllChildren(child);
        }
    }

    private void FlipTransform(Transform transformToFlip)
    {
        SpriteRenderer sprite;
        if (transformToFlip.TryGetComponent<SpriteRenderer>(out sprite))
            sprite.flipX = !sprite.flipX;
        transformToFlip.localPosition = new Vector3(
            transformToFlip.localPosition.x * -1,
            transformToFlip.localPosition.y,
            transformToFlip.localPosition.z);
        transformToFlip.localEulerAngles = new Vector3(
            transformToFlip.localEulerAngles.x,
            transformToFlip.localEulerAngles.y,
            transformToFlip.localEulerAngles.z * -1
        );
    }
}
