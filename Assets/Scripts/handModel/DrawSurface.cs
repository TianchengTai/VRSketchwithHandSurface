using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRPainting;
using CommandUndoRedo;
using Leap;
using Leap.Unity;
using Leap.Unity.Space;
using NURBS;
using Leap.Unity.Interaction;
using static RecognitionHand;

public class DrawSurface : MonoBehaviour
{
    //虚物体
    GameObject VirtualSurface;

    public static GameObject DrawingBoard;
    public LeapXRServiceProvider provider;
    Controller controller = new Controller();
    public Material mat;  //物体的材质

    //--------------------------------------------------------------------------
    List<Vector3> thumbJoints;
    List<Vector3> indexJoints;
    List<Vector3> otherJoints;
    Vector3 palmPosition;
    Vector3 thumbDir;
    Vector3 indexDir;
    Vector3 otherDir;
    Vector3 palmDir;
    Vector3 wristPosition;

    GestureType gesture = GestureType.none;
    //-----------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        //create();
        //------------------------------------------------------
        thumbJoints = new List<Vector3>();
        indexJoints = new List<Vector3>();
        otherJoints = new List<Vector3>();
        palmPosition = new Vector3();
        thumbDir = new Vector3();
        otherDir = new Vector3();
        palmDir = new Vector3();
        wristPosition = new Vector3();

        gesture = GestureType.none;
        //gameObject.GetComponent<Renderer>().enabled = false;
        DrawingBoard = GameObject.Find("Drawing Board");
        if (mat == null)
        {
            mat = new Material(Shader.Find("Standard"));
        }
        //------------------------------------------------------       
    }




    float waitTime = 0f;
    float interval = 1f;
    public bool isCollider;

    // Update is called once per frame
    void Update()
    {
        Frame source = controller.Frame();
        Frame dest = new Frame();
        provider.transformFrame(source, dest);
        GetHandInfo(dest);

        waitTime += Time.deltaTime;
        if (dest.Hands.Count == 0) {
            if (VirtualSurface != null) {
                VirtualSurface.SetActive(false);
            }
        }
        else {
            foreach (var hand in dest.Hands) {
                if (hand.IsLeft) {
                    if (isCollider) {
                        if (VirtualSurface != null) {
                            VirtualSurface.SetActive(false);
                        }
                        break;
                    }
                    else{
                        if (VirtualSurface != null) {
                            VirtualSurface.SetActive(true);
                        }
                    }
                    if (VirtualSurface != null && VirtualSurface.activeSelf) {
                        SetPosition(VirtualSurface,gesture);
                    }
                    GestureType CurrGesture = RecognitionHand.recognizeHand(calculateHandAngles());
                    //Debug.Log(gesture.ToString());
                    if (waitTime > interval) {
                        if (VirtualSurface != null) {
                            Destroy(VirtualSurface);
                        }
                        VirtualSurface = CreateSurface(CurrGesture);
                        gesture = CurrGesture;
                        waitTime = 0;
                    }
                }

            }
        }
    }

    void GetHandInfo(Frame dest){
        foreach (var hand in dest.Hands)
        {
            if (hand.IsRight) {
                continue;
            }
            palmPosition = hand.PalmPosition.ToVector3();

            thumbJoints.Clear();
            indexJoints.Clear();
            otherJoints.Clear();

            List<Finger> others = hand.Fingers;
            Finger thumb = others[0];
            Finger index = others[1];
            others.RemoveAt(0);
            others.RemoveAt(0);
            thumbDir = thumb.Direction.ToVector3();
            indexDir = index.Direction.ToVector3();
            palmDir = hand.PalmNormal.ToVector3();
            wristPosition= hand.WristPosition.ToVector3();
            foreach (var item in others)
            {
                otherDir += item.Direction.ToVector3();
            }
            otherDir /= 3.0f;
            foreach (Bone.BoneType boneType in (Bone.BoneType[])Enum.GetValues(typeof(Bone.BoneType)))
            {
                try
                {
                    thumbJoints.Add(thumb.Bone(boneType).NextJoint.ToVector3());
                    indexJoints.Add(index.Bone(boneType).NextJoint.ToVector3());

                    Vector3 avg = new Vector3();
                    foreach (var other in others)
                    {
                        avg += other.Bone(boneType).NextJoint.ToVector3();
                    }
                    otherJoints.Add(avg / 3.0f);
                }
                catch (System.IndexOutOfRangeException) { };
            }
        }
    }

    // void create() {
    //     List<CP> cpList21 = new List<CP>();
    //     cpList21.Add(new CP(new Vector3(-0.5f, 0, 0), 1));
    //     cpList21.Add(new CP(new Vector3(0.5f, 0, 0), 1));

    //     List<CP> cpList22 = new List<CP>();
    //     cpList22.Add(new CP(new Vector3(0, 0, -0.5f), 1));
    //     cpList22.Add(new CP(new Vector3(0, 0, 0.5f), 1));

    //     //GameObject cone;
    //     //List<CP> cpList = new List<CP>() { new CP(new Vector3(1,0,0), 1), new CP(new Vector3(1,1,0), 1) }; //去中心化
    //     //Vector3 axis4 = new Vector3(0,1,0);  //轴的方向向量
    //     //float angle4 = 360;//旋转角
    //     //Pipeline.RotateAndRender(out cone, cpList, axis4, angle4, 2, 2, false);

    //     GameObject surface;
    //     Pipeline.PanAndRender(out surface, cpList21, cpList22, 2, 2, false, false);
    // }


    GameObject CreateSurface(GestureType gesture) {
        switch (gesture) {
            case GestureType.ping:
                GameObject plane;
                plane = CreatePlane();
                InitSurface(plane, "Plane", Color.grey);
                return plane;
            case GestureType.qu:
                GameObject surface;
                surface = CreateQuMian();
                InitSurface(surface, "Surface", Color.grey);
                return surface;
            case GestureType.zhu:
                GameObject cylinder;
                cylinder = CreateCylinder();
                InitSurface(cylinder, "Cylinder", Color.grey);
                return cylinder;
            case GestureType.zhui:
                GameObject cone;
                cone = CreateCone();
                InitSurface(cone, "Cone", Color.grey);
                return cone;
            case GestureType.qiu:
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                InitSurface(sphere, "Sphere", Color.grey);
                sphere.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                return sphere;
            default:
                return null;
        }
    }

    void SetPosition(GameObject surface,GestureType gestureType) {

        switch (gestureType) {
            case GestureType.ping:
                surface.transform.position = 3 * (indexJoints[3] - otherJoints[3]) + 3 * (indexJoints[3] - indexJoints[0]) + palmPosition;

                surface.transform.rotation = Quaternion.LookRotation(Vector3.Cross(otherJoints[0] - indexJoints[0], -palmDir), -palmDir);

                break;
            case GestureType.qiu:
                surface.transform.position = palmPosition;
                break;
            case GestureType.qu:
                surface.transform.position = -2*(indexJoints[3] - otherJoints[3]) - (indexJoints[3] - indexJoints[0]) + palmPosition;
                surface.transform.rotation = Quaternion.LookRotation(-wristPosition+(indexJoints[0]+otherJoints[0])/2, -palmDir);
                break;
            case GestureType.zhu:
                Vector3 axis1 = Vector3.Cross(indexJoints[0] - thumbJoints[2], indexJoints[1] - thumbJoints[0]);
                surface.transform.position = palmPosition;
                surface.transform.up = axis1;
                break;
            case GestureType.zhui:
                Vector3 axis = Vector3.Cross(indexJoints[0] - thumbJoints[2], indexJoints[1] - thumbJoints[0]);
                surface.transform.position = palmPosition;
                surface.transform.up = axis;
                break;
            default:
                surface.transform.position = palmPosition;
                surface.transform.forward = palmDir;
                break;
        }
    }

    GameObject CreatePlane() {
        GameObject plane = new GameObject();
        MeshFilter mf =plane.AddComponent<MeshFilter>();
        mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/plane.asset");
        plane.AddComponent<MeshRenderer>();
        plane.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        return plane;
    }


    GameObject CreateCylinder() {
        float angle = Vector3.Angle(otherJoints[3] - otherJoints[0], wristPosition - otherJoints[0]);
        float ratio = (Mathf.Max(angle, 80) - 60) / 60;
        GameObject cylinder = new GameObject();
        MeshFilter mf = cylinder.AddComponent<MeshFilter>();
        Debug.Log(angle);
        if (angle > 160) {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/zhu1.asset");
        }
        else if (angle > 145) {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/zhu2.asset");
        }
        else if (angle > 130) {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/zhu3.asset");

        }
        else {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/zhu4.asset");
        }
        cylinder.AddComponent<MeshRenderer>();
        cylinder.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        return cylinder;
    }


    GameObject CreateCone() {
        float angle = Vector3.Angle(otherJoints[3] - otherJoints[0], wristPosition - otherJoints[0]);
        float ratio = (Mathf.Max(angle,80) - 60) / 60;
        GameObject cone = new GameObject();
        MeshFilter mf = cone.AddComponent<MeshFilter>();
        Debug.Log(angle);
        if (angle > 160) {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/zhui1.asset");
        }
        else if (angle > 145) {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/zhui2.asset");
        }
        else if (angle > 130) {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/zhui3.asset");

        }
        else {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/zhui4.asset");
        }
        cone.AddComponent<MeshRenderer>();
        cone.transform.localScale = new Vector3(0.15f,0.15f,0.15f);
        return cone;



        
    }

    GameObject CreateQuMian() {
        GameObject cone = new GameObject();
        MeshFilter mf = cone.AddComponent<MeshFilter>();
        float angle = Vector3.Angle(indexJoints[0] - indexJoints[1], indexJoints[1] - indexJoints[3]);
        if (angle < 20) {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/bakedMesh1.asset");
        }
        else if (angle < 40) {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/bakedMesh2.asset");
        }
        else if (angle < 60) {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/bakedMesh3.asset");

        }
        else {
            mf.mesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/bakedMesh4.asset");
        }
        cone.AddComponent<MeshRenderer>();
        cone.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        return cone;
    }


    // 表面添加碰撞器、材质、颜色等基础信息
    void InitSurface(GameObject surface,string name,Color color)
    {
        if (surface.GetComponent<Collider>()) {
            Destroy(surface.GetComponent<Collider>());
        }
        MeshCollider MC = surface.AddComponent<MeshCollider>();
        MC.convex = true;
        surface.layer = LayerMask.NameToLayer("virtual_surface");
        surface.name = name;
        //surface.transform.position = position;
        surface.GetComponent<MeshRenderer>().material = mat;
        surface.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g,color.b,0.3f) ;
    }

    // 在实例化表面时调用，将表面layer调整，添加刚体、InteractionBehaviour、ManipulationHand等组件，并调整参数
    public void FinishSurface() {
        GameObject surface = VirtualSurface;
        AddInteractionComponent(surface);
        VirtualSurface = null;
        UndoRedoManager.Insert(new DrawCommand(surface));
        SelectModel.AddSurface(surface);
    }

    public static void AddInteractionComponent(GameObject surface){
        Rigidbody Rb = surface.AddComponent<Rigidbody>();
        Rb.useGravity = false;
        Rb.isKinematic = true;
        InteractionBehaviour IB = surface.AddComponent<InteractionBehaviour>();
        IB.manager = GameObject.Find("Interaction Manager").GetComponent<InteractionManager>();
        IB.ignoreContact = true;
        //IB.overrideNoContactLayer = true;
        surface.AddComponent<ManipulationHand>();
        surface.transform.SetParent(DrawingBoard.transform);
        surface.layer = LayerMask.NameToLayer("hidden_surface");
        // aglign and joint
        //surface.AddComponent<AlignAndJoint>();
    }

    public static void RemoveInteractionComponent(GameObject surface){
        ManipulationHand MH = surface.GetComponent<ManipulationHand>();
        Destroy(MH);
        InteractionBehaviour IB = surface.GetComponent<InteractionBehaviour>();
        Destroy(IB);
        Rigidbody Rb = surface.GetComponent<Rigidbody>();
        Destroy(Rb);
    }

    List<String> calculateHandAngles()
    {
        // logData.Add(new List<String>(){thumbDir.ToString(),indexDir.ToString(),otherDir.ToString(),palmDir.ToString(),
        return new List<String>(){
                ""+Vector3.Angle (thumbDir, indexDir),""+Vector3.Angle (thumbDir, otherDir),""+Vector3.Angle (indexDir, otherDir),
                ""+Vector3.Angle (thumbDir, palmDir),""+Vector3.Angle (indexDir, palmDir),""+Vector3.Angle (otherDir, palmDir)};
    }

    private List<CP> Line2CPList(LineRenderer line, Vector3 center, bool decentration = false)
    {
        List<CP> cpList = new List<CP>();
        for (int i = 0; i < line.positionCount; i++)
        {
            CP cp = new CP(decentration ? line.GetPosition(i) - center : line.GetPosition(i), 1);
            cpList.Add(cp);
        }
        return cpList;
    }


}