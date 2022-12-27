using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGenericVisual : MonoBehaviour
{
    [System.Serializable]
    public struct TilemapSpriteUV
    {
        public Tilemap.TilemapObject.TilemapSprite tilemapSprite;
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    private struct UVCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }
    [SerializeField] private TilemapSpriteUV[] tilemapSpriteUVArray = new TilemapSpriteUV[3];

    private Grid<Tilemap.TilemapObject> grid ;
    private Mesh mesh;
    private bool updateMesh;
    private Dictionary<Tilemap.TilemapObject.TilemapSprite, UVCoords> uvCoordsDictionary;
    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;

        uvCoordsDictionary = new Dictionary<Tilemap.TilemapObject.TilemapSprite, UVCoords>();

        foreach (TilemapSpriteUV tilemapSpriteUV in tilemapSpriteUVArray)
        {
            uvCoordsDictionary[tilemapSpriteUV.tilemapSprite] = new UVCoords
            {
                uv00 = new Vector2(tilemapSpriteUV.uv00Pixels.x / textureWidth, tilemapSpriteUV.uv00Pixels.y / textureHeight),
                uv11 = new Vector2(tilemapSpriteUV.uv11Pixels.x / textureWidth, tilemapSpriteUV.uv11Pixels.y / textureHeight),
            };
        }
    }
    //закидываем наш грид для отображения
    public void SetGrid(Tilemap tilemap, Grid<Tilemap.TilemapObject> grid)
    {
        this.grid = grid;
        UpdateHeatMapVisual();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
        tilemap.OnLoaded += Tilemap_OnLoaded;
    }

    public void Tilemap_OnLoaded(object sender, System.EventArgs e)
    {
        updateMesh = true;
    }
    //чекаем изменения в гриде
    public void Grid_OnGridValueChanged(object sender, Grid<Tilemap.TilemapObject>.OnGridValueChangedEventArgs e)
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

                Tilemap.TilemapObject gridValue = grid.GetValue(x, y);
                Tilemap.TilemapObject.TilemapSprite tilemapSprite = gridValue.GetTilemapSprite();
                Vector2 gridUV00, gridUV11;
                if(tilemapSprite == Tilemap.TilemapObject.TilemapSprite.None)
                {
                    gridUV00 = Vector2.zero;
                    gridUV11 = Vector2.zero;
                    //quadSize = Vector3.zero;
                }
                else
                {
                    UVCoords uvCoords = uvCoordsDictionary[tilemapSprite];
                    gridUV00 = uvCoords.uv00;
                    gridUV11 = uvCoords.uv11;
                }
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridUV00, gridUV11); 
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}