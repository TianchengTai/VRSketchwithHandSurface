using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap.Unity.Interaction;
using JointDirection;


public enum TouchAction
{
    Free,
    Pinching,
    Scaling
}
public class ManipulationHand : MonoBehaviour
{
    public LeapXRServiceProvider provider;
    private Controller controller;
    //private bool isPinched = false;
    //private bool isGrabbed = false;
    TouchAction currAction;
    private InteractionBehaviour interactionBehaviour;

    private float prevGrabDegree = 0.0f;
    private Vector3 prevPinchPosition;
    private Quaternion prevPinchQuaternion;

    const float deltaCloseFinger = 30.0f;
    const float deltaFarFinger = 50.0f;
    const float deltaCloseGrabDegree = 0.2f;

    void Start()
    {
        controller = new Controller();
        interactionBehaviour = GetComponent<InteractionBehaviour>();
        provider = GameObject.Find("Main Camera").GetComponent<LeapXRServiceProvider>();
        currAction = TouchAction.Free;
        interactionBehaviour.OnGraspStay = () =>
        {
            //Debug.Log(interactionBehaviour.moveObjectWhenGrasped);
            Frame source = controller.Frame();
            Frame dest = new Frame();
            provider.transformFrame(source, dest);
            foreach (Hand hand in dest.Hands) {
                if (hand.IsLeft) {
                    if (GetIsMaxPinch(hand)) {
                        //interactionBehaviour.ignoreGrasping = false;
                    }
                    else {
                        interactionBehaviour.ReleaseFromGrasp();
                        interactionBehaviour.ignoreGrasping = true;
                    }
                }
            }
        };
    }
 
    
    // 开始接触
    // void OnTriggerEnter(Collider collider) {
    //     if (!enabled) return;
    //     GameObject.Find("Draw Surface").GetComponent<DrawSurface>().isCollider = true;
    //     if (collider.tag.Equals("hand"))
    //         Debug.Log("开始接触");
    //     Frame source = controller.Frame();
    //     Frame dest = new Frame();
    //     provider.transformFrame(source, dest);
    //     foreach (Hand hand in dest.Hands) {
    //         if (hand.IsLeft) {
    //             prevGrabDegree = GetGrabDegree(hand);
    //         }
    //     }
    // }

    // 接触结束
    void OnTriggerExit(Collider collider) {
        GameObject.Find("Draw Surface").GetComponent<DrawSurface>().isCollider = false;
        if (!enabled) return;
        if (collider.tag.Equals("hand"))
            Debug.Log("接触结束");
        // isGrabbed = false;
        // isPinched = false;
        currAction = TouchAction.Free;
        prevGrabDegree = 0;
    }

    // 接触持续中
    void OnTriggerStay(Collider collider) {
        if (!enabled) return;
        GameObject.Find("Draw Surface").GetComponent<DrawSurface>().isCollider = true;
        //if (!collider.tag.Equals("hand")) return;
        Frame source = controller.Frame();
        Frame dest = new Frame();
        provider.transformFrame(source, dest);
        foreach (Hand hand in dest.Hands) {
            if (hand.IsLeft) {
                // 当前未确定动作
                if(currAction == TouchAction.Free){
                    // 先判断是否是缩放（抓的动作）
                    if(GetIsGrab(hand)){
                        currAction = TouchAction.Scaling;
                        return; //要返回，接触中不能改动作类型了
                    }
                    // 不是缩放则判断是否是捏取
                    if(GetIsPinch(hand)){
                        currAction = TouchAction.Pinching;
                        return;
                    }
                }
                // 如果是捏取动作
                else if(currAction == TouchAction.Pinching){
                    // 将组件设置为可抓取，其余判断交给上面的OnGraspStay()函数
                    interactionBehaviour.ignoreGrasping = false;
                    AutoAlignAndJoint();
                }
                // 如果是缩放动作
                else if(currAction==TouchAction.Scaling){
                    // 将组件设置为不可抓取
                    interactionBehaviour.ignoreGrasping = true;
                    // 进行缩放操作
                    Scale(hand);
                    AutoAlignAndJoint();
                }

                // if (GetIsPinch(hand)) {
                //     if (isPinched) {
                //         isGrabbed = false;
                //         interactionBehaviour.ignoreGrasping = false;
                //         isPinched = GetIsMaxPinch(hand);
                //     }
                //     else {
                //         interactionBehaviour.ReleaseFromGrasp();
                //         interactionBehaviour.ignoreGrasping = true;
                //         isPinched = GetIsMinPinch(hand);
                //     }
                // }
                // else {
                //     if (isPinched) {
                //         interactionBehaviour.ReleaseFromGrasp();
                //         interactionBehaviour.ignoreGrasping = true;
                //         isPinched = false;
                //     }
                //     if (isGrabbed) {
                //         Scale(hand);
                //     }
                //     else {
                //         isGrabbed = GetIsGrab(hand);
                //     }
                // }
            }
        }
    }



 
    /// <summary>
    /// 缩放
    /// </summary>
    public void Scale(Hand hand)
    {
        // 如果prev 还没有设定
        if (prevGrabDegree==0) {
            prevGrabDegree = GetGrabDegree(hand);
            return ;
        }
        float currGrabDegree = GetGrabDegree(hand);
        if (Mathf.Abs( currGrabDegree - prevGrabDegree) <= deltaCloseGrabDegree) {
            return;
        }
        Vector3 value = transform.localScale;
        float rate =  0.5f * (prevGrabDegree - currGrabDegree);
        transform.localScale += new Vector3(value.x * rate, value.y * rate, value.z * rate);
        prevGrabDegree = GetGrabDegree(hand);
    }

    // TODO: 验证判定条件是否合理
    public bool GetIsGrab(Hand hand) {
        // float indexMag = (hand.Fingers[1].TipPosition - hand.Fingers[0].TipPosition).Magnitude;
        // float midMag = (hand.Fingers[2].TipPosition - hand.Fingers[0].TipPosition).Magnitude;
        // //Debug.Log(midMag - indexMag);
        // return hand.PinchDistance >= deltaCloseFinger && midMag >= indexMag && midMag - indexMag <= deltaCloseFinger;
        float indexMag = (hand.Fingers[1].TipPosition - hand.PalmPosition).Magnitude;
        float midMag = (hand.Fingers[2].TipPosition - hand.PalmPosition).Magnitude;
        return midMag >= 1.01f*indexMag;
    }

    public bool GetIsMinPinch(Hand hand){
        //Debug.Log(hand.PinchDistance);
        float indexMag = (hand.Fingers[1].TipPosition - hand.PalmPosition).Magnitude;
        float midMag = (hand.Fingers[2].TipPosition - hand.PalmPosition).Magnitude;
//        float ringMag = (hand.Fingers[3].TipPosition - hand.PalmPosition).Magnitude;
//        float pinkyMag = (hand.Fingers[4].TipPosition - hand.PalmPosition).Magnitude;
        return hand.PinchDistance <= deltaCloseFinger;

    }

    // TODO: 验证判定条件是否合理
    public bool GetIsPinch(Hand hand) {
        float indexMag = (hand.Fingers[1].TipPosition - hand.PalmPosition).Magnitude;
        float midMag = (hand.Fingers[2].TipPosition - hand.PalmPosition).Magnitude;
        //        float ringMag = (hand.Fingers[3].TipPosition - hand.PalmPosition).Magnitude;
        //        float pinkyMag = (hand.Fingers[4].TipPosition - hand.PalmPosition).Magnitude;
        return midMag <= indexMag;

    }

    public bool GetIsMaxPinch(Hand hand) {
        //Debug.Log(hand.PinchDistance);
        //float indexMag = (hand.Fingers[1].TipPosition - hand.PalmPosition).Magnitude;
        //float midMag = (hand.Fingers[2].TipPosition - hand.PalmPosition).Magnitude;
        //        float ringMag = (hand.Fingers[3].TipPosition - hand.PalmPosition).Magnitude;
        //        float pinkyMag = (hand.Fingers[4].TipPosition - hand.PalmPosition).Magnitude;
        return hand.PinchDistance <= deltaFarFinger;

    }

    public float GetGrabDegree(Hand hand){
        return hand.GrabStrength;//0-open,1-fist
    }

    // align and joint
    private void AutoAlignAndJoint(){
        Collider[] hitColliders=new Collider[10];
        int no_Collider = Physics.OverlapSphereNonAlloc(transform.position, 0.15f, hitColliders,LayerMask.GetMask("hidden_surface"));
        if (hitColliders.Length>1){
            foreach(Collider collider in hitColliders){
                if (collider!=null&&collider.transform!=transform&&!collider.transform.IsChildOf(transform)){
                    align(collider.transform,transform);
                    joint(collider.transform,transform);
                    break;
                }
            }
        }
    }

    private void align(Transform aimTransform, Transform curTransform)
    {
        bool sameUp = Vector3.Dot(curTransform.up, aimTransform.up) >=0.0f;
        curTransform.rotation = aimTransform.rotation;
        if (!sameUp)
        {
            curTransform.RotateAround(curTransform.position, curTransform.forward, 180.0f);
        }
    }
    private void joint(Transform aimTransform, Transform curTransform)
    {
        Vector3 aim2cur=(curTransform.position - aimTransform.position).normalized;
        curTransform.position = aimTransform.position;
        MoveDirection[] normalDirections = { new MoveUp(aimTransform.up.normalized),new MoveRight(aimTransform.right.normalized), new MoveForward(aimTransform.forward.normalized)
                , new MoveDown(-aimTransform.up.normalized),new MoveRight(-aimTransform.right.normalized),  new MoveForward(-aimTransform.forward.normalized )};
        int translateDirIndex = 0;
        float maxDot = 0.0f;
        for(int i=0;i<normalDirections.Length;i++)
        {
            float tempDot = Vector3.Dot(aim2cur, normalDirections[i].getDirection());
            if (tempDot>0&&tempDot >= maxDot)
            {
                maxDot = tempDot;
                translateDirIndex = i;
            }     
        }
        normalDirections[translateDirIndex].move(aimTransform, curTransform);
    }
    
}