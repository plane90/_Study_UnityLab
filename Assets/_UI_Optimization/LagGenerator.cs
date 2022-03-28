using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIOptimization
{
    public class LagGenerator : MonoBehaviour
    {
        private void Awake()
        {
            Procedure(-1);
        }

        private static void Procedure(int cnt)
        {
            if (cnt < 0)
            {
                return;
            }
            Debug.Log($"Proceduer start id:{System.Threading.Thread.CurrentThread.ManagedThreadId} ");
            int count = 0;
            int i = 0;
            while (true)
            {
                i++;
                if (i == 2000000000)
                {
                    i = 0;
                    count++;
                    if (count == cnt)
                    {
                        Debug.Log($"Proceduer end id:{System.Threading.Thread.CurrentThread.ManagedThreadId} ");
                        return;
                    }
                }
            }
        }
    }
}