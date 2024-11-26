using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public static class Utility
{
    public static Vector3 randomPos(float range)
    {
        float x = UnityEngine.Random.Range(-range, range);
        float z = UnityEngine.Random.Range(-range, range);

        return new Vector3(x, 0, z);
    }

    public static Quaternion RotationSystem()
    {
        float r = UnityEngine.Random.Range(0, 360);
        Quaternion angle = Quaternion.Euler(0, r, 0);
        return angle;
    }

    public static Quaternion RotationSystem(bool x, bool y, bool z)
    {
        float xAngle = 0;
        float yAngle = 0;
        float zAngle = 0;
        if (x) 
        {
          xAngle = UnityEngine.Random.Range(0, 360);
        }
        if (y)
        {
            yAngle = UnityEngine.Random.Range(0, 360);
        }
        if (z)
        {
            zAngle = UnityEngine.Random.Range(0, 360);

        }

        Quaternion angle = Quaternion.Euler(xAngle, yAngle, zAngle);
        return angle;
    }


    public static Vector3 RandomScale(float x, float y, float z)
    {
        float xScale = 1;
        float yScale = 1;
        float zScale = 1;
        if (x!=0)
        {
            xScale = UnityEngine.Random.Range(1, x);
        }
        if (y != 0)
        {
            yScale = UnityEngine.Random.Range(1,y);
        }
        if (z != 0)
        {
            zScale = UnityEngine.Random.Range(1,z);

        }

        Vector3 Scale =new Vector3(xScale, yScale, zScale);
        return Scale;
    }

    public static bool Chance(int a)
    {
        int r = Random.Range(0, 10000);

        if (r < a)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}


