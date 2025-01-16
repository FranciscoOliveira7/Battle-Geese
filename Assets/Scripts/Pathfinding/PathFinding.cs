using System.Collections.Generic;
using System;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    PathGrid grid;

    void Awake()
    {
        grid = GetComponent<PathGrid>();
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        PathNode startNode = grid.NodeFromWorldPoint(request.pathStart);
        PathNode targetNode = grid.NodeFromWorldPoint(request.pathEnd);

        if (/*startNode.walkable && *//*targetNode.walkable && */startNode != targetNode)
        {
            Heap<PathNode> openSet = new Heap<PathNode>(grid.MaxSize);
            HashSet<PathNode> closedSet = new HashSet<PathNode>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                PathNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (PathNode neighbour in grid.GetNeighbours(currentNode))
                {
                    if (neighbour != targetNode && (!neighbour.walkable || closedSet.Contains(neighbour)))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        callback(new PathResult(waypoints, pathSuccess, request.callback, request._requester));
    }

    Vector3[] RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<PathNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    public bool IsPositionValid(Vector3 position)
    {
        PathNode node = grid.NodeFromWorldPoint(position);
        return node.walkable;
    }
}
