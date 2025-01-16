using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PathGrid : MonoBehaviour
{
	public event Action OnGridGenerated;
	public bool displayGridGizmos;
	public bool displayOnlyUnwalkableGizmos;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public float unwalkablePadding;
	PathNode[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;
	private bool isFirstUpdate = false;

	void Start() {
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		// CreateGrid();
	}

	private void Update()
	{
		if (!isFirstUpdate)
		{
			isFirstUpdate = true;
			CreateGrid();
			OnGridGenerated?.Invoke();
		}
	}

	public int MaxSize => gridSizeX * gridSizeY;

	void CreateGrid() {
		grid = new PathNode[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = CheckWalkable(worldPoint, unwalkablePadding);
				grid[x,y] = new PathNode(walkable,worldPoint, x,y);
			}
		}
	}
	
	private bool CheckWalkable(Vector3 worldPoint, float overflow)
	{
		return !Physics.CheckSphere(worldPoint, nodeRadius + overflow, unwalkableMask);
	}

	public List<PathNode> GetNeighbours(PathNode node) {
		List<PathNode> neighbours = new List<PathNode>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add(grid[checkX,checkY]);
				}
			}
		}
		return neighbours;
	}

	public PathNode NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f;
		float percentY = (worldPosition.z - transform.position.z) / gridWorldSize.y + 0.5f;

		int x = Mathf.RoundToInt(Mathf.Clamp(gridSizeX * percentX, 0f, gridSizeX - 1));
		int y = Mathf.RoundToInt(Mathf.Clamp(gridSizeY * percentY, 0f, gridSizeY - 1));
		return grid[x,y];
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
		if (grid != null && displayGridGizmos) {
			foreach (PathNode n in grid) {
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				if (displayOnlyUnwalkableGizmos && n.walkable) continue;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
			}
		}
	}
}
