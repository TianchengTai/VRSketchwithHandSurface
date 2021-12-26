using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

//分别挂在Saturation和hui上
//主要是算了一个ClickPoint
public class ColorPickClick : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Button.ButtonClickedEvent Click;

    public Vector3 ClickPoint { get; set; }

    public void OnDrag(PointerEventData eventData)
    {
        //as运算符：转换不成功得到一个null值，不会抛出异常；仅适用于引用类型，不能用于值类型

        //一个Transform组件的变换是有“参考系”的，如果该Transform有父级，则该Transform的Position、Rotation、Scale的变换，是相对于父级的；反之，如果没有父级，则默认为世界坐标系。
        //GUI坐标，左上（0，0），右下屏幕分辨率
        //Vieport坐标，相机占屏幕，左下（0，0），右上（1，1）
        //屏幕坐标，左下（0，0），右上屏幕分辨率，gameObject.z = camera.z
        //世界坐标
        var rect = transform as RectTransform;//Saturation的RectTransform
        Debug.Log(transform);
        ClickPoint = rect.InverseTransformPoint(eventData.position);
        //根据当前分辨率的屏幕坐标，左下角为原点（0，0），右上角为屏幕宽高
        //将position变换到Saturation的本地空间，eventData相对于物体rect的位置
        Click.Invoke();//激活Click方法
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var rect = transform as RectTransform;//Saturation的位置
        ClickPoint = rect.InverseTransformPoint(eventData.position);//ClickPoint是eventData相对于rect的坐标
        Debug.Log(ClickPoint);
        Click.Invoke();//触发事件
    }
}