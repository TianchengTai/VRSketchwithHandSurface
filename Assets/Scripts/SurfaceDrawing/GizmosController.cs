using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using RuntimeGizmos;

namespace VRPainting
{
    public class GizmosController : MonoBehaviour
    {
        public SteamVR_Behaviour_Pose left_pose;

        public SteamVR_Behaviour_Pose right_pose;

        public TransformGizmo transformGizmo;

        public SteamVR_Action_Boolean LeftPress = SteamVR_Input.GetBooleanAction("SnapTurnLeft");

        public SteamVR_Action_Boolean RightPress = SteamVR_Input.GetBooleanAction("SnapTurnRight");

        public SteamVR_Action_Boolean UpPress = SteamVR_Input.GetBooleanAction("SnapTurnUp");

        public SteamVR_Action_Boolean DownPress = SteamVR_Input.GetBooleanAction("SnapTurnDown");

        public SteamVR_Action_Boolean menu = SteamVR_Input.GetBooleanAction("menu");

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (UpPress.GetStateDown(left_pose.inputSource))
            {
                transformGizmo.enabled = !transformGizmo.enabled;
            }
            if (transformGizmo.enabled)
            {
                if (LeftPress.GetStateDown(left_pose.inputSource))
                {
                    transformGizmo.lastType();
                }
                if (RightPress.GetStateDown(right_pose.inputSource))
                {
                    transformGizmo.nextType();
                }
            }
        }
    }
}
