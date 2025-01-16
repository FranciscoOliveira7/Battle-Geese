using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePath
{
    public readonly Vector3[] lookPoints;
    public readonly Line[] turnBounderies;
    public readonly int finishLineIndex;
    public readonly int slowDownIndex;

    public NodePath(Vector3[] waypoints, Vector3 startPos, float turnDistance, float stoppingDistance)
    {
        lookPoints = waypoints;
        turnBounderies = new Line[lookPoints.Length];
        finishLineIndex = turnBounderies.Length - 1;
        
        Vector2 previousPoint = V3ToV2(startPos);
        for (int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = V3ToV2(lookPoints[i]);
            Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDistance;
            turnBounderies[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDistance);
            previousPoint = turnBoundaryPoint;
        }
        
        float dstFromEndPoint = 0f;
        for (int i = lookPoints.Length - 1; i > 0; i--)
        {
            dstFromEndPoint += Vector3.Distance(lookPoints[i], lookPoints[i - 1]);
            if (dstFromEndPoint > stoppingDistance)
            {
                slowDownIndex = i;
                break;
            }
        }
    }

    Vector2 V3ToV2(Vector3 v3) => new Vector2(v3.x, v3.z);

    public void DrawWithGizmos()
    {
        Gizmos.color = Color.black;
        foreach (Vector3 lookPoint in lookPoints)
        {
            Gizmos.DrawCube(lookPoint, Vector3.one / 4);
        }
        Gizmos.color = Color.white;
        foreach (Line line in turnBounderies)
        {
            line.DrawWithGizmos(10);
        }
    }
}
