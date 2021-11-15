﻿
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorPick : MonoBehaviour
{

    public Image Saturation;//饱和度
    public Image Hue;//色泽度
    public Image Paint;//选中颜色

    public RectTransform Point_Stauration;
    public RectTransform Point_Hue;

    private Sprite Saturation_Sprite;
    private Sprite Hue_Sprite;

    private Color32 currentHue = Color.red;//初始颜色为红色


    private void Awake()
    {

    }

    private void Start()
    {
        UpdateStauration();//更新饱和度
        UpdateHue();//更新色泽度
    }
    
    private void UpdateStauration()
    {
        float sWidth = 200, sHeight = 200;
        Saturation_Sprite = Sprite.Create(new Texture2D((int)sWidth, (int)sHeight), new Rect(0, 0, sWidth, sHeight), new Vector2(0, 0));//纹理 纹理的哪一部分，中心在左下角
        
        //设置纹理的每一个像素
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

    private static Color GetSaturation(Color color, float x, float y)//x、y的范围是0-1
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
        float w = 50, h = 50;
        Hue_Sprite = Sprite.Create(new Texture2D((int)w, (int)h), new Rect(0, 0, w, h), new Vector2(0, 0));
        for (int y = 0; y <= h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                var pixColor = GetHue(y / h);
                Hue_Sprite.texture.SetPixel(x, ((int)h - y), pixColor);
            }
        }
        Hue_Sprite.texture.Apply();
        Hue.sprite = Hue_Sprite;
    }

    //B,r,G,b,R,g //大写是升，小写是降
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

    private Vector2 clickPoint = Vector2.zero;//（0，0）
    public void OnStaurationClick(ColorPickClick sender)
    {
        var size2 = Saturation.rectTransform.sizeDelta / 2;
        var pos = Vector2.zero;
        pos.x = Mathf.Clamp(sender.ClickPoint.x, -size2.x, size2.x);
        pos.y = Mathf.Clamp(sender.ClickPoint.y, -size2.y, size2.y);
        Point_Stauration.anchoredPosition = clickPoint = pos;

        UpdateColor();
    }

    public void OnHueClick(ColorPickClick sender)
    {
        var h = Hue.rectTransform.sizeDelta.y / 2.0f;
        var y = Mathf.Clamp(sender.ClickPoint.y, -h, h);
        Point_Hue.anchoredPosition = new Vector2(0, y);

        y += h;
        currentHue = GetHue(1 - y / Hue.rectTransform.sizeDelta.y);
        UpdateStauration();
        UpdateColor();
    }

    public void UpdateColor()//更新颜色
    {
        var size2 = Saturation.rectTransform.sizeDelta / 2;
        var pos = clickPoint;
        pos += size2;

        var color = GetSaturation(currentHue, pos.x / Saturation.rectTransform.sizeDelta.x, 1 - pos.y / Saturation.rectTransform.sizeDelta.y);
        Paint.color = color;
    }
}
