using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace VRPainting
{
    public class DoubleHandsScaling : MonoBehaviour
    {
        public SteamVR_Behaviour_Pose left_pose;

        public SteamVR_Behaviour_Pose right_pose;

        public GameObject FacingObject;
        public GameObject PointObject;

        public Vector3 left_posi;

        public Vector3 right_posi;

        public Vector3 FacingObjectSize;

        public Quaternion FacingObjectRotation;

        public Vector3 PointObjectPosition;
        public SteamVR_Action_Boolean LeftBtn = SteamVR_Input.GetBooleanAction("SideBtn");
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            GetFacingObject();
            GetPointObject();
            if (FacingObject!=null&& LeftBtn.GetStateDown(left_pose.inputSource) && LeftBtn.GetState(right_pose.inputSource)
            || LeftBtn.GetStateDown(left_pose.inputSource) && LeftBtn.GetStateDown(right_pose.inputSource)
            || LeftBtn.GetState(left_pose.inputSource) && LeftBtn.GetStateDown(right_pose.inputSource))
            {
                left_posi = left_pose.transform.position;
                right_posi = right_pose.transform.position;
                FacingObjectSize = FacingObject.transform.localScale;
                FacingObjectRotation = FacingObject.transform.rotation;
            }
            else if (FacingObject!=null && LeftBtn.GetState(left_pose.inputSource) && LeftBtn.GetState(right_pose.inputSource))
            {
                Vector3 curr_left_posi = left_pose.transform.position;
                Vector3 curr_right_posi = right_pose.transform.position;
                Vector3 curr_left2right = curr_right_posi - curr_left_posi;
                Vector3 prev_left2right = right_posi - left_posi;
                FacingObject.transform.localScale = FacingObjectSize * curr_left2right.sqrMagnitude / prev_left2right.sqrMagnitude;
                FacingObject.transform.rotation = Quaternion.FromToRotation(prev_left2right, curr_left2right) * FacingObjectRotation;
            }


            if (PointObject!=null && LeftBtn.GetStateDown(left_pose.inputSource)){
                left_posi = left_pose.transform.position;
                PointObjectPosition = PointObject.transform.position;
            }
            else if(PointObject!=null && LeftBtn.GetState(left_pose.inputSource) && !LeftBtn.GetState(right_pose.inputSource)){
                PointObject.transform.position = left_pose.transform.position - left_posi + PointObjectPosition;
            }
        }

        void GetFacingObject(){
            Ray ray = new Ray(Camera.main.transform.position,Camera.main.transform.forward);
            // LineRenderer lr = gameObject.GetComponent<LineRenderer>();
            // if(lr==null){
            //     lr = gameObject.AddComponent<LineRenderer>();
            // }
            // lr.positionCount = 2;
            // lr.SetPosition(0,Camera.main.transform.position);
            // lr.SetPosition(1,Camera.main.transform.position+Camera.main.transform.forward*100);        
            // lr.material=new Material(Shader.Find("Sprites/Default"));
            // lr.startWidth=0.01f;
            // lr.endWidth=0.01f;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1)){
                FacingObject = hit.transform.gameObject;
                // lr.SetPosition(1,hit.point);
            }
            else{
                FacingObject = null;
            }
        }

        void GetPointObject(){
            Ray ray = new Ray(left_pose.transform.position,left_pose.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 0.1f)){
                PointObject = hit.transform.gameObject;
            }
            else{
                PointObject = null;
            }
        }
    }
}
