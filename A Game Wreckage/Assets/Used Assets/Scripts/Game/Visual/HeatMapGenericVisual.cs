using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapGenericVisual : MonoBehaviour
{
    private Grid<HeatMapGridObject> grid ;
    private Mesh mesh;
    private bool updateMesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    //закидываем наш грид для отображения
    public void SetGrid(Grid<HeatMapGridObject> grid)
    {
        this.grid = grid;
        UpdateHeatMapVisual();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }
    //чекаем изменения в гриде
    public void Grid_OnGridValueChanged(object sender, Grid<HeatMapGridObject>.OnGridValueChangedEventArgs e)
    {
        updateMesh = true;
    }
    //отображаем изменения
    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateHeatMapVisual(); 
        }
    }
    /*а тут уже взаимодействие с мешами и передача всех параметров, ну и с координатами приколы, 
     чтобы грид не был на 20 от ориджина по X, а меш остался в ориджине*/
    private void UpdateHeatMapVisual() {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);
         for (int x=0; x<grid.GetWidth(); ++x)
        {
            for(int y=0; y<grid.GetHeight(); ++y)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                HeatMapGridObject gridValue = grid.GetValue(x, y);
                float gridValueNormalized = gridValue.GetValueNormalized();
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValueUV, gridValueUV); 
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
