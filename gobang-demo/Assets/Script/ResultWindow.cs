﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultWindow : MonoBehaviour {

    // 重玩按钮
    public Button ReplayButton;

    // 提示文字
    public Text Message;

    // 主循环句柄
    public MainLoop mainLoop;

    public Text txxttimer;
    void Start () {
        ReplayButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            mainLoop.Restart();
        });
	}
	
    public void Show( ChessType wintype)
    {
        switch( wintype )
        {
            case ChessType.Black:
                {
                    Message.text = string.Format("恭喜, 你战胜了电脑! 耗时"+ txxttimer.text);
                }
                break;
            case ChessType.White:
                {
                    Message.text = string.Format("你被电脑击败了! 耗时"+ txxttimer.text);
                }
                break;
        }
    }
}
