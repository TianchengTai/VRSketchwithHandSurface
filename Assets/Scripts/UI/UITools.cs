using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��UI�����Ĺ���
/// </summary>
public class UITools : MonoBehaviour
{
    public void SetGameObjectInActive(GameObject sender)
    {
        sender.SetActive(false);
    }

    public void SetGameObjectActive(GameObject sender)
    {
        sender.SetActive(true);
    }

}
