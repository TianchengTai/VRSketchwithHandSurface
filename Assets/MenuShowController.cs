using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRPainting;
using CommandUndoRedo;
public class MenuShowController : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject surfacePlane;
    private GameObject conePlane;
    private GameObject cylinderPlane;

    public Material mat;
 
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
        GameObject surface = new GameObject();
        MeshFilter mf = surface.AddComponent<MeshFilter>();
        Debug.Log("Assets/Resources/" + mesh + ".asset");
        mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/"+mesh+".asset");
        surface.AddComponent<MeshRenderer>();
        surface.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        GameObject camera = GameObject.Find("Main Camera");
        surface.transform.position= camera.transform.position+camera.transform.forward.normalized*0.5f;
        InitSurface(surface, mesh, Color.grey);
        surface.transform.SetParent(GameObject.Find("Drawing Board").transform);
        surface.layer = LayerMask.NameToLayer("hidden_surface");
        UndoRedoManager.Insert(new DrawCommand(surface));
        SelectModel.AddSurface(surface);
    }

    public void genSphere(){
        GameObject surface = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject camera = GameObject.Find("Main Camera");
        surface.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        surface.transform.position= camera.transform.position+camera.transform.forward.normalized*0.5f;
        InitSurface(surface, "shpere", Color.grey);
        surface.transform.SetParent(GameObject.Find("Drawing Board").transform);
        surface.layer = LayerMask.NameToLayer("hidden_surface");
        UndoRedoManager.Insert(new DrawCommand(surface));
        SelectModel.AddSurface(surface);
    }
    void InitSurface(GameObject surface, string name, Color color)
    {
        if (surface.GetComponent<Collider>())
        {
            Destroy(surface.GetComponent<Collider>());
        }
        MeshCollider MC = surface.AddComponent<MeshCollider>();
        MC.convex = true;
        surface.layer = LayerMask.NameToLayer("virtual_surface");
        surface.name = name;
        //surface.transform.position = position;
        surface.GetComponent<MeshRenderer>().material = mat;
        surface.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, 0.3f);
    }

    
}
