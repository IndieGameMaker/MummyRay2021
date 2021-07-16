using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject goodObj;
    public GameObject badObj;

    public int goodObjCount = 30;
    public int badObjCount = 10;

    private List<GameObject> goodList = new List<GameObject>();
    private List<GameObject> badList = new List<GameObject>();


    void Start()
    {
        SetStateObject();
    }

    public void SetStateObject()
    {
        // Good Object 생성
        for (int i = 0; i < goodObjCount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-24.0f, 24.0f), 0.05f, Random.Range(-24.0f, 24.0f));
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            goodList.Add(Instantiate(goodObj, transform.position + pos, rot, transform));
        }

        // Bad Object 생성
    }
}
