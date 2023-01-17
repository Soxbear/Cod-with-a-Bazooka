using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Dynamic<T>
{
    void PassValue(T Value);

    T GetValue();
}