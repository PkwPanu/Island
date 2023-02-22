using Newtonsoft.Json.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class test : NetworkBehaviour
{
    [SerializeField] GameObject mouse;
    MyGrid Grid, Hand, Draw;

    [SerializeField] GameObject cube;
    [SerializeField] GameObject CubeArea;
    [SerializeField] GameObject DrawBotton;
    [SerializeField] Material[] Player_color;

     GameObject currentCube ,Cube1, Cube2;

    int numPlayer;
    int currentNum;
    int id = -1;
    NetworkVariable<int> i_player = new NetworkVariable<int>(default,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    NetworkVariable<bool> DrawEnable = new NetworkVariable<bool>(default,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    bool laydownEnable = false;

  
    // Start is called before the first frame update
    void Start()
    {
        Grid = new MyGrid(15, 15, 10f, new Vector3(-75, 0, -75));
        Hand = new MyGrid(2, 1, 10f, new Vector3(-10, 0, -90));
        Draw = new MyGrid(1, 1, 6f, new Vector3(-20, 0, -90));

        numPlayer = 4;
        Grid.InitiatePlayer(numPlayer);
        Grid.InitiateBool();

        Instantiate(DrawBotton, Draw.CenterPosition(0, 0) + new Vector3(0, 10, 0), Quaternion.identity);
        Island();

        //Grid.check();
    }

    public void StartHost()
    {
        Debug.Log("server start");
        i_player.Value = 0;
        DrawEnable.Value = true;
        id = int.Parse(NetworkManager.LocalClientId.ToString());
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        
            StatusLabels();

        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(200, 10, 300, 300));

        GUILayout.Label(NetworkManager.LocalClientId.ToString());
        if (IsServer)
                {
                    playerLabels();
                }
        GUILayout.EndArea();
    }
    void StatusLabels()
    {
        GUILayout.Label("player : " + i_player.Value.ToString());
        GUILayout.Label("DrawEnable : " + DrawEnable.Value.ToString());
        GUILayout.Label("laydownEnable : " + laydownEnable.ToString());
    }

    void playerLabels()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
            {
                GUILayout.Label("id : " + uid.ToString());
            }
        }
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Debug.Log("hi");
        }
    }

    void Update()
    {
        //Debug.Log(id + " / " + i_player.Value);
        if (id < 1)
        {
            if (NetworkManager.LocalClientId < 5)
            {
                id = int.Parse(NetworkManager.LocalClientId.ToString());
            }
        }
       
        if(id != i_player.Value%2)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && Draw.OnGrid(mouse.transform.position) && DrawEnable.Value)
        {

            if (IsServer)
            {
                int x = Random.Range(1, 7);            
                ChangCubeClientRpc(x, 7 - x);
                DrawEnable.Value = false;
            }
            else
            {
                SubmitDrawRequestServerRpc();
            }

            updateEnablePointServerRpc();     
            //Grid.updatecheck();



        }

        if (Input.GetMouseButtonDown(0) && Hand.OnGrid(mouse.transform.position))
        {
            
            Hand.GetXY(mouse.transform.position, out int x, out int y);
            currentCube = mouse.GetComponent<MousePosition>().Cube;
            currentNum = int.Parse(currentCube.transform.Find("Canvas").transform.Find("Button").transform.Find("Text").GetComponent<Text>().text);
            submitcurrent_numServerRpc(currentNum);
            laydownEnable = true;
        }

        if (Input.GetMouseButtonDown(0) && Grid.OnGrid(mouse.transform.position) && laydownEnable)
        {
            layCube();
            
        }

        if (Input.GetMouseButtonDown(1) && Grid.OnGrid(mouse.transform.position))
        {
            Debug.Log(Grid.GetValue(mouse.transform.position));
        }

        
    }

    [ServerRpc(RequireOwnership = false)]
    void submitcurrent_numServerRpc(int x)
    {
        currentNum = x;
    }



    void layCube()
    {
        Grid.GetXY(mouse.transform.position, out int x, out int y);
        layCubeServerRpc(x, y);
      
    }

    [ServerRpc(RequireOwnership = false)]
    void layCubeServerRpc(int x, int y)
    {
        if (!Grid.gridbool[x, y])
        {
            return;
        }

        if (Grid.GetValue(x, y) < 2)
        {
            layClientRpc(x, y,currentNum);
            layOver(x, y);
            Grid.setValue(x, y, Grid.GetValue(x, y) + 1);
            nextround();
        }
    }

    [ClientRpc]
    void layClientRpc(int x, int y,int num)
    {
        GameObject Cube = Instantiate(cube, Grid.CenterPosition(x, y) + new Vector3(0, 20, 0), Quaternion.identity);
        Cube.transform.Find("Canvas").transform.Find("Button").transform.Find("Text").GetComponent<Text>().text = num.ToString();
        Cube.GetComponent<MeshRenderer>().material = Player_color[i_player.Value];
        laydownEnable = false;
        Destroy(Cube1);
        Destroy(Cube2);
    }

    void layOver(int x, int y)
    {
        for (int i = 0; i < numPlayer; i++)
        {
            Grid.gridvalue[i, x, y] = 0;
        }
        Grid.gridvalue[i_player.Value, x, y] = currentNum;
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitDrawRequestServerRpc(ServerRpcParams serverRpcRarams = default)
    {
        int x = Random.Range(1, 7);
        ChangCubeClientRpc(x, 7 - x);
        DrawEnable.Value = false;
    }

    [ClientRpc]
    void ChangCubeClientRpc(int val1, int val2,ClientRpcParams clientRpcParams = default)
    {
       
        Cube1 = Instantiate(cube, Hand.CenterPosition(0, 0) + new Vector3(0, 10, 0), Quaternion.identity);
        Cube1.transform.Find("Canvas").transform.Find("Button").transform.Find("Text").GetComponent<Text>().text = val1.ToString();
        Cube1.GetComponent<MeshRenderer>().material = Player_color[i_player.Value]; 
        Cube2 = Instantiate(cube, Hand.CenterPosition(1, 0) + new Vector3(0, 10, 0), Quaternion.identity);
        Cube2.transform.Find("Canvas").transform.Find("Button").transform.Find("Text").GetComponent<Text>().text = val2.ToString();
        Cube2.GetComponent<MeshRenderer>().material = Player_color[i_player.Value];
    }


    void nextround()
    {
        DrawEnable.Value = true;
        i_player.Value = (i_player.Value + 1) % 4;
    }

    [ServerRpc(RequireOwnership = false)]
    void updateEnablePointServerRpc()
    {
        Grid.InitiateBool();
        
        for (int x = 0; x < Grid.width; x++)
        {
            for (int y = 0; y < Grid.height; y++)
            {  
                int distance = Grid.gridvalue[i_player.Value, x, y];
                if (distance > 0)
                {
                    int N = 0, S = 0, E = 0, W = 0;
                    for (int i = 1; i < distance; i++)
                    {
                        N += Grid.GetValue(x + i, y);
                        S += Grid.GetValue(x - i, y);
                        E += Grid.GetValue(x, y + i);
                        W += Grid.GetValue(x, y - i);
                    }
                    if (N == 0)
                        Grid.setBool(x + distance, y, true);
                    if (S == 0)
                        Grid.setBool(x - distance, y, true);
                    if (E == 0)
                        Grid.setBool(x, y + distance, true);
                    if (W == 0)
                        Grid.setBool(x, y - distance, true);
                }
            }
        }

        if (i_player.Value == 0)
            Grid.gridbool[0, 0] = true;
        if (i_player.Value == 1)
            Grid.gridbool[0, 14] = true;
        if (i_player.Value == 2)
            Grid.gridbool[14, 0] = true;
        if (i_player.Value == 3)
            Grid.gridbool[14, 14] = true;

    }

    void createIsland(Vector3 position)
    {
        Instantiate(CubeArea, position, Quaternion.identity);
    }

    void Island()
    {
        int[] zeta;
        for (int x = 0; x < Grid.width; x++)
        {
            if (x == 0 || x == 1 || x == 13 || x == 14)
                zeta = new int[] { 0, 1, 6, 7, 8, 13, 14 };
            else if (x == 3 || x == 4 || x == 10 || x == 11)
                zeta = new int[] { 2, 3, 4, 10, 11, 12 };
            else if (x == 5 || x == 9)
                zeta = new int[] { 3, 11 };
            else if (x == 6 || x == 7 || x == 8)
                zeta = new int[] { 6, 7, 8 };
            else
                zeta = new int[] { };
            for (int y = 0; y < Grid.height; y++)
            {
                if (compare(y, zeta))
                {
                    createIsland(Grid.CenterPosition(x, y));
                }
            }
        }
    }

    bool compare(int a, int[] b)
    {
        foreach (int x in b)
        {
            if (x == a)
                return true;
        }
        return false;
    }

}
