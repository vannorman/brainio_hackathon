using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
//[System.Serializable]
public class SOCircularBufferDouble : ScriptableObject
{
    [SerializeField]
    private string dataName = "";

    private const int sizeOfBuffer = 100;
    private uint numberValuesAdded = 0; // count of number times Push called, so we only use that max of that many

    private double[] dataBuffer = new double [sizeOfBuffer];

    private int _indexLatest;

    public int indexLatest { get { return _indexLatest; } }
        
    public double getLatest 
    {
        get { return dataBuffer[_indexLatest]; }
    }

    public void pushValue(double value)
    {
        if (_indexLatest == sizeOfBuffer - 1)
        {
            _indexLatest = 0;
        }
        else
        {
            _indexLatest++;
        }
        dataBuffer[_indexLatest] = value;
        numberValuesAdded++;
        if (numberValuesAdded >= sizeOfBuffer)
            numberValuesAdded = sizeOfBuffer;
    }

    public double averageLastN(int n)
    {
        double retVal = 0;
        double accumulator = 0;
        int counter = 0;
        if (n > sizeOfBuffer)
            Debug.LogError("requested size greater than buffer size");
        for (int idx = _indexLatest; counter < n && counter < numberValuesAdded; counter++)
        {
            accumulator += dataBuffer[idx];
            idx--;
            if (idx < 0)
                idx = sizeOfBuffer - 1;
        }

        retVal = accumulator / n;
        return retVal;
    }
}
