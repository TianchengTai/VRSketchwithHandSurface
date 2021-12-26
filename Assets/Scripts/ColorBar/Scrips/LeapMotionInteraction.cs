/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using UnityEngine.UI;

public class LeapMotionInteraction : MonoBehaviour
{
    public Transform[] bonesL;
    public Transform[] bonesR;
    public HandModelBase leftHand;
    public HandModelBase rightHand;

    List<HandModelBase> handModelList = new List<HandModelBase>();

    Ray ray;

    public Image Paint;//把color拖过来

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftHand != null && leftHand.IsTracked)
        {
            //如果伸出食指
            if (bonesL[1].position.z > bonesL[0].position.z
                && bonesL[1].position.z > bonesL[2].position.z
                && bonesL[1].position.z > bonesL[3].position.z
                && bonesL[1].position.z > bonesL[4].position.z)
            {
                interact();
            }           
        }
    }

    void interact()
    {
       

        Vector2 screenPos = Camera.main.WorldToScreenPoint(bonesR[1].position);
        ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit[] hit = Physics.RaycastAll(ray, 2000f, 1 << LayerMask.NameToLayer("UI")); ;
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                Debug.Log("检测到物体" + hit[i].collider.name);
                //hit[i].transform;
                //ChangeColor();
            }
        }
    }

   void ChangeColor(Color color)
   {
        Paint.color = color;
   }
}
*/