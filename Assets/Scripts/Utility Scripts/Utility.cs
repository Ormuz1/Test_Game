using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Utility
{
    public static void InvokeAction(this MonoBehaviour mb, System.Action f, float delay)
    {
        mb.StartCoroutine(InvokeCoroutine(f, delay));
    }
    private static IEnumerator InvokeCoroutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    public static void InstantiateThis(this GameObject go, Transform point)
    {
        GameObject.Instantiate(go, point);
    }
}

[System.Serializable]
public class ComponentUnityEvent : UnityEvent<Component> {}
