using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaypointPath : MonoBehaviour
{
    private const float GIZMO_SPHERE_RADIUS = .2f;
    private void OnDrawGizmos() {
        LinearPathNode[] nodes = GetComponentsInChildren<LinearPathNode>();
        for (int i = 0; i < nodes.Length; i++)
        {
            LinearPathNode node = nodes[i];
            DrawNodeGizmo(node.transform.position, i < nodes.Length - 1? nodes[i +1 ].transform.position : null);
        }
    }

    private void DrawNodeGizmo(Vector3 nodePosition, Vector3? nextNodePosition = null)
    {
        Gizmos.DrawWireSphere(nodePosition, GIZMO_SPHERE_RADIUS);
        if(!(nextNodePosition is null))
        {
            Gizmos.DrawLine(nodePosition, (Vector3) nextNodePosition);
        }

    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        int waypointIndex = currentWaypoint.GetSiblingIndex();
        if (currentWaypoint == null || waypointIndex == transform.childCount - 1)
            return transform.GetChild(0);
        return transform.GetChild(waypointIndex + 1);
    }

    public LinearPathNode[] GetPathToWaypoint(LinearPathNode currentWaypoint, LinearPathNode targetWaypoint)
    {
        List<LinearPathNode> path = new List<LinearPathNode>();
        LinearPathNode[] nodes = GetComponentsInChildren<LinearPathNode>();
        int currentWaypointIndex = Array.IndexOf(nodes, currentWaypoint);
        int targetWaypointIndex = Array.IndexOf(nodes, targetWaypoint);
        int MoveTowardsTarget(int currentIndex) => targetWaypointIndex < currentIndex ? currentIndex - 1 : currentIndex + 1;
        int i;
        for (i = currentWaypointIndex; i != targetWaypointIndex; i = MoveTowardsTarget(i))
        {
            path.Add(nodes[i]);
        }
        path.Add(nodes[i]);
        return path.ToArray();
    }
}


public static class TransformExtension
{    
    public static IEnumerator MoveObjectAlongPath(this Transform objectToMove, LinearPathNode[] path, float speed)
    {
        objectToMove.position = path[0].transform.position;
        objectToMove.rotation = path[0].transform.rotation;
        for (int i = 1; i < path.Length; i++)
        {
            Transform nodeTransform = path[i].transform;
            yield return objectToMove.ChangePositionAndRotationGradually(nodeTransform.position, nodeTransform.rotation, speed);
        }
    }
    
    public static IEnumerator ChangePositionAndRotationGradually(this Transform objectToMove, Vector3 targetPosition, Quaternion targetRotation, float speed)
    {
        Vector3 startPosition = objectToMove.position;
        Quaternion startRotation = objectToMove.rotation;
        float timeToReachTarget = Vector3.Distance(startPosition, targetPosition) / speed;
        while (objectToMove.position != targetPosition)
        {
            float timeElapsed = timeToReachTarget - (Vector3.Distance(objectToMove.position, targetPosition) / speed);
            float lerpAmount = timeElapsed / timeToReachTarget;
            objectToMove.position = Vector3.MoveTowards(objectToMove.position, targetPosition, speed * Time.deltaTime);
            objectToMove.rotation = Quaternion.Lerp(startRotation, targetRotation, lerpAmount);
            yield return null;
        }
        objectToMove.position = targetPosition;
        objectToMove.rotation = targetRotation;
    }
}

