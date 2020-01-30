using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GO : MonoBehaviour
{
    public bool _boom = false;
    public int _moveForm;//0=block;1=right;2=left;3=up;4=down;
    public int form;
    public bool onClick = false;
    public int MMove { set => _moveForm = value; }//2020.1.30 这个有必要吗?(没有)
    public float moveSpeed = 2f;
    //Vector3 gOposition;

    public GameObject gOframe;
    public Vector2Int mLocalPosition;
    
    void Start()
    {
        
        Debug.Log("Created  :" + gameObject.name + "   " + gameObject.transform.position.x + " " + gameObject.transform.position.y);
    }
    public void MDestory()
    {
        Destroy(gameObject);
    }
    
    void Update()
    {
        #region 不想看_moveForm
        if (_moveForm!=0)//no Block
        {
            switch (_moveForm)
            {
                case 1:
                    Vector3 toPosition = new Vector3(mLocalPosition.x + 1f, mLocalPosition.y, 0f);

                    float distance = toPosition.x - gameObject.transform.localPosition.x;
                    //Debug.Log("Time.delteTime= "+Time.deltaTime);
                    //Debug.Log("dist = "+distance);
                    if (distance >= moveSpeed*Time.deltaTime)
                    {
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + moveSpeed * Time.deltaTime,
                                                                         gameObject.transform.localPosition.y,
                                                                         0f);
                    }
                    else
                    {
                        gameObject.transform.localPosition = toPosition;
                        mLocalPosition = new Vector2Int(mLocalPosition.x + 1, mLocalPosition.y);
                        _moveForm = 0;
                    }
                    break;
                case 2:
                    toPosition = new Vector3(mLocalPosition.x - 1f, mLocalPosition.y, 0f);

                    distance = toPosition.x - gameObject.transform.localPosition.x;
                    //Debug.Log("Time.delteTime= " + Time.deltaTime);
                    //Debug.Log("dist = " + distance);
                    if (-distance >= moveSpeed * Time.deltaTime)
                    {
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - moveSpeed * Time.deltaTime,
                                                                         gameObject.transform.localPosition.y,
                                                                         0f);
                    }
                    else
                    {
                        gameObject.transform.localPosition = toPosition;
                        mLocalPosition = new Vector2Int(mLocalPosition.x - 1, mLocalPosition.y);
                        _moveForm = 0;
                    }
                    break;
                case 3:
                    toPosition = new Vector3(mLocalPosition.x, mLocalPosition.y+ 1f, 0f);

                    distance = toPosition.y - gameObject.transform.localPosition.y;
                    //Debug.Log("Time.delteTime= " + Time.deltaTime);
                    //Debug.Log("dist = " + distance);
                    if (distance >= moveSpeed * Time.deltaTime)
                    {
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x ,
                                                                         gameObject.transform.localPosition.y + moveSpeed * Time.deltaTime,
                                                                         0f);
                    }
                    else
                    {
                        gameObject.transform.localPosition = toPosition;
                        mLocalPosition = new Vector2Int(mLocalPosition.x, mLocalPosition.y + 1);
                        _moveForm = 0;
                    }
                    break;
                case 4:
                    toPosition = new Vector3(mLocalPosition.x, mLocalPosition.y - 1f, 0f);

                    distance = toPosition.y - gameObject.transform.localPosition.y;
                    //Debug.Log("Time.delteTime= " + Time.deltaTime);
                    //Debug.Log("dist = " + distance);
                    if (-distance >= moveSpeed * Time.deltaTime)
                    {
                        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x ,
                                                                         gameObject.transform.localPosition.y - moveSpeed * Time.deltaTime,
                                                                         0f);
                    }
                    else
                    {
                        gameObject.transform.localPosition = toPosition;
                        mLocalPosition = new Vector2Int(mLocalPosition.x, mLocalPosition.y - 1);
                        _moveForm = 0;
                    }
                    break;
            }
        }
        else
        {
            //don't move it
        }
        #endregion

        gOframe.SetActive(onClick);
    }
}
