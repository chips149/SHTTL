using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleMapBuilder : MonoBehaviour
{
    public GameObject userAreaPrefab;
    public GameObject nailPrefab;
    public int width, height;
    public float xGap =1, yGap =1;

    public bool startWithSingle;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnValidate()
    {
        
        var center = new Vector3( width * xGap / 2, 0, 0) - Vector3.right * (0.5f * xGap);
        
        var nailParent = transform.Find("NailParent");
        var nailIndex = 0;
        for (int y = 0; y < height; y++)
        {
            var isSingle = y % 2 == 1 ^ startWithSingle;


            if (isSingle)
            {
                for (int x = 0; x < width -1; x++)
                {
                    nailParent.GetChild(nailIndex).localPosition = 
                        new Vector3(x * xGap, y * yGap, 0)
                        + Vector3.right * (xGap * 0.5f)
                        - center;


                    nailIndex++;
                }
            }
            else
            {
                for (int x = 0; x < width ; x++)
                {

                    nailParent.GetChild(nailIndex).localPosition 
                        = new Vector3(x * xGap, y * yGap, 0)
                          -center;
                    nailIndex++;
                }
            }
            

        }

        
        var userAreaParent = transform.Find("UserAreaParent");
        var userAreaIndex = 0;
        for (int y = 0; y < height; y++)
        {
            var isSingle = y % 2 == 0 ^ startWithSingle;

            if (isSingle)
            {
                for (int x = 0; x < width -1; x++)
                {
                    userAreaParent.GetChild(userAreaIndex).localPosition = 
                        new Vector3(x * xGap, y * yGap, 0)
                        + Vector3.right * (xGap * 0.5f)
                        - center;

                    userAreaIndex++;
                } 
            }
            else
            {
                for (int x = 0; x < width ; x++)
                {
                    userAreaParent.GetChild(userAreaIndex).localPosition 
                        = new Vector3(x * xGap, y * yGap, 0)
                          -center;
                    userAreaIndex++;
                } 
            }
            
        }

    }
}
