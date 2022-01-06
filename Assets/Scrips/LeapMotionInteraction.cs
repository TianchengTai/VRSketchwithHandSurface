using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using UnityEngine.UI;

public class LeapMotionInteraction : MonoBehaviour
{

    public RectTransform canvas;

    public Image Saturation;
    public Image Hue;
    public Image Paint;

    private Color32 currentHue = Color.red;

    public Transform[] bonesL;
    public Transform[] bonesR;
    public HandModelBase leftHand;
    public HandModelBase rightHand;

    List<HandModelBase> handModelList = new List<HandModelBase>();
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        UpdateStauration();
        UpdateHue();
        Debug.Log(GetSaturation(Color.red, 0, 0.5f));//(0.5, 0.5, 0.5)
    }

    private void UpdateStauration()
    {
        float sWidth = 200, sHeight = 200;
        Sprite Saturation_Sprite = Sprite.Create(new Texture2D((int)sWidth, (int)sHeight), new Rect(0, 0, sWidth, sHeight), new Vector2(0, 0));//���� ��������һ���֣����������½�

        for (int y = 0; y <= sHeight; y++)
        {
            for (int x = 0; x < sWidth; x++)
            {
                var pixColor = GetSaturation(currentHue, x / sWidth, y / sHeight);
                Saturation_Sprite.texture.SetPixel(x, ((int)sHeight - y), pixColor);
            }
        }
        Saturation_Sprite.texture.Apply();
        Saturation.sprite = Saturation_Sprite;
    }

    private void UpdateHue()
    {
        float hWidth = 50, hHeight = 50;
        Sprite Hue_Sprite = Sprite.Create(new Texture2D((int)hWidth, (int)hHeight), new Rect(0, 0, hWidth, hHeight), new Vector2(0, 0));
        for (int y = 0; y <= hHeight; y++)
        {
            for (int x = 0; x < hWidth; x++)
            {
                var pixColor = GetHue(y / hHeight);
                Hue_Sprite.texture.SetPixel(x, ((int)hHeight - y), pixColor);
            }
        }
        Hue_Sprite.texture.Apply();
        Hue.sprite = Hue_Sprite;
    }

    private static Color GetSaturation(Color color, float x, float y)
    {
        Color newColor = Color.white;
        for (int i = 0; i < 3; i++)
        {
            if (color[i] != 1)
            {
                newColor[i] = (1 - color[i]) * (1 - x) + color[i];
            }
        }
        newColor *= (1 - y);
        newColor.a = 1;
        return newColor;
    }

    private readonly static int[] hues = new int[] { 2, 0, 1, 2, 0, 1 };
    private readonly static Color[] colors = new Color[] { Color.red, Color.blue, Color.blue, Color.green, Color.green, Color.red };
    private readonly static float c = 1.0f / hues.Length;
    private static Color GetHue(float y)
    {
        y = Mathf.Clamp01(y); 
        var index = (int)(y / c);//0*6=6;0.5*6=3,1*6=6;
        var h = hues[index];
        var newColor = colors[index];
        float less = (y - index * c) / c;
        newColor[h] = index % 2 == 0 ? less : 1 - less;
        return newColor;
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer lr = gameObject.GetComponent<LineRenderer>();
        if (leftHand != null && leftHand.IsTracked)
        {
            if (bonesL[1].position.z > bonesL[0].position.z
                && bonesL[1].position.z > bonesL[2].position.z
                && bonesL[1].position.z > bonesL[3].position.z
                && bonesL[1].position.z > bonesL[4].position.z)
            {
                
                if(lr==null){
                    lr=gameObject.AddComponent<LineRenderer>();
                }
                lr.enabled =true;
                lr.positionCount=2;
                lr.SetPosition(0,bonesL[5].position);
                lr.SetPosition(1,bonesL[5].position +  (bonesL[1].position - bonesL[5].position).normalized*0.2f);
                lr.startWidth=0.01f;
                lr.endWidth=0.01f;
                lr.material=new Material(Shader.Find("Sprites/Default"));
                lr.material.color = Color.green;
                // lr.startColor=Color.green;
                // lr.endColor=Color.green;      
                interact();
            }else{
                if(lr!=null){
                    lr.enabled =false;
                }
            }           
        }
    }

    void interact()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(bonesR[1].position);//左手食指指尖
        Ray ray = new Ray(bonesL[5].position,bonesL[1].position-bonesL[5].position);
        //ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit[] hit = Physics.RaycastAll(ray,0.2f, 1 << LayerMask.NameToLayer("UI")); ;
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                // Debug.Log("Collider" + hit[i].collider.name);
                // Debug.DrawLine(ray.origin, hit[i].point, Color.green, 10);             
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, RectTransformUtility.WorldToScreenPoint(Camera.main, hit[i].point), Camera.main, out localPoint);
                Debug.Log(localPoint);//在画布上的坐标
                var pos = localPoint;
                var size2 = Saturation.rectTransform.sizeDelta / 2;//饱和度面板的中心                
                pos += size2;
                var color = GetHue(0.5f);
                color = GetSaturation(currentHue, pos.x / Saturation.rectTransform.sizeDelta.x, 1 - pos.y / Saturation.rectTransform.sizeDelta.y);
                Paint.color = color;
            }
        }
    }
}