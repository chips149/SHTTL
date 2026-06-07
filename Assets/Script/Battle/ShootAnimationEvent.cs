using System.Collections.Generic;
using UnityEngine;

public class ShootAnimationEvent : MonoBehaviour
{
 
    void CreateBall()
    {
        GameState.Bm.CreateNewMarbleAndSetPos();
    }
}
