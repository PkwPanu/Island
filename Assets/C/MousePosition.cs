using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera maincamera;
    public GameObject Cube;

    // Update is called once per frame
    void Update()
    {
        Ray ray = maincamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray,out RaycastHit raycastHit))
        {
            
            transform.position = raycastHit.point;

            if (raycastHit.collider.tag == "water")
            {
           
                Cube = raycastHit.transform.gameObject;
            }
        }

    }

}
