﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Board : MonoBehaviour {

    public GameObject CrossPrefab;

    // 交叉点大小
    const float CrossSize = 40;

    // 15个交叉
    public const int CrossCount = 15;

    // 棋盘大小
    public const int Size = 560;
    public const int HalfSize = Size / 2;

    // 存储每个交叉点按钮信息
    Dictionary<int, Cross> _crossMap = new Dictionary<int, Cross>();

    static int MakeKey( int x, int y )
    {
        return x * 10000 + y;
    }
    public void Deletepositionobject(int gridX,int gridY)
    {
       //Cross temp= GetComponentInChildren<Cross>();
       // GameObject temp01 = temp.GetComponentInChildren<GameObject>();
        //Destroy(temp01);
        //  Destroy(child.gameObject);

    }
    public void Reset( )
    {
        // 删除棋盘上的所有对象
        foreach( Transform child in gameObject.transform )
        {
            Destroy(child.gameObject);
        }

        var mainLoop = GetComponent<MainLoop>();

        _crossMap.Clear();

        for (int x = 0; x < Board.CrossCount; x++)
        {
            for (int y = 0; y < Board.CrossCount; y++)
            {
                var crossObj = GameObject.Instantiate<GameObject>(CrossPrefab);

                // 归属于本层对象下
                crossObj.transform.SetParent(gameObject.transform);

                // 复位缩放
                crossObj.transform.localScale = Vector3.one;

                // 设置位置
                var pos = crossObj.transform.localPosition;
                pos.x = -Board.HalfSize + x * CrossSize;
                pos.y = -Board.HalfSize + y * CrossSize;
                pos.z = 1;
                crossObj.transform.localPosition = pos;

                // 记录格子信息
                var cross = crossObj.GetComponent<Cross>();
                cross.GridX = x;
                cross.GridY = y;
                cross.mainLoop = mainLoop;

                _crossMap.Add(MakeKey(x, y ), cross);


            }
        }
    }

    

	void Start () {

        Reset();
	}

    public Cross GetCross(int gridX, int gridY )
    {        
        Cross cross;
        if ( _crossMap.TryGetValue( MakeKey(gridX, gridY), out cross ) )
        {
            return cross;
        }

        return null;
    }
	

}
