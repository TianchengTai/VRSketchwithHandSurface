using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShowController : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject surfacePlane;
    private GameObject conePlane;
    private GameObject cylinderPlane;
 
    void Start()
    { 

        surfacePlane = GameObject.Find("SurfacePlane");
        conePlane = GameObject.Find("ConePlane");
        cylinderPlane = GameObject.Find("CylinderPlane");
 
        surfacePlane.SetActive(false);
        conePlane.SetActive(false);
        cylinderPlane.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void showSurface()
    {
        conePlane.SetActive(false);
        cylinderPlane.SetActive(false);
        if(surfacePlane.activeSelf)
    {
        surfacePlane.SetActive(false);
    }
    else
    {
        surfacePlane.SetActive(true);
    } 
    }

    public void showCone()
    {
        surfacePlane.SetActive(false);
        cylinderPlane.SetActive(false);
        if (conePlane.activeSelf)
        {
            conePlane.SetActive(false);
        }
        else
        {
            conePlane.SetActive(true);
        }
    }

    public void showCylinder()
    {
        surfacePlane.SetActive(false);
        conePlane.SetActive(false);
        if (cylinderPlane.activeSelf)
        {
            cylinderPlane.SetActive(false);
        }
        else
        {
            cylinderPlane.SetActive(true);
        }
    }
    public void setFalse()
    {
        surfacePlane.SetActive(false);
        conePlane.SetActive(false);
        cylinderPlane.SetActive(false);
    }
    public void genSurfaceWithLeftTrigger(System.String mesh)
    {
        GameObject cone = new GameObject();
        MeshFilter mf = cone.AddComponent<MeshFilter>();
        Debug.Log("Assets/Resources/" + mesh + ".asset");
        mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/"+mesh+".asset");
        cone.AddComponent<MeshRenderer>();
        cone.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        GameObject camera = GameObject.Find("Main Camera");
        cone.transform.position= camera.transform.position+(camera.transform.forward+ 0.5f*camera.transform.right-0.5f*camera.transform.up).normalized;
        InitSurface(cone, mesh, Color.grey);

    }
    void InitSurface(GameObject surface, string name, Color color)
    {
        if (surface.GetComponent<Collider>())
        {
            Destroy(surface.GetComponent<Collider>());
        }
        MeshCollider MC = surface.AddComponent<MeshCollider>();
        MC.convex = true;
        //surface.layer = LayerMask.NameToLayer("virtual_surface");
        surface.name = name;
        //surface.transform.position = position;
        surface.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        surface.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 0.3f);
    }
}
