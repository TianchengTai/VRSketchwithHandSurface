using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTools
{

    //����ռ�������ֱ�ߵĽ���
    public static Vector3 calculateIntersection(List<Vector3> line1, List<Vector3> line2)
    {
        Vector3 p1 = line1[0];//��һ��ֱ�ߵĵ�һ����
        Vector3 p2 = line1[1];//��һ��ֱ�ߵĵڶ�����
        Vector3 a = p1 - p2;

        Vector3 p3 = line2[0];//�ڶ���ֱ�ߵĵ�һ����
        Vector3 p4 = line2[1];//�ڶ���ֱ�ߵĵڶ�����
        Vector3 b = p3 - p4;

        //����Ƿ���Թ�������ֱ��
        if(p1.Equals(p2) || p3.Equals(p4)){
            Debug.Log("���ܹ���ֱ��");
        }
        //�ж�����ֱ���Ƿ�ƽ��,���������Ĳ���Ƿ�Ϊ0
        if (Vector3.Cross(a,b).Equals(new Vector3(0,0,0)))
        {
            Debug.Log("����ֱ��ƽ��");
        }

        //�жϸ��������Ƿ�ֱ������b,�����ֱ˵������
        Vector3 auxiliary = Vector3.Cross(p3 - p1, a);
        if (Vector3.Dot(auxiliary, b) != 0)
        {
            Debug.Log("����ֱ������");
        }

        //��������ֱ�߱��ཻ���󽻵�

        //������˵õ����
        double area1 = Vector3.Cross(auxiliary, b).magnitude / 2;       
        double area2 = Vector3.Cross(p2 + p3 - p1 - p4, b).magnitude / 2;
        double specificValue = area1 / area2;
        //��֪p1o/p1p2 ��o���λ��2
        return p1 + (p2 - p1) * (float)specificValue;

    }
}
