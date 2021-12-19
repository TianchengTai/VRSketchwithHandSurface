using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace VRPainting{
    public class DoubleHandsScaling : MonoBehaviour
{
        public SteamVR_Behaviour_Pose left_pose;

        public SteamVR_Behaviour_Pose right_pose;

        public GameObject DrawingBoard;

        public Vector3 left_posi;

        public Vector3 right_posi;

        public Vector3 BoardSize;

        public Vector3 BoardPosition;
        public SteamVR_Action_Boolean LeftBtn = SteamVR_Input.GetBooleanAction("SideBtn");
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(LeftBtn.GetStateDown(left_pose.inputSource)&&LeftBtn.GetState(right_pose.inputSource)
            || LeftBtn.GetStateDown(left_pose.inputSource)&&LeftBtn.GetStateDown(right_pose.inputSource)
            || LeftBtn.GetState(left_pose.inputSource)&&LeftBtn.GetStateDown(right_pose.inputSource)){
                left_posi = left_pose.transform.position;
                right_posi = right_pose.transform.position;
                BoardSize = DrawingBoard.transform.localScale;
                BoardPosition = DrawingBoard.transform.position;
                return;
            }
            if(LeftBtn.GetState(left_pose.inputSource)&&LeftBtn.GetState(right_pose.inputSource)){
                Vector3 curr_left_posi = left_pose.transform.position;
                Vector3 curr_right_posi = right_pose.transform.position;
                Vector3 curr_left2right = curr_right_posi-curr_left_posi;
                Vector3 prev_left2right = right_posi-left_posi;
                DrawingBoard.transform.localScale=BoardSize*curr_left2right.sqrMagnitude/prev_left2right.sqrMagnitude;
                DrawingBoard.transform.position = Quaternion.FromToRotation(prev_left2right,curr_left2right)*BoardPosition;
            }
        }
}
}
