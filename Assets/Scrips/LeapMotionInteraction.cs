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

     public Image Paint;//把color拖过来

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
        Debug.Log(GetSaturation(Color.red, 0, 0.5f));
    }

    private Color32 currentHue = Color.red;
    private void UpdateStauration()
    {
        float sWidth = 200, sHeight = 200;
        Sprite Saturation_Sprite = Sprite.Create(new Texture2D((int)sWidth, (int)sHeight), new Rect(0, 0, sWidth, sHeight), new Vector2(0, 0));
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

    private readonly static int[] hues = new int[] { 2, 0, 1, 2, 0, 1 };
    private readonly static Color[] colors = new Color[] { Color.red, Color.blue, Color.blue, Color.green, Color.green, Color.red };
    private readonly static float c = 1.0f / hues.Length;
    private static Color GetHue(float y)
    {
        y = Mathf.Clamp01(y); 
        var index = (int)(y / c);
        var h = hues[index];
        var newColor = colors[index];
        float less = (y - index * c) / c;
        newColor[h] = index % 2 == 0 ? less : 1 - less;
        return newColor;
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
            }
            Debug.DrawLine(ray.origin, hit[0].point, Color.green, 10);
            //Debug.Log("ray.origin" + ray.origin);(0.0, 0.0, 1.7)
            //Debug.Log("hit.point" + hit.point);(0.2, 0.0, 0.5)
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, RectTransformUtility.WorldToScreenPoint(Camera.main, hit[0].point), Camera.main, out localPoint);
            Debug.Log(localPoint);
            var size2 = Saturation.rectTransform.sizeDelta / 2;
            var pos = localPoint;
            pos += size2;
            var color = GetSaturation(currentHue, pos.x / Saturation.rectTransform.sizeDelta.x, 1 - pos.y / Saturation.rectTransform.sizeDelta.y);
            ChangeColor(color);
        }
    }

   void ChangeColor(Color color)
   {
        Paint.color = color;
   }
}