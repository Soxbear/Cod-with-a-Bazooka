using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    Player Player;
    void Start()
    {  
        Player = FindObjectOfType<Player>();
    }
    void Update()
    {
        transform.position = Player.transform.position + (Vector3) Player.aimPos;
    }
}
