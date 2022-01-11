using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRPainting
{
    public class StateDisplay : MonoBehaviour
    {
        Text text;

        GlobalState global;
        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<Text>();
            global = GameObject.Find("Global State").GetComponent<GlobalState>();
        }

        // Update is called once per frame
        void Update()
        {
            float fps = 1f / Time.deltaTime;
            string action = global.action.ToString();
            string mode = global.mode.ToString();
            string surfaceMode = global.surfaceMode.ToString();
            text.text = $"��ǰ����:{action}\n��ͼģʽ:{mode}\n����ģʽ:{surfaceMode}\nFPS:{fps.ToString("f2")}";
        }
    }

}