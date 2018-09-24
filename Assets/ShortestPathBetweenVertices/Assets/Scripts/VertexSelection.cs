using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexSelection : MonoBehaviour
{
    public static GameObject StartingPoint = null;
    public static GameObject DestinationPoint = null;
    public LayerMask clickMask;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector3 clickPosition = -Vector3.one;

            RayCast(clickPosition);

            if (StartingPoint != null && DestinationPoint != null)
                PathFinder.find(StartingPoint, DestinationPoint);

            Debug.Log(clickPosition);
        }
    }
    public void RayCast(Vector3 clickPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, clickMask))
        {

            clickPosition = hit.point;

            if (Input.GetMouseButtonDown(0))
            {
                if (StartingPoint != null)
                    DestroyObject(StartingPoint);
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = clickPosition;
                sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                sphere.name = "StartingPoint";
                sphere.GetComponent<Renderer>().material.color = Color.blue;
                StartingPoint = sphere;
            }
            else
            {
                if (DestinationPoint != null)
                    DestroyObject(DestinationPoint);
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = clickPosition;
                sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                sphere.name = "DestinationPoint";

                sphere.GetComponent<Renderer>().material.color = Color.red;

                DestinationPoint = sphere;
            }
        }
    }
}
