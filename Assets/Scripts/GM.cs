using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public int[,] gContainer = new int[10, 10];
    public GameObject[,] gOLines = new GameObject[10,10];
    public GameObject[] gPrefabs;//[4]是选择
    public GameObject gLinePrefab;
    public GameObject[] gOSelCristal = new GameObject[2];//gameObject-Selected-Cristal
    private int   count_gOSelCristal = 0;

    public bool animing = false;//动画播放中==true
    public int playing = 1;//用户可操作==1
    public bool gIsDead = false;

    void MouseSelect()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (col.Length > 0)
            {
                foreach (Collider2D c in col)//其实这边只会碰到一个23333   c是Collider2D的缩写
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

    public void GenMObj(int form,int x,int y)//Generate Moveable OBJ
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
    public void PlayDead()
    {

    }

    public int Boom(int x,int y)
    {
        print("in "+x+" "+y);
        GameObject gOL = gOLines[x, y];
        Collider2D[] col = Physics2D.OverlapPointAll(gOL.transform.position);
        if (col.Length > 0)
        {
            Destroy(col[0].gameObject);
            return 1;
        }
        else
        {
            return 0;
            //[x,y]上不存在宝石
        }
    }

    public int Lazer_anime(int x,int y)//gOL=gameObjectLines , 此操作为Lazer当前块的移动方向
    {
        GameObject gOL = gOLines[x, y];
        Collider2D[] col = Physics2D.OverlapPointAll(gOL.transform.position);
        if (col.Length > 0)
        {
            return col[0].GetComponent<GO>()._moveForm;
        }
        else
        {
            return 0;//当前无块,无动画
        }
    }

    public int Lazer(GameObject gOL)//gOL=gameObjectLines , 此操作为Lazer当前块的类型
    {
        Collider2D[] col = Physics2D.OverlapPointAll(gOL.transform.position);
        if (col.Length > 0)
        {
            return col[0].GetComponent<GO>().form;
        }
        else
        {
            return -1;//NONE
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
                if(gContainer[i,j] == -1)
                {
                    //这个位置为空,可以进行下落操作
                }
            }
        }
    }
    void Start()
    {
        GenLines();

        GenMObj(0, 0, 0);
        GenMObj(2, 1, 0);
        GenMObj(0, 2, 0);
        GenMObj(0, 3, 0);

        GenMObj(2, 0, 1);
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
        playing = count_gOSelCristal == 2 ? 0 : 1;

        animing = false;
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                animing |= (Lazer_anime(i, j) != 0);

        if (gIsDead)
        {

        }
        else
        {
            if (playing==1 && animing == false)
            {
                MouseSelect();
            }
            else if(playing == 0 && animing==false)
            {
                int[,] prob_x = new int[10, 10];
                int[,] prob_y = new int[10, 10];
                //bool[,] enable = new bool[10, 10];

                LazerAll();
                //为gContainer[,]数组赋值
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (i != 9 && i != 0)
                        {
                            if (gContainer[i, j] == gContainer[i + 1, j])
                                prob_x[i, j]++;
                            if (gContainer[i, j] == gContainer[i - 1, j])
                                prob_x[i, j]++;
                        }
                        else if (i == 9 && gContainer[i, j] == gContainer[i - 1, j])
                        {
                            prob_x[i, j]++;
                        }
                        else if (i == 0 && gContainer[i, j] == gContainer[i + 1, j])
                        {
                            prob_x[i, j]++;
                        }
                        //prob_x

                        if (j != 9 && j != 0)
                        {
                            if (gContainer[i, j] == gContainer[i, j + 1])
                                prob_y[i, j]++;
                            if (gContainer[i, j] == gContainer[i, j - 1])
                                prob_y[i, j]++;
                        }
                        else if (j == 9 && gContainer[i, j] == gContainer[i, j - 1])
                        {
                            prob_y[i, j]++;
                        }
                        else if (j == 0 && gContainer[i, j] == gContainer[i, j + 1])
                        {
                            prob_y[i, j]++;
                        }
                        //prob_y
                    }
                }

                //for (int i = 0; i < 10; i++)
                //    for (int j = 0; j < 10; j++)
                //        enable[i, j] = false;
                //背景色2333

                int boomCount = 0;//可以作为分数记录器哦

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if(prob_x[i,j]==2 )//&& i!=0&&i!=9 , 因为四角!出现2
                        {
                            //enable[i, j] = true;
                            //enable[i+1, j] = true;
                            //enable[i-1, j] = true;
                            boomCount += Boom(i, j);
                            boomCount += Boom(i+1, j);
                            boomCount += Boom(i-1, j);
                        }
                        if(prob_y[i,j]==2 )
                        {
                            //enable[i, j] = true;
                            //enable[i, j+1] = true;
                            //enable[i, j-1] = true;
                            boomCount += Boom(i, j);
                            boomCount += Boom(i, j+1);
                            boomCount += Boom(i, j-1);
                        }
                    }
                }

                if(boomCount ==0)//无消除,执行无效操作后的返回动画
                {
                    
                    if (gOSelCristal[0].transform.position.x == gOSelCristal[1].transform.position.x)
                    {
                        if (gOSelCristal[0].transform.position.y > gOSelCristal[1].transform.position.y)
                        {
                            gOSelCristal[0].GetComponent<GO>().MMove = 4;
                            gOSelCristal[1].GetComponent<GO>().MMove = 3;
                        }
                        else
                        {
                            gOSelCristal[0].GetComponent<GO>().MMove = 3;
                            gOSelCristal[1].GetComponent<GO>().MMove = 4;
                        }
                    }
                    else if(gOSelCristal[0].transform.position.y == gOSelCristal[1].transform.position.y)
                    {
                        if(gOSelCristal[0].transform.position.x > gOSelCristal[1].transform.position.x)
                        {
                            gOSelCristal[0].GetComponent<GO>().MMove = 2;
                            gOSelCristal[1].GetComponent<GO>().MMove = 1;
                        }
                        else
                        {
                            gOSelCristal[0].GetComponent<GO>().MMove = 1;
                            gOSelCristal[1].GetComponent<GO>().MMove = 2;
                        }
                    }

                    animing = true;
                }
                //以上为消除 & 如果无消除,执行无效操作后的返回动画



                //以上为下落
                count_gOSelCristal = 0;
            }
        }
    }
    
}
