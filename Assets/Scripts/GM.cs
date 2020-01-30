using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public int[,] gContainer = new int[10, 10];
    public GameObject[,] gOLines = new GameObject[10,10];
    public GameObject[] gPrefabs;//[4]是选择
    public GameObject gLinePrefab;
    public GameObject[] gOSelCristal = new GameObject[2];
    private int   count_gOSelCristal = 0;

    public bool playing = true;//is 用户操作ing?
    public bool gIsDead = false;

    void MouseSelect()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (col.Length > 0)
            {
                foreach (Collider2D c in col)//其实这边只会碰到一个23333
                {
                    Debug.Log(count_gOSelCristal);
                    if(count_gOSelCristal==0)
                    {
                        c.gameObject.GetComponent<GO>().onClick = true;
                        gOSelCristal[0] = c.gameObject;
                        count_gOSelCristal=1;
                    }
                    else
                    {
                        if(c.gameObject.GetComponent<GO>().mLocalPosition.x == gOSelCristal[0].GetComponent<GO>().mLocalPosition.x &&
                            c.gameObject.GetComponent<GO>().mLocalPosition.y == gOSelCristal[0].GetComponent<GO>().mLocalPosition.y)//选中了相同宝石
                        {
                            gOSelCristal[0].GetComponent<GO>().onClick = false;
                            count_gOSelCristal = 0;//删除
                        }
                        else if (c.gameObject.GetComponent<GO>().mLocalPosition.x == gOSelCristal[0].GetComponent<GO>().mLocalPosition.x+1 &&
                            c.gameObject.GetComponent<GO>().mLocalPosition.y == gOSelCristal[0].GetComponent<GO>().mLocalPosition.y)//c 在右侧
                        {
                            gOSelCristal[1] = c.gameObject;
                            gOSelCristal[0].GetComponent<GO>().onClick = false;

                            gOSelCristal[1].GetComponent<GO>().MMove = 2;//c左移
                            gOSelCristal[0].GetComponent<GO>().MMove = 1;//gOSel右移
                            
                            count_gOSelCristal = 2;//需要处理
                        }
                        else if (c.gameObject.GetComponent<GO>().mLocalPosition.x == gOSelCristal[0].GetComponent<GO>().mLocalPosition.x - 1 &&
                            c.gameObject.GetComponent<GO>().mLocalPosition.y == gOSelCristal[0].GetComponent<GO>().mLocalPosition.y)//c 左侧
                        {
                            gOSelCristal[1] = c.gameObject;
                            gOSelCristal[0].GetComponent<GO>().onClick = false;

                            gOSelCristal[1].GetComponent<GO>().MMove = 1;//c右移
                            gOSelCristal[0].GetComponent<GO>().MMove = 2;//gOSel左移

                            count_gOSelCristal = 2;//需要处理
                        }
                        else if (c.gameObject.GetComponent<GO>().mLocalPosition.x == gOSelCristal[0].GetComponent<GO>().mLocalPosition.x &&
                            c.gameObject.GetComponent<GO>().mLocalPosition.y == gOSelCristal[0].GetComponent<GO>().mLocalPosition.y + 1)//c 在上方
                        {
                            gOSelCristal[1] = c.gameObject;
                            gOSelCristal[0].GetComponent<GO>().onClick = false;

                            gOSelCristal[1].GetComponent<GO>().MMove = 4;//c下移
                            gOSelCristal[0].GetComponent<GO>().MMove = 3;//gOSel上移

                            count_gOSelCristal = 2;//需要处理
                        }
                        else if (c.gameObject.GetComponent<GO>().mLocalPosition.x == gOSelCristal[0].GetComponent<GO>().mLocalPosition.x &&
                            c.gameObject.GetComponent<GO>().mLocalPosition.y == gOSelCristal[0].GetComponent<GO>().mLocalPosition.y - 1)//c 在下方
                        {
                            gOSelCristal[1] = c.gameObject;
                            gOSelCristal[0].GetComponent<GO>().onClick = false;

                            gOSelCristal[1].GetComponent<GO>().MMove = 3;//c上移
                            gOSelCristal[0].GetComponent<GO>().MMove = 4;//gOSel下移

                            count_gOSelCristal = 2;//需要处理
                        }
                        else//选中了不在周围的宝石
                        {
                            gOSelCristal[0].GetComponent<GO>().onClick = false;
                            gOSelCristal[0] = c.gameObject;
                            gOSelCristal[0].GetComponent<GO>().onClick = true;
                        }

                    }
                }
            }
        }
    }

    public void GenMObj(int form,int x,int y)
    {
        GameObject mObj;GameObject fObj;
        fObj = Instantiate(gPrefabs[4], gameObject.transform.position, gameObject.transform.rotation);

        mObj = Instantiate(gPrefabs[form], transform.position, transform.rotation);
        mObj.transform.parent = transform;
        mObj.transform.localPosition = new Vector3(x, y, 0f);
        mObj.AddComponent<GO>().mLocalPosition = new Vector2Int(x, y);
        mObj.GetComponent<GO>().gOframe = fObj;
        mObj.GetComponent<GO>().form = form;

        fObj.transform.parent = mObj.transform;
        fObj.transform.localPosition = new Vector3(0, 0, 0);
        fObj.GetComponent<SpriteRenderer>().sortingOrder = 1;

        mObj.SetActive(true);
    }

    public void GenLines()
    {
        for(int i = 0;i< 10; i++)
            for(int j=0;j< 10; j++)
            {
                gOLines[i, j] = Instantiate(gLinePrefab, transform.position, transform.rotation);
                gOLines[i, j].transform.parent = transform;
                gOLines[i, j].name = i.ToString() + "_" + j.ToString();
                gOLines[i, j].transform.localPosition = new Vector3(i, j, 0f);
                gOLines[i, j].SetActive(true);
            }
    }

    public bool check_gContainer(int x,int y,int k)
    {

        //用dfs解决染色

        return false;
    }

    public int Lazer(GameObject gOL)
    {
        Collider2D[] col = Physics2D.OverlapPointAll(gOL.transform.position);
        if (col.Length > 0)
        {
            return col[0].GetComponent<GO>().form;
        }
        else
        {
            return -1;
        }
    }

    public void LazerAll()
    {
        for(int i=0;i<10;i++)
        {
            for(int j=0;j<10;j++)
            {
                gContainer[i,j] = Lazer(gOLines[i, j]);
            }
        }

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if(gContainer[i,j] != -1)
                {

                }
                else
                {
                    //continue;
                }
            }
        }
    }
    void Start()
    {
        GenLines();
        GenMObj(0, 0, 0);
        GenMObj(1, 1, 0);
        GenMObj(0, 2, 0);
        GenMObj(0, 3, 0);

        GenMObj(1, 0, 1);
        GenMObj(1, 1, 1);
        GenMObj(2, 2, 1);
        GenMObj(1, 3, 1);

        GenMObj(0, 0, 2);
        GenMObj(1, 1, 2);
        GenMObj(3, 2, 2);
        GenMObj(0, 3, 2);

        GenMObj(2, 0, 3);
        GenMObj(2, 1, 3);
        GenMObj(0, 2, 3);
        GenMObj(3, 3, 3);

        GenMObj(3, 0, 4);
        GenMObj(2, 1, 4);
        GenMObj(2, 2, 4);
        GenMObj(0, 3, 4);

        //Lazer(gOLines[1,0]);//碰撞事件只能在Start后开始(一帧你碰不了)
    }

    void Update()
    {
        playing = count_gOSelCristal == 2 ? false : true;
        if(gIsDead)
        {

        }
        else
        {
            if (playing)
            {
                MouseSelect();
            }
            else
            {
                LazerAll();
            }
        }
    }
    public void PlayDead()
    {

    }

}
