using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convert 
{
    public int[,] Convert1Dto2D(int[] a)
    {
        int[,] array2d = new int[10,10];
        int i  = 0 , j = 0;
        for(int x = 0 ; x < a.Length ;x++)
        {
            array2d[i,j] = a[x];
            j++;
            if(x%10 == 9)
            {
                i +=1;
                j = 0;
            }
        }
        return array2d;
    }
}
