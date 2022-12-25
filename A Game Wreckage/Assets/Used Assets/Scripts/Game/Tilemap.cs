using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tilemap
{
    public event EventHandler OnLoaded;
    private Grid<TilemapObject> grid;
    public Tilemap(int width, int height, float cellSize, Vector3 originalPosition)
    {
        grid = new Grid<TilemapObject>(width, height, cellSize, originalPosition, (Grid<TilemapObject> g, int x, int y) => new TilemapObject(g, x, y));
    }

    public void SetTilemapSprite(Vector3 position, TilemapObject.TilemapSprite tilemapSprite)
    {
        TilemapObject tilemapObject = grid.GetValue(position);
        if(tilemapObject != null)
        {
            tilemapObject.SetTilemapSprite(tilemapSprite);
        }
    }

    public TilemapObject.TilemapSprite GetTilemapSprite(Vector3 position)
    {
        TilemapObject tilemapObject = grid.GetValue(position);
        if (tilemapObject != null)
        {
            return tilemapObject.GetTilemapSprite();
        }
        return default;
    }

    public void SetTilemapVisual(TilemapGenericVisual tilemapVisual)
    {
        tilemapVisual.SetGrid(this, grid);
    }

    /*Система сохранения и загрузки файла тайлкарты*/
    public class SaveObject
    {
        public TilemapObject.SaveObject[] tilemapObjectSaveObjectArray;
    }
    public void Save()
    {
        List<TilemapObject.SaveObject> tilemapObjectSaveList = new List<TilemapObject.SaveObject>();
        for (int x = 0; x < grid.GetWidth(); ++x)
        {
            for (int y = 0; y < grid.GetHeight(); ++y)
            {
                TilemapObject tilemapObject = grid.GetValue(x, y);
                tilemapObjectSaveList.Add(tilemapObject.Save());
            }
        }
        SaveObject saveObject = new SaveObject { tilemapObjectSaveObjectArray = tilemapObjectSaveList.ToArray() };

        SaveSystem.SaveObject(saveObject);
    }

    public void Load(string filename)
    {
        SaveObject saveObject =  SaveSystem.LoadObject<SaveObject>(filename);
        foreach (TilemapObject.SaveObject tilemapObjectSaveObject in saveObject.tilemapObjectSaveObjectArray)
        {
            TilemapObject tilemapObject = grid.GetValue(tilemapObjectSaveObject.x, tilemapObjectSaveObject.y);
            tilemapObject.Load(tilemapObjectSaveObject);
        }
        OnLoaded?.Invoke(this, EventArgs.Empty);
    }
    public class TilemapObject
    {
        public enum TilemapSprite
        {
            None,
            Grass,
            Water,
            Mountain
        }
        private Grid<TilemapObject> grid;
        private int x;
        private int y;
        private TilemapSprite tilemapSprite;

        //Чтобы можно было работать с каждым отдельным элементов Карты тайлов в гриде
        public TilemapObject(Grid<TilemapObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }
        public void SetTilemapSprite(TilemapSprite tilemapSprite)
        {
            this.tilemapSprite = tilemapSprite;
            grid.TriggeredGridObjectChanged(x, y); 
        }
        public TilemapSprite GetTilemapSprite()
        {
            return tilemapSprite;
        }
        //этот оверрайд для отображения человеческого, ну или проверки желаемых значений динамически
        public override string ToString()
        {
            return null;//tilemapSprite.ToString();
        }

        [System.Serializable]
        public  class SaveObject
        {
            public TilemapSprite tilemapSprite;
            public int x;
            public int y;
        }

        //Сохранение и загрузка
        public  SaveObject Save()
        {
            return new SaveObject
            {
                tilemapSprite = tilemapSprite,
                x = x,
                y = y
            };
        }

        public void Load(SaveObject saveObject)
        {
            tilemapSprite = saveObject.tilemapSprite;
        }

    }
}
