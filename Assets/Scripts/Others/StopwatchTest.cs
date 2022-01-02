using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;
using System.IO;

public class StopwatchTest : MonoBehaviour
{
    //��ʱ
    Stopwatch stopwatch;
    TimeSpan timeSpan;
    String time;
    //�Ʊʻ���
    int strokesCount;
    //�Ʊ�����
    int surfacesCount;

    StreamWriter sw;//true׷�ӣ�false����
    // Start is called before the first frame update
    void Start()
    {

        sw = new StreamWriter("E:/data.csv", true);

        //��һ��
        sw.Write("Time,");
        sw.Write("StrokesCount,");
        sw.WriteLine("SurfacesCount");

        stopwatch = new Stopwatch();
        strokesCount = 10;
        surfacesCount = 8;


        //ˢ�»���
        sw.Flush();
        //�ر���
        sw.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            stopwatch.Reset();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            stopwatch.Start();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            stopwatch.Stop();
            timeSpan = stopwatch.Elapsed;
            time = timeSpan.Hours.ToString() + ":" 
                + timeSpan.Minutes.ToString() + ":" 
                + timeSpan.Seconds.ToString() + ":"
                + timeSpan.Milliseconds.ToString();
            UnityEngine.Debug.Log(time);
            CountStrokes();
            WriteData();
            ScreenCapture.CaptureScreenshot("E:/screenshot"+Time.time +".png");

        }
    }

    private void WriteData()
    {

        sw = new StreamWriter("E:/data.csv", true);

        //�ڶ���
        sw.Write(time + ",");
        sw.Write(strokesCount.ToString() + ",");
        sw.WriteLine(surfacesCount.ToString());

        //ˢ�»���
        sw.Flush();
        //�ر���
        sw.Close();
    }

    private void CountStrokes() {
        strokesCount = 0;
        surfacesCount = GameObject.Find("Draw Surface").transform.childCount-1;
        for(int i = 0; i <= surfacesCount; i++) {
            strokesCount += GameObject.Find("Draw Surface").transform.GetChild(i).childCount;
        }
    }
}
