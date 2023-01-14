using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1.3f);
    }
}
