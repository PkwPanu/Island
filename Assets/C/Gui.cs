
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using UnityEngine;


namespace HelloWorld
{
    public class Gui : MonoBehaviour
    {
        public string stringToEdit = "127.0.0.1";
     
        void OnGUI()
        {
            if(!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            stringToEdit = GUI.TextField(new Rect(10, 200, 200, 20), stringToEdit, 25);
            
            GUILayout.BeginArea(new Rect(10, 300, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
             
                StartButtons();
                
            }
            else
            {
                StatusLabels();

                //SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

         void StartButtons()
        {
            if (GUILayout.Button("Host"))
            {
                NetworkManager.Singleton.StartHost();
                GameObject.Find("GameMaster").GetComponent<test>().StartHost();
            }
            if (GUILayout.Button("Connenct to MyPc"))
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("58.11.6.140", 7777);
                NetworkManager.Singleton.StartClient();
            }
            if (GUILayout.Button("Client to my localPc"))
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("192.168.1.54", 7777); ;
                NetworkManager.Singleton.StartClient();
            }
            if (GUILayout.Button("Client"))
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(stringToEdit, 7777); ;
                NetworkManager.Singleton.StartClient();
            }
      

            if (GUILayout.Button("Server"))
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(stringToEdit, 7777);
                NetworkManager.Singleton.StartServer();
            }

            //if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
            if (GUILayout.Button("Close")) Application.Quit();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

           // GUILayout.Label("Transport: " +
           //     NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);

            
        }
/*
        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
                {
                    foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                        NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
                }
                else
                {
                    var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<HelloWorldPlayer>();
                    player.Move();
                }
            }
        }
*/
    }
}

