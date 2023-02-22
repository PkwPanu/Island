using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGrid
{
    public int width;
    public int height;
    private float cellSize;
    private Vector3 originPosition;

    //variable
    private int[,] gridArray;
    private bool[,] gridbool;

    

    public MiniGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        gridArray = new int[width, height];
        gridbool = new bool[width, height];

    }

    private Vector3 GetworldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldposition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldposition.x - originPosition.x) / cellSize);
        y = Mathf.FloorToInt((worldposition.z - originPosition.z) / cellSize);
    }

    public Vector3 CenterPosition(Vector3 worldposition)
    {
        int x, y;
        GetXY(worldposition, out x, out y);
        return GetworldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f;
    }

    public Vector3 CenterPosition(int x, int y)
    {
        return GetworldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f;
    }

    public void setValue(int x, int y, int value)
    {

        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;          
        }
    }


    public void setValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        setValue(x, y, value);

    }

    public void setbool(int x, int y, bool value)
    {

        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridbool[x, y] = value;
        }
    }

    public bool[,] GetBool()
    {
      return gridbool;    
    }


    public void setbool(Vector3 worldPosition, bool value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        setbool(x, y, value);

    }

    public bool OnGrid(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

}
