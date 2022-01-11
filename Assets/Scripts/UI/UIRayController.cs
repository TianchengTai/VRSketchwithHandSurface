using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class UIRayController : MonoBehaviour
{

    public SteamVR_Behaviour_Pose pose;

    public SteamVR_Action_Boolean InteractUI = SteamVR_Input.GetBooleanAction("InteractUI");

    public float dist;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray raycast = new Ray(pose.transform.position, pose.transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit, dist,LayerMask.GetMask("UI"));
        if (bHit&&InteractUI.GetStateDown(pose.inputSource)){
            Debug.Log("hit "+hit.collider.name);
            hit.collider.GetComponent<EventTripple>().Activate();
        }
    }
}
