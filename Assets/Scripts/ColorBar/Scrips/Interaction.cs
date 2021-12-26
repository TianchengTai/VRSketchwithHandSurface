using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public RectTransform canvas;

    public Image Saturation;
    public Image Hue;
    public Image Paint;

    private Color32 currentHue = Color.red;
    // Start is called before the first frame update
    void Start()
    {
        UpdateStauration();//更新饱和度
        UpdateHue();//更新色泽度
        Debug.Log(GetSaturation(Color.red,0,0.5f));//(0.5, 0.5, 0.5)
    }

    private void UpdateStauration()
    {
        //新建一个像素图，宽200，高200
        float sWidth = 200, sHeight = 200;
        Sprite Saturation_Sprite = Sprite.Create(new Texture2D((int)sWidth, (int)sHeight), new Rect(0, 0, sWidth, sHeight), new Vector2(0, 0));//纹理 纹理的哪一部分，中心在左下角

        //设置纹理的每一个像素
        for (int y = 0; y <= sHeight; y++)
        {
            for (int x = 0; x < sWidth; x++)
            {
                var pixColor = GetSaturation(currentHue, x / sWidth, y / sHeight);//当前的色泽度，当前位置
                Saturation_Sprite.texture.SetPixel(x, ((int)sHeight - y), pixColor);//明天要搞清楚SetPixel这个x，y是啥
            }
        }
        Saturation_Sprite.texture.Apply();
        Saturation.sprite = Saturation_Sprite;//为image的sprite组件赋值
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

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                //返回一条射线Ray从摄像机到屏幕指定一个点
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线的起点和射线的方向,射线的方向在设置时如果未单位化，Unity 3D会自动进行单位归一化处理
                //Ray Camera.main.ViewportPointToRay(Vector3 pos)  返回一条射线Ray从摄像机到视口（视口之外无效）指定一个点
                RaycastHit hit;//RaycastHit类用于存储发射射线后产生的碰撞信息。
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.green, 10);//与碰撞器交点的position坐标
                                                                           //Debug.Log("ray.origin" + ray.origin);(0.0, 0.0, 1.7)
                                                                           //Debug.Log("hit.point" + hit.point);(0.2, 0.0, 0.5)
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, RectTransformUtility.WorldToScreenPoint(Camera.main, hit.point), Camera.main,out localPoint);
                    Debug.Log(localPoint);
                    var size2 = Saturation.rectTransform.sizeDelta / 2;
                    var pos = localPoint;//点击的位置，现在要搞清除这个clickPoint是啥
                    pos += size2;//加上Saturation的尺寸
                    var color = GetSaturation(currentHue, pos.x / Saturation.rectTransform.sizeDelta.x, 1 - pos.y / Saturation.rectTransform.sizeDelta.y);//计算color

                    Paint.color = color;//修改颜色
                }           
        }
    }

    private static Color GetSaturation(Color color, float x, float y)//x、y的范围是0-1
    {
        Color newColor = Color.white;//初始化新颜色为白色
        for (int i = 0; i < 3; i++)//更新RGB
        {
            //对于R来说，如果不是1，就是不是默认值
            if (color[i] != 1)
            {
                newColor[i] = (1 - color[i]) * (1 - x) + color[i];
            }
        }
        newColor *= (1 - y);//亮度
        newColor.a = 1;//不透明
        return newColor;
    }

    private readonly static int[] hues = new int[] { 2, 0, 1, 2, 0, 1 };
    private readonly static Color[] colors = new Color[] { Color.red, Color.blue, Color.blue, Color.green, Color.green, Color.red };
    private readonly static float c = 1.0f / hues.Length;
    private static Color GetHue(float y)
    {
        y = Mathf.Clamp01(y); //限制value在0~1之间并返回value。如果value小于0，返回0。如果value大于1,返回1，否则返回value
        var index = (int)(y / c);//0*6=6;0.5*6=3,1*6=6;
        var h = hues[index];//哪一个分量
        var newColor = colors[index];//0-5哪一种颜色
        float less = (y - index * c) / c;
        newColor[h] = index % 2 == 0 ? less : 1 - less;//如果是偶数，less；如果是奇数，1-less；
        return newColor;
    }
}
