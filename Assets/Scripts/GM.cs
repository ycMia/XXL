using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public int[,] gContainer = new int[10, 11];
    public GameObject[,] gOLines = new GameObject[10,11];
    public GameObject[] gPrefabs;//[4]是选择
    public GameObject gLinePrefab;
    public GameObject[] gOSelCristal = new GameObject[2];//gameObject-Selected-Cristal
    public int   count_gOSelCristal = 0;

    public bool animationPlaying = false;//动画播放中==true
    public int playing = 1;//用户可操作==1
    public bool gIsDead = false;

    public bool tBtn = false;

    void MouseSelect()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (col.Length > 0)
            {
                foreach (Collider2D c in col)//其实这边只会碰到一个23333   c是Collider2D的缩写
                {
                    //Debug.Log(count_gOSelCristal);
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

        //mObj.SetActive(true);
    }

    public void GenLines()
    {
        for(int i = 0;i< 10; i++)
            for(int j=0;j< 11; j++)
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
        //print("in "+x+" "+y);
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

    public GameObject Lazer_getObject(int x, int y)
    {
        //print("in "+x+" "+y);
        GameObject gOL = gOLines[x, y];
        Collider2D[] col = Physics2D.OverlapPointAll(gOL.transform.position);
        if (col.Length > 0)
        {
            return col[0].gameObject;
        }
        else
        {
            return null;
            //[x,y]上不存在宝石,返回null
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
            for(int j=0;j<11;j++)
            {
                gContainer[i,j] = Lazer(gOLines[i, j]);
            }
        }

        //for (int i = 0; i < 10; i++)
        //{
        //    for (int j = 0; j < 10; j++)
        //    {
        //        if(gContainer[i,j] == -1)
        //        {
        //            //这个位置为空,可以进行下落操作
        //            //下落操作已经完成
        //        }
        //    }
        //}
    }
    public void SpawnRow()
    {
        LazerAll();
        for (int i = 0; i < 10; i++)
        {
            if( gContainer[i, 10] == -1 && gContainer[i, 10] == -1)
            {
                GenMObj(Random.Range(0, 3), i, 10);
            }
        }
    }

    void Start()
    {
        GenLines();
        //Lazer(gOLines[1,0]);//碰撞事件只能在Start后开始(一帧你碰不了)
    }
    
    void Update()
    {
        if (tBtn)
            Time.timeScale = 20;
        else
            Time.timeScale = 1;

        SpawnRow();

        //if(tBtn ==true)
        //{
        //    GenMObj(3, 0, 10);
        //    GenMObj(2, 1, 10);
        //    GenMObj(2, 2, 10);
        //    GenMObj(0, 3, 10);
        //    tBtn = false;
        //}//用于模拟RandomRow

        playing = 1;//初始设定玩家可以操作...
        LazerAll();
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)//RandomRow方法施工完毕后i,j的限制条件应该改为10
                if (Lazer(gOLines[i, j]) == -1)
                {
//                  print("in" + i + " " + j);
                    playing = 0;//如果在范围内有空块(需要下落操作)则玩家必须等待下落完毕
                    i = 10;//跳出大循环
                    break;
                }

        if (count_gOSelCristal == 2 && (gOSelCristal[0] == null || gOSelCristal[1] == null)) count_gOSelCristal = 0;
        //防止完全下落完毕后玩家的选择宝石数仍然保持在2个(选择中bug)


        animationPlaying = false;
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                animationPlaying |= (Lazer_anime(i, j) != 0);

        if (gIsDead)
        {
            //playIsDead
        }
        else if (animationPlaying == false)
        {
            //print("in 下落");
            SpawnRow();
            //为gContainer[,]数组赋值,即获取全部已经block住的宝石的阵列
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)//RandomRow方法施工完毕后i,j的限制条件应该改为10
                {
                    //    if(i==1&&j==1)
                    //        print(i + " " + j + " " + gContainer[i, j]);
                    if (gContainer[i, j] == -1)//这个块被扫描出是空的
                    {
                        for (int z = j; z < 11; z++)//第11层, 下落层也要包括进去
                        {
                            if (Lazer_getObject(i, z) != null)
                            {
                                Lazer_getObject(i, z).GetComponent<GO>().MMove = 4;
                            }
                            else
                            {
                                print("有区域需要填补"+i+" "+z);
                            }
                        }
                    }
                }
            }
            //以上为下落

            //print("in 消除");
            LazerAll();
            //为gContainer[,]数组赋值,即获取全部已经block住的宝石的阵列

            int[,] prob_x = new int[10, 10];
            int[,] prob_y = new int[10, 10];
            //bool[,] enable = new bool[10, 10];

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
                    if (prob_x[i, j] == 2)//&& i!=0&&i!=9 , 因为四角不会出现2
                    {
                        //enable[i, j] = true;
                        //enable[i+1, j] = true;
                        //enable[i-1, j] = true;
                        boomCount += Boom(i, j);
                        boomCount += Boom(i + 1, j);
                        boomCount += Boom(i - 1, j);
                    }
                    if (prob_y[i, j] == 2)
                    {
                        //enable[i, j] = true;
                        //enable[i, j+1] = true;
                        //enable[i, j-1] = true;
                        boomCount += Boom(i, j);
                        boomCount += Boom(i, j + 1);
                        boomCount += Boom(i, j - 1);
                    }
                }
            }
            //if (playing == 1 && animationPlaying == false && count_gOSelCristal == 2)
            if (boomCount == 0 && gOSelCristal[0]!=null && gOSelCristal[1]!=null && count_gOSelCristal ==2)//无消除,执行无效操作后的返回动画(这里只能判断物体是否存在了,我别无他法)
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
                else if (gOSelCristal[0].transform.position.y == gOSelCristal[1].transform.position.y)
                {
                    if (gOSelCristal[0].transform.position.x > gOSelCristal[1].transform.position.x)
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
                count_gOSelCristal = 0;
            }

            else
            {
                //Debug.Log(boomCount);
            }
        }
        //(有消除或无消除后都会播放动画(下落/回弹)animing == true)
        //以上为消除

        if (playing==1)
        {
            if(animationPlaying == false)
                MouseSelect();
        }

    }
    
}
