using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
public class ColorPickClick : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Button.ButtonClickedEvent Click;

    public Vector3 ClickPoint { get; set; }

    public void OnDrag(PointerEventData eventData)
    {
        var rect = transform as RectTransform;
        ClickPoint = rect.InverseTransformPoint(eventData.position);//根据当前分辨率的屏幕坐标，左下角为原点（0，0），右上角为屏幕宽高
        Debug.Log(ClickPoint);
        Click.Invoke();//触发事件
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var rect = transform as RectTransform;
        ClickPoint = rect.InverseTransformPoint(eventData.position);//ClickPoint是eventData相对于rect的坐标
        Debug.Log(ClickPoint);
        Click.Invoke();//触发事件
    }
}