using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Camera thisCamera;

    private void Awake() {
        thisCamera = GetComponent<Camera>() ?? Camera.main;
    }

    public void Shake(float duration, float intensity)
    {
        StartCoroutine(ScreenShakeCoroutine(duration, intensity));
    }

    private IEnumerator ScreenShakeCoroutine(float duration, float intensity)
    {
        Vector3 startPosition = thisCamera.transform.position;
        for(float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            Vector2 randomMovement = Random.insideUnitCircle;
            thisCamera.transform.position = startPosition + new Vector3(randomMovement.x, randomMovement.y, startPosition.z) * intensity;
            yield return null;
        }
    }
}
