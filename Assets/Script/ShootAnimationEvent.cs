using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAnimationEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void CreateBall()
    {
        GameState.Bm.CreateNewMarbleAndSetPos();
    }
}
