using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MoseOnNetwork : NetworkBehaviour
{
    [SerializeField] private Camera maincamera;
    public GameObject Cube;

    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            transform.position = new Vector3(0, 10, 0);
        }
    }

    public void Move()
    {
            Ray ray = maincamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {

                transform.position = raycastHit.point;

                if (raycastHit.collider.tag == "water")
                {
                    Debug.Log("cube");
                    Cube = raycastHit.transform.gameObject;
                }
            }

            //transform.position = Position.Value; ;
    
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {

    }


    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
