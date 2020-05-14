using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Data : MonoBehaviour
{

    [SerializeField] private Transform container;
    public Button Predict;

    [SerializeField] private Toggle[] _toggles;
    
    public int currentColor {  get { return _currentColor; } set { _currentColor = value; } }
    private int _currentColor;
    public int size = 0;
    public bool centered = false;
    public int offsetcentered = 0;
    
    
    // Training
    public double[] dataListX;
    public double[] dataListY;
    public double[] idColorList;

    //Real
    public double[] blankDataListX ;
    public double[] blankDataListY ;
    public double[] blankidColorList ;
    
    public double[] temp ;
    private List<Vector2> postaken = new List<Vector2>();
    public List<GameObject> _dataOjects = new List<GameObject>();
    public List<GameObject> _blankDataOjects = new List<GameObject>();
    //public List<Point> _pointList = new List<Point>();
    public List<Vector2> _testList = new List<Vector2>();

    public enum DataConfig    
    {
        RealLinS,
        Soft,
        XOR,
        Cross,
        MultiClassS,
        MultiClassSoft,
        MultiClassHard,
    }
    
    [Header("Data-setup")]
    [SerializeField]
    public DataConfig _dataconfig;

    [Header("Display")] 
    [SerializeField] 
    private GameObject _plot; 
    public List<Material> _colors = new List<Material>();
    public List<Material> _brightColors = new List<Material>();
    

    
    public void SelectColor(bool value)
    {
        Toggle source = EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();
        int i = 0;
       
        foreach (Toggle t in _toggles)
        {
            if (t != source)
            {
                t.isOn = false;
                t.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {               
                currentColor = i;
                t.transform.GetChild(0).gameObject.SetActive(true);
            }

            i++;
        }
    }

    public void SetDataTestType(int val)
    {
        _dataconfig = (DataConfig)val;
        if (val > 3)
        {
            _toggles[2].gameObject.SetActive(true);
        }
        else
        {
            
            _toggles[2].gameObject.SetActive(false);
            if (_toggles[2].isOn)
            {            
                currentColor = 0;
                _toggles[0].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        CreateData();
    }
    
    public void AddData(GameObject plot)
    {
        Vector3 p = plot.transform.position;

        temp= new double[dataListX.Length+1];

        idColorList = Copy(idColorList);

        dataListX = Copy(dataListX);

        dataListY = Copy(dataListY);
        InstantiateData(p.x+offsetcentered, p.z+offsetcentered, _colors[currentColor], currentColor, temp.Length-1);
    }

    private double[] Copy(double[] _in)
    {
        double[] _out = new double[_in.Length+1];
        for (int i = 0; i < _in.Length; i++)
        {
            _out[i]=_in[i];
        }
        return _out;
    }
    private void InstantiateData(float x,float y, Material m, int idColor,int id)
    {
        postaken.Add(new Vector2(x,y));
        x = x- offsetcentered;
        y = y- offsetcentered;
        GameObject p = Instantiate(_plot, new Vector3(x, 1f, y), Quaternion.identity,container);
        p.GetComponent<MeshRenderer>().material = m;
        
        dataListX[id] = x;
        dataListY[id] = y;
        switch (idColor)
        {
            case 0:
                idColorList[id]=-1;
                p.GetComponent<PlotData>().colorId = idColor;
                p.GetComponent<PlotData>().normalizedColorId = -1;
                break;
            case 1:
                idColorList[id]=1;
                p.GetComponent<PlotData>().colorId = idColor;
                p.GetComponent<PlotData>().normalizedColorId = 1;
                break;
            case 2:
                idColorList[id]=2;
                p.GetComponent<PlotData>().colorId = idColor;
                p.GetComponent<PlotData>().normalizedColorId = 2;
                break;
        }
        _dataOjects.Add(p); 
    }

    private void InstantiateBlankData(int x, int y, Material m, int id)
    {
        x = x- offsetcentered;
        y = y- offsetcentered;
        GameObject p = Instantiate(_plot, new Vector3(x, 0, y), Quaternion.identity,container);
        
        p.GetComponent<MeshRenderer>().material = m;
        blankDataListX[id] = x;
        blankDataListY[id] = y;
        blankidColorList[id]=4;
        _blankDataOjects.Add(p);
    }
    
    private void CreateDataTest()
    {
        _testList.Add(new Vector2(5, 5));
        _testList.Add(new Vector2(20, 20));
        _testList.Add(new Vector2(11, 15));
    }

    public void CreateData()
    {
        if (size == 0)
        {
            Debug.Log("size can't be 0");
            return;
        }
        if (centered)
        {
            offsetcentered = size/2;
        }
        else
        {
            offsetcentered = 0;
        }
        CreateDataTest();

        if (_dataOjects.Count > 0)
        {
            foreach (GameObject g in _dataOjects)
            {
                DestroyImmediate(g);
            }
            _dataOjects.Clear();
        }
               
        if (_blankDataOjects.Count > 0)
        {
            foreach (GameObject g in _blankDataOjects)
            {
                DestroyImmediate(g);
            }
            _blankDataOjects.Clear();
        }
        postaken.Clear();
        
        int numred, numblue, numgreen,x,y,alpha,beta;
        switch (_dataconfig)
        {
            /*
            case DataConfig.SimpleLinS:
                numred = 2;
                numblue = 2;
                dataListX = new double[numred+numblue+2];
                dataListY = new double[numred+numblue+2];
                idColorList = new double[numred+numblue+2];
                for (int i = 0; i <= numred; i++)
                {
                    x = Random.Range(0,(size/2));
                    y = Random.Range(0, size-1);
                    while (postaken.Contains(new Vector2(x,y)))
                    {
                        x = Random.Range(0, (size/2));
                        y = Random.Range(0, size-1);
                    }
                    InstantiateData(x,y,_colors[0], 0,i);
                }
                for (int i = 0; i <= numblue; i++)
                {
                    x = Random.Range((size/2)+1, size-1);
                    y = Random.Range((size/2)+1, size-1);
                    while (postaken.Contains(new Vector2(x,y)))
                    {
                        x = Random.Range(0, (size/2));
                        y = Random.Range(0, size-1);
                    }
                    InstantiateData(x,y,_colors[1], 1,i+numred+1);
                }
                break;
            */
            case DataConfig.RealLinS:
                numred = 5;
                numblue = 5;
                dataListX = new double[numred + numblue + 2];
                dataListY = new double[numred + numblue + 2];
                idColorList = new double[numred + numblue + 2];
                int seedx1 = Random.Range(1, size-1);
                int seedy1 = Random.Range(1, size-1);
                int seedx2 = Random.Range(1, size-1);
                int seedy2 = Random.Range(1, size-1);
                int count = 0;
                
                while (Mathf.Sqrt(((seedx2 - seedx1)*(seedx2 - seedx1)) + ((seedy2 - seedy1)*(seedy2 - seedy1)))<8 || ((seedx2 - seedx1)^2) + ((seedy2 - seedy1)^2)==0)
                {                     
                    seedx2 = Random.Range(1, size-1);
                    seedy2 = Random.Range(1, size-1);
                    count++;
                }
                for (int i = 0; i <= numred; i++)
                {
                    x = Mathf.Clamp(Random.Range(-2,2)+seedx1,0,size-1);
                    y = Mathf.Clamp(Random.Range(-2,2)+seedy1,0,size-1);
                    while (postaken.Contains(new Vector2(x,y)))
                    {
                        x = Mathf.Clamp(Random.Range(-2,2)+seedx1,0,size-1);
                        y = Mathf.Clamp(Random.Range(-2,2)+seedy1,0,size-1);
                    }
                    InstantiateData(x,y,_colors[0], 0,i);
                }
                for (int i = 0; i <= numblue; i++)
                {
                    x = Mathf.Clamp(Random.Range(-2,2)+seedx2,0, size-1);
                    y = Mathf.Clamp(Random.Range(-2,2)+seedy2,0,size-1);
                    while (postaken.Contains(new Vector2(x,y)))
                    {
                        x = Mathf.Clamp(Random.Range(-2,2)+seedx2,0, size-1);
                        y = Mathf.Clamp(Random.Range(-2,2)+seedy2,0,size-1);
                    }
                    InstantiateData(x,y,_colors[1], 1,i+numred+1);
                }
                break;
            case DataConfig.Soft:
                numred = 15;
                numblue = 15;
                dataListX = new double[numred+numblue+2];
                dataListY = new double[numred+numblue+2];
                idColorList = new double[numred+numblue+2];
                alpha = Random.Range(1, 3);
                beta = Random.Range(1, (size)/4);
                for (int i = 0; i <= numred; i++)
                {
                    x = Random.Range(0, size);
                    y = Mathf.Clamp(Random.Range(0, ((alpha * x) + beta))+Random.Range(0,2), 0, size-1);
                    while (postaken.Contains(new Vector2(x, y)))
                    {
                        x = Random.Range(0, size);
                        y = Mathf.Clamp(Random.Range(0, ((alpha * x) + beta))+Random.Range(0,2), 0, size-1);
                    }

                    InstantiateData(x, y, _colors[0], 0,i);
                }
                for (int i = 0; i <= numblue; i++)
                {
                    x = Random.Range(0, (size-beta)/alpha);
                    y = Random.Range(((alpha*x)+beta)-Random.Range(0,4),size-1);
                    while (postaken.Contains(new Vector2(x,y)))
                    {
                        x = Random.Range(0, (size-beta)/alpha);
                        y = Random.Range(((alpha*x)+beta)-Random.Range(0,4),size-1);
                    }                   
                    InstantiateData(x,y,_colors[1], 1,i+numred+1);
                }
                break;
            case DataConfig.XOR:
                dataListX = new double[4];
                dataListY = new double[4];
                idColorList = new double[4];
                InstantiateData(size-1,0,_colors[0],0,0);
                InstantiateData(0,size-1,_colors[0],0,1);
                InstantiateData(0,0,_colors[1],1,2);
                InstantiateData(size-1,size-1,_colors[1],1,3);
                break;
            case DataConfig.Cross:
                numred = 20;
                numblue = 20;
                dataListX = new double[numred+numblue+2];
                dataListY = new double[numred+numblue+2];
                idColorList = new double[numred+numblue+2];
                for (int i = 0; i <= numred; i++)
                {
                    x = Random.Range(0, size);
                    if (x <= (size)/4 || x >= (3*size)/4)
                    {
                        y = Random.Range((size)/4, (3*size)/4); 
                    }
                    else
                    {
                        y = Random.Range(0, size);  
                    }
                    
                    int counter = 0;
                    while (postaken.Contains(new Vector2(x, y)) && counter<size)
                    {
                        x = Random.Range(0, size);
                        if (x <= (size)/4 || x >= (3*size)/4)
                        {
                            y = Random.Range((size)/4, (3*size)/4); 
                        }
                        else
                        {
                            y = Random.Range(0, size);  
                        }
                        counter++;
                    }

                    InstantiateData(x, y, _colors[0], 0,i);
                }
                for (int i = 0; i <= numblue; i++)
                {
                    x = Random.Range(0, size);
                    y = Random.Range(0, size); 
                    if (x > size/2 && y > size/2)
                    {
                        x = Random.Range((3*size)/4, size);
                        y = Random.Range((3*size)/4, size);
                    }
                    else if (x < size/2 && y < size/2)
                    {
                        x = Random.Range(0, (size)/4);
                        y = Random.Range(0, (size)/4);  
                    }
                    else if (x > size/2 && y < size/2)
                    {
                        x = Random.Range((3*size)/4, size);
                        y = Random.Range(0, (size)/4);  
                    }
                    else 
                    {
                        x = Random.Range(0, (size)/4);
                        y = Random.Range((3*size)/4, size);  
                    }
                    int counter = 0;
                    while (postaken.Contains(new Vector2(x,y)) && counter<size)
                    {
                        x = Random.Range(0, size);
                        y = Random.Range(0, size); 
                        if (x > size/2 && y > size/2)
                        {
                            x = Random.Range((3*size)/4, size);
                            y = Random.Range((3*size)/4, size);
                        }
                        else if (x < size/2 && y < size/2)
                        {
                            x = Random.Range(0, (size)/4);
                            y = Random.Range(0, (size)/4);  
                        }
                        else if (x > size/2 && y < size/2)
                        {
                            x = Random.Range((3*size)/4, size);
                            y = Random.Range(0, (size)/4);  
                        }
                        else 
                        {
                            x = Random.Range(0, (size)/4);
                            y = Random.Range((3*size)/4, size);  
                        }
                        counter++;
                    }                   
                    InstantiateData(x,y,_colors[1], 1,i+numred+1);
                }
                break;
            case DataConfig.MultiClassSoft:
                numred = 20;
                numblue = 20;
                numgreen = 20;
                dataListX = new double[numred+numblue+3+numgreen];
                dataListY = new double[numred+numblue+3+numgreen];
                idColorList = new double[numred+numblue+3+numgreen];
                for (int i = 0; i <= numred; i++)
                {
                    x = Random.Range(0, size);
                    if (x < size/2)
                    {
                        y = Random.Range(0, x);
                    }
                    else
                    {
                        y = Random.Range(0, -x+size);
                    }
                    int counter = 0;
                    while (postaken.Contains(new Vector2(x, y)) && counter<size)
                    {
                        x = Random.Range(0, size);
                        if (x < size/2)
                        {
                            y = Random.Range(0, x);
                        }
                        else
                        {
                            Random.Range(0, -x+size);
                        }
                        counter++;
                    }

                    InstantiateData(x,y,_colors[0], 0,i);
                }
                for (int i = 0; i <= numblue; i++)
                {
                    x = Random.Range(0, size/2);
                    y = Random.Range(x, size);
                    
                    int counter = 0;
                    while (postaken.Contains(new Vector2(x, y)) && counter<size)
                    {
                        x = Random.Range(0, size/2);
                        y = Random.Range(x, size);
                        counter++;
                    }                  
                    InstantiateData(x,y,_colors[1], 1,i+numred);
                }
                for (int i = 0; i <= numgreen; i++)
                {
                    x = Random.Range(size/2, size);
                    y = Random.Range(-x+size, size);
                    
                    int counter = 0;
                    while (postaken.Contains(new Vector2(x, y)) && counter<size)
                    {
                        x = Random.Range(size/2, size);
                        y = Random.Range(-x+size, size);
                        counter++;
                    }                  
                    InstantiateData(x,y,_colors[2], 2,i+numred+numblue+2);
                }
                break;
            case DataConfig.MultiClassHard:
                int offsetX = 0;
                int offsetY = 0;
                int num = 80;
                dataListX = new double[num];
                dataListY = new double[num];
                idColorList = new double[num];
                int num2 = 5;
                int color = Random.Range(0,3);
                
                for (int i = 0; i < ((size)/4)-1; i++)
                {
                    for (int j = 0; j < ((size)/4)-1; j++)
                    {                                                              
                        for (int I = 0; I < num2; I++)
                        {
                            x = Random.Range(offsetX, offsetX + (size)/4);
                            y = Random.Range(offsetY, offsetY + (size)/4);
                            int counter = 0;
                            while (postaken.Contains(new Vector2(x, y)) && counter < size)
                            {
                                x = Random.Range(offsetX, offsetX + (size)/4);
                                y = Random.Range(offsetY, offsetY + (size)/4);
                                counter++;
                            }
                            InstantiateData(x, y, _colors[color], color,i*j);
                        }
                        color = (color + 1) % 3;
                        offsetX += (size)/4;
                    }
                    offsetY += (size)/4;
                    offsetX = 0;
                }
                break;
            case DataConfig.MultiClassS:
                numred = 5;
                numblue = 5;
                numgreen = 5;
                dataListX = new double[numred+numblue+3+numgreen];
                dataListY = new double[numred+numblue+3+numgreen];
                idColorList = new double[numred+numblue+3+numgreen];
                int Q1, Q2, Q3,upperX=0,lowerX=0,upperY=0,lowerY=0;
                Q1 = Random.Range(1, 5);
                switch (Q1)
                {
                    case 1:
                        upperX = size / 2;
                        upperY = size / 2;
                        lowerX = 0;
                        lowerY = 0;
                        break;
                    case 2:
                        upperX = size / 2;
                        upperY = size;
                        lowerX = 0;
                        lowerY = size / 2;
                        break;
                    case 3:
                        upperX = size;
                        upperY = size;
                        lowerX = size / 2;
                        lowerY = size / 2;
                        break;
                    case 4:
                        upperX = size;
                        upperY = size/ 2;
                        lowerX = size / 2;
                        lowerY = 0;
                        break;
                }
                upperX = upperX - 2;
                upperY = upperY - 2;
                lowerX = lowerX + 2;
                lowerY = lowerY + 2; 
                for (int i = 0; i <= numred; i++)
                {
                    x = Random.Range(lowerX, upperX-2);
                    y = Random.Range(lowerY, upperY-2);
                    
                    int counter = 0;
                    while (postaken.Contains(new Vector2(x, y)) && counter<100)
                    {
                        x = Random.Range(lowerX, upperX-2);
                        y = Random.Range(lowerY, upperY-2);
                        counter++;
                    }                  
                    InstantiateData(x,y,_colors[0], 0,i);
                }
                Q2 = Random.Range(1, 5);
                while (Q2==Q1)
                {
                    Q2 = Random.Range(1, 5);
                }
                switch (Q2)
                {
                    case 1:
                        upperX = size / 2;
                        upperY = size / 2;
                        lowerX = 0;
                        lowerY = 0;
                        break;
                    case 2:
                        upperX = size / 2;
                        upperY = size;
                        lowerX = 0;
                        lowerY = size / 2;
                        break;
                    case 3:
                        upperX = size;
                        upperY = size;
                        lowerX = size / 2;
                        lowerY = size / 2;
                        break;
                    case 4:
                        upperX = size;
                        upperY = size/ 2;
                        lowerX = size / 2;
                        lowerY = 0;
                        break;
                }
                upperX = upperX - 2;
                upperY = upperY - 2;
                lowerX = lowerX + 2;
                lowerY = lowerY + 2; 
                for (int i = 0; i <= numblue; i++)
                {
                    x = Random.Range(lowerX, upperX-2);
                    y = Random.Range(lowerY, upperY-2);
                    
                    int counter = 0;
                    while (postaken.Contains(new Vector2(x, y)) && counter<100)
                    {
                        x = Random.Range(lowerX, upperX-2);
                        y = Random.Range(lowerY, upperY-2);
                        counter++;
                    }                
                    InstantiateData(x,y,_colors[1], 1,i+numred+1);
                }
                
                Q3 = Random.Range(1, 5);
                while (Q3==Q1|| Q3==Q2)
                {
                    Q3 = Random.Range(1, 5);
                }
                switch (Q3)
                {
                    case 1:
                        upperX = size / 2;
                        upperY = size / 2;
                        lowerX = 0;
                        lowerY = 0;
                        break;
                    case 2:
                        upperX = size / 2;
                        upperY = size;
                        lowerX = 0;
                        lowerY = size / 2;
                        break;
                    case 3:
                        upperX = size;
                        upperY = size;
                        lowerX = size / 2;
                        lowerY = size / 2;
                        break;
                    case 4:
                        upperX = size;
                        upperY = size/ 2;
                        lowerX = size / 2;
                        lowerY = 0;
                        break;
                }

                upperX = upperX - 2;
                upperY = upperY - 2;
                lowerX = lowerX + 2;
                lowerY = lowerY + 2; 
                for (int i = 0; i <= numgreen; i++)
                {
                    x = Random.Range(lowerX, upperX-2);
                    y = Random.Range(lowerY, upperY-2);
                    
                    int counter = 0;
                    while (postaken.Contains(new Vector2(x, y)) && counter<100)
                    {
                        x = Random.Range(lowerX+2, upperX-2);
                        y = Random.Range(lowerY+2, upperY-2);
                        counter++;
                    }                 
                    InstantiateData(x,y,_colors[2], 2,i+numred+numblue+2);
                }
                
                
                break;
        }

        
        // CREATE BLANK DATA

        int blankdata = size*size;
        blankDataListX = new double[blankdata];
        blankDataListY = new double[blankdata];
        blankidColorList = new double[blankdata];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                InstantiateBlankData(i,j,_colors[4],(i*size) + j);
            }
        }
        //Debug.Log(dataListX[0]);
    }
    
    public void ColorPoint(int id, int color)
    {
        GameObject g = _blankDataOjects[id]; 
        switch (color)
        {
            case -1:
                    g.GetComponent<PlotData>().colorId = 0;
                g.GetComponent<PlotData>().colorId = -1;
                _blankDataOjects[id].GetComponent<Renderer>().material = _colors[0];
                break;
            case 1:
                g.GetComponent<PlotData>().colorId = 1;
                g.GetComponent<PlotData>().colorId = 1;
                _blankDataOjects[id].GetComponent<Renderer>().material = _colors[1];
                break;
        }
    }

    public void UpdateTrainingCoord()
    {
        for (int i = 0; i < dataListX.Length; i++)
        {
            Vector3 pos = _dataOjects[i].transform.position;
            dataListX[i] = pos.x;
            dataListY[i] = pos.z;
        }
    }

    public void ResetBlanks()
    {
        foreach (GameObject g in _blankDataOjects)
        {
            g.GetComponent<Renderer>().material = _colors[4];
        }
    }
}

[Serializable]
public class Point
{
    public Vector2 position;
    public int idColor; // 0 = bleu, 1 = rouge, 2 = vert, 3 = jaune

    public Point(Vector2 _position, int _idColor)
    {
        position = _position;
        idColor = _idColor;
    }
}