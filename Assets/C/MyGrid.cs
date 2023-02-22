using UnityEngine;

public class MyGrid
{
    public int width;
    public int height;
    private float cellSize;
    private int[,] gridArray;
    
    public bool[,] gridbool;
    public int[,,] gridvalue;

    public TextMesh[,] debugTextArray;
    private Vector3 originPosition;

    public MyGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        debugTextArray = new TextMesh[width, height];

        gridArray = new int[width, height];
        


        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                //debugTextArray[x,y] = CreateWorldText(null, gridbool[0,x,y].ToString(), 
                //GetworldPosition(x,y)+ new Vector3(cellSize,0,cellSize)*.5f,20,Color.white, TextAnchor.MiddleCenter,
                //TextAlignment.Left,5000);
                //Debug.DrawLine(GetworldPosition(x, y), GetworldPosition(x, y + 1),Color.white,100f);
                DrawLine(GetworldPosition(x, y), GetworldPosition(x, y + 1), Color.white);
                DrawLine(GetworldPosition(x, y), GetworldPosition(x + 1, y), Color.white);         
            }
        }

        DrawLine(GetworldPosition(0, height), GetworldPosition(width, height), Color.white);
        DrawLine(GetworldPosition(width, 0), GetworldPosition(width, height), Color.white);
        

        
    }

    private Vector3 GetworldPosition(int x, int y)
    {
        return new Vector3(x,0,y) * cellSize + originPosition;
    }

   
    public void GetXY(Vector3 worldposition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldposition.x - originPosition.x) / cellSize);
        y = Mathf.FloorToInt((worldposition.z - originPosition.z) / cellSize);
    }

    public Vector3 CenterPosition(Vector3 worldposition)
    {
        int x, y;
        GetXY(worldposition, out  x, out y);
        return GetworldPosition(x,y) + new Vector3(cellSize, 0, cellSize) * .5f ;
    }

    public Vector3 CenterPosition(int x, int y)
    {
        return GetworldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f;
    }

    public void setValue(int x, int y, int value)
    {
        
        if (x >= 0 && y>=0 && x < width && y < height)
        {       
            gridArray[x, y]= value;
            //debugTextArray[x, y].text = gridArray[x, y].ToString(); 
        }
    }

    public void setBool(int x, int y, bool value)
    {

        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridbool[x, y] = value;      
        }
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

    public void setValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition,out x,out y);
        setValue(x, y, value);
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

    
     public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        transform.localEulerAngles = new Vector3(90, 0, 0);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
 
     void DrawLine(Vector3 start, Vector3 end, Color color)
         {
             GameObject myLine = new GameObject();
             myLine.transform.position = start;
             myLine.AddComponent<LineRenderer>();
             LineRenderer lr = myLine.GetComponent<LineRenderer>();
             lr.material = new Material(Shader.Find("Sprites/Default"));
             lr.startColor = color;
             lr.endColor = color;
             lr.startWidth = .5f;
             lr.endWidth = .5f;
             lr.SetPosition(0, start);
             lr.SetPosition(1, end);             
         }

    public void InitiatePlayer(int numPlayer)
    { 
        this.gridvalue = new int[numPlayer, width, height];
    }

    public void InitiateBool()
    {
        this.gridbool = new bool[width, height];
    }

    public void check()
    {
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                debugTextArray[x, y] = CreateWorldText(null, gridbool[x, y].ToString(),
                GetworldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter,
                TextAlignment.Left, 5000);              
            }
        }
    }

    public void updatecheck()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                debugTextArray[x, y].text = gridbool[x, y].ToString();
            }
        }
    }


}
