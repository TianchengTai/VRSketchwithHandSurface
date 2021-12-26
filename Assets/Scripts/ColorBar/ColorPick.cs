
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorPick : MonoBehaviour
{

    public Image Saturation;//饱和度 亮度or明度
    public Image Hue;//色泽度
    public Image Paint;//颜色

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
    
    //初始化Stauration
    private void UpdateStauration()
    {
        //新建一个像素图，宽200，高200
        float sWidth = 200, sHeight = 200;
        Saturation_Sprite = Sprite.Create(new Texture2D((int)sWidth, (int)sHeight), new Rect(0, 0, sWidth, sHeight), new Vector2(0, 0));//纹理 纹理的哪一部分，中心在左下角
        
        //设置纹理的每一个像素
        for (int y = 0; y <= sHeight; y++)
        {
            for (int x = 0; x < sWidth; x++)
            {
                var pixColor = GetSaturation(currentHue, x / sWidth, y / sHeight);//当前的色泽度，当前位置
                Saturation_Sprite.texture.SetPixel(x, ((int)sHeight - y), pixColor);
            }
        }
        Saturation_Sprite.texture.Apply();
        Saturation.sprite = Saturation_Sprite;//为image的sprite组件赋值
    }

    //获取饱和度，返回一个新的颜色
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

    //初始化Hue
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
    private readonly static float c = 1.0f / hues.Length; // 1/6

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



    private Vector2 clickPoint = Vector2.zero;//（0，0）
    public void OnStaurationClick(ColorPickClick sender)
    {
        var size2 = Saturation.rectTransform.sizeDelta / 2; 
        var pos = Vector2.zero;//位置（0，0）
        pos.x = Mathf.Clamp(sender.ClickPoint.x, -size2.x, size2.x);//value的值、最小值、最大值，限制value的值在min和max之间
        pos.y = Mathf.Clamp(sender.ClickPoint.y, -size2.y, size2.y);
        Point_Stauration.anchoredPosition = clickPoint = pos;//修改锚点的位置

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
        //锚点集中在一起
        //RectTransform rect = transform.GetComponent<RectTransform>();
        //1）通过sizeDelta
        //rect.sizeDelta
        //2）通过rect
        //rect.rect.size
        //它还有两个分别表示宽高的属性也可以使用
        //rect.rect.height
        //rect.rect.width
        var pos = clickPoint;//点击的位置，现在要搞清除这个clickPoint是啥
        pos += size2;//加上Saturation的尺寸

        var color = GetSaturation(currentHue, pos.x / Saturation.rectTransform.sizeDelta.x, 1 - pos.y / Saturation.rectTransform.sizeDelta.y);//计算color
        
        Paint.color = color;//修改颜色
    }
}
