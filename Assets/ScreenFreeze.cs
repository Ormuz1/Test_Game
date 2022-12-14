using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFreeze : MonoBehaviour
{
    public void FreezeScreen(float duration)
    {
        Time.timeScale = 0;
        StartCoroutine(StopFreezeAfterSeconds(duration));
    }

    private IEnumerator StopFreezeAfterSeconds(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1;
    }
}
