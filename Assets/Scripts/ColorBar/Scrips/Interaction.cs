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
        UpdateStauration();//���±��Ͷ�
        UpdateHue();//����ɫ���
        Debug.Log(GetSaturation(Color.red,0,0.5f));//(0.5, 0.5, 0.5)
    }

    private void UpdateStauration()
    {
        //�½�һ������ͼ����200����200
        float sWidth = 200, sHeight = 200;
        Sprite Saturation_Sprite = Sprite.Create(new Texture2D((int)sWidth, (int)sHeight), new Rect(0, 0, sWidth, sHeight), new Vector2(0, 0));//���� �������һ���֣����������½�

        //���������ÿһ������
        for (int y = 0; y <= sHeight; y++)
        {
            for (int x = 0; x < sWidth; x++)
            {
                var pixColor = GetSaturation(currentHue, x / sWidth, y / sHeight);//��ǰ��ɫ��ȣ���ǰλ��
                Saturation_Sprite.texture.SetPixel(x, ((int)sHeight - y), pixColor);//����Ҫ�����SetPixel���x��y��ɶ
            }
        }
        Saturation_Sprite.texture.Apply();
        Saturation.sprite = Saturation_Sprite;//Ϊimage��sprite�����ֵ
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
                //����һ������Ray�����������Ļָ��һ����
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//���ߵ��������ߵķ���,���ߵķ���������ʱ���δ��λ����Unity 3D���Զ����е�λ��һ������
                //Ray Camera.main.ViewportPointToRay(Vector3 pos)  ����һ������Ray����������ӿڣ��ӿ�֮����Ч��ָ��һ����
                RaycastHit hit;//RaycastHit�����ڴ洢�������ߺ��������ײ��Ϣ��
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.green, 10);//����ײ�������position����
                                                                           //Debug.Log("ray.origin" + ray.origin);(0.0, 0.0, 1.7)
                                                                           //Debug.Log("hit.point" + hit.point);(0.2, 0.0, 0.5)
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, RectTransformUtility.WorldToScreenPoint(Camera.main, hit.point), Camera.main,out localPoint);
                    Debug.Log(localPoint);
                    var size2 = Saturation.rectTransform.sizeDelta / 2;
                    var pos = localPoint;//�����λ�ã�����Ҫ��������clickPoint��ɶ
                    pos += size2;//����Saturation�ĳߴ�
                    var color = GetSaturation(currentHue, pos.x / Saturation.rectTransform.sizeDelta.x, 1 - pos.y / Saturation.rectTransform.sizeDelta.y);//����color

                    Paint.color = color;//�޸���ɫ
                }           
        }
    }

    private static Color GetSaturation(Color color, float x, float y)//x��y�ķ�Χ��0-1
    {
        Color newColor = Color.white;//��ʼ������ɫΪ��ɫ
        for (int i = 0; i < 3; i++)//����RGB
        {
            //����R��˵���������1�����ǲ���Ĭ��ֵ
            if (color[i] != 1)
            {
                newColor[i] = (1 - color[i]) * (1 - x) + color[i];
            }
        }
        newColor *= (1 - y);//����
        newColor.a = 1;//��͸��
        return newColor;
    }

    private readonly static int[] hues = new int[] { 2, 0, 1, 2, 0, 1 };
    private readonly static Color[] colors = new Color[] { Color.red, Color.blue, Color.blue, Color.green, Color.green, Color.red };
    private readonly static float c = 1.0f / hues.Length;
    private static Color GetHue(float y)
    {
        y = Mathf.Clamp01(y); //����value��0~1֮�䲢����value�����valueС��0������0�����value����1,����1�����򷵻�value
        var index = (int)(y / c);//0*6=6;0.5*6=3,1*6=6;
        var h = hues[index];//��һ������
        var newColor = colors[index];//0-5��һ����ɫ
        float less = (y - index * c) / c;
        newColor[h] = index % 2 == 0 ? less : 1 - less;//�����ż����less�������������1-less��
        return newColor;
    }
}
