using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;

public class CallDllFunctions : MonoBehaviour
{
    [DllImport("MACHINELEARNINGDLL")]
    public static extern int Add(int a, int b);

    [DllImport("MACHINELEARNINGDLL")]
    public static extern IntPtr CreatePerceptron(int nbiteration, double learningRate);

    [DllImport("MACHINELEARNINGDLL")]
    public static extern void TrainingPerceptron(IntPtr Modele, double[] pointPositionListX, double[] pointpositionListY, int nbrows, double[] idColorList);
    //void TrainingPerceptron(Perceptron* Modele,double* pointPositionListX,double* pointPositionListY,int nbrows, double* idcolor

    [DllImport("MACHINELEARNINGDLL")]
    public static extern void DestroyPerceptron(IntPtr p);


    [DllImport("MACHINELEARNINGDLL")]
    public static extern int PerdictPoint(IntPtr TestPeceptron, double[] pointTest, int nbrows);
    //int PerdictPoint(Perceptron* TestPeceptron, double* pointTest,int nbrows)



    [SerializeField]
    public Data dataSource;

    private IntPtr Perceptron;
    private void Start()
    {
        //dataSource.CreateData();
        //int nbrows = dataSource.dataListX.Length;
        Debug.Log("Perceptron create : " + Perceptron);
    }

    public void Training()
    {
        Debug.Log("TRAINING");
        dataSource.UpdateTrainingCoord();
        int nbrows = dataSource.dataListX.Length;
        Perceptron = CreatePerceptron(50, 1);
        TrainingPerceptron(Perceptron, dataSource.dataListX, dataSource.dataListY, dataSource.dataListX.Length, dataSource.idColorList);
    }

    public void Predict()
    {        
        List<int> result = new List<int>();
        for (int i = 0; i < dataSource.blankDataListX.Length; i++)
        {
            double[] point = new double[2] { dataSource.blankDataListX[i], dataSource.blankDataListY[i] };
            result.Add((PerdictPoint(Perceptron, point, 2)));
        }
        for (int i = 0; i < result.Count; i++)
        {
            dataSource.ColorPoint(i, result[i]);
        }
        DestroyPerceptron(Perceptron);
        Perceptron = IntPtr.Zero;
    }

    public void NewDataSet()
    {
        dataSource.CreateData();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("TRAINING");
            dataSource.UpdateTrainingCoord();
            TrainingPerceptron(Perceptron, dataSource.dataListX, dataSource.dataListY, dataSource.dataListX.Length, dataSource.idColorList);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            List<int> result = new List<int>();
            for (int i = 0; i < dataSource.blankDataListX.Length; i++)
            {
                double[] point = new double[2] { dataSource.blankDataListX[i], dataSource.blankDataListY[i] };
                result.Add((PerdictPoint(Perceptron, point, 2)));
            }
            for (int i = 0; i < result.Count; i++)
            {
                dataSource.ColorPoint(i, result[i]);
            }
            DestroyPerceptron(Perceptron);
            Perceptron = IntPtr.Zero;
        }
        
        if (Perceptron == IntPtr.Zero)
        {
            dataSource.Predict.interactable = false;
        }
        else
        {
            dataSource.Predict.interactable = true;
        }
    }


}