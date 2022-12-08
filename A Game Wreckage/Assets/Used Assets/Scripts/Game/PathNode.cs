using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid<PathNode> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;
    //1 - трава, 2 - вода, 3 - гора
    public int type ;

    public PathNode CameFromNode;
    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        type = 1;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(int type1)
    {
        this.type = type1;
        grid.TriggeredGridObjectChanged(x, y);
    }
    public override string ToString()
    {
        return null;//x + "," + y;
    }
}
