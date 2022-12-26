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
    public int type;
    public Faction occupied;
    public Sprite check;
    public PathNode CameFromNode;
    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        type = 1;
        occupied = Faction.None;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(Tilemap.TilemapObject.TilemapSprite tilemapSprite)
    {
        if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Grass)
            this.type = 1;
        else if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Water)
            this.type = 2;
        else
            this.type = 3;
        grid.TriggeredGridObjectChanged(x, y);
    }

    public void SetIsOccupied(Faction occupation)
    {
        this.occupied = occupation;
    }
    public override string ToString()
    {
        return null;//x + "," + y;
    }
}
