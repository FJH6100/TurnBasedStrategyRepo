using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode cameFromPathNode;
    private bool isWalkable = true;
    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }
    public override string ToString()
    {
        return "x: " + gridPosition.x + " z: " + gridPosition.z;
    }
    public int GetFCost()
    {
        return fCost;
    }
    public int GetGCost()
    {
        return gCost;
    }
    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }
    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;
    }

    public void SetCameFromPathNode(PathNode cameFromPathNode)
    {
        this.cameFromPathNode = cameFromPathNode;
    }

    public PathNode GetCameFromPathNode()
    {
        return cameFromPathNode;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }
}
