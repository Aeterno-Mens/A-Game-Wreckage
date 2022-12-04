using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap
{
    private Grid<int> grid;
    public Tilemap(int width, int height, float cellSize, Vector3 originalPosition)
    {
        grid = new Grid<int>(width, height, cellSize, originalPosition, (Grid<int> g, int x, int y) => 0);
    }
}
