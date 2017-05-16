using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeColorSheme : ScriptableObject
{
    public Color U;
    public Color R;
    public Color F;
    public Color L;
    public Color B;
    public Color D;

    public Color GetColor(int index)
    {
        switch (index)
        {
            case 0:
                return U;
            case 1:
                return R;
            case 2:
                return F;
            case 3:
                return L;
            case 4:
                return B;
            case 5:
                return D;
            default:
                throw new IndexOutOfRangeException();
        }
    }
}
