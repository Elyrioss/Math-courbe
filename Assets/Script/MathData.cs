using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using Voronoi2;
using Random = UnityEngine.Random;

public class MathData : MonoBehaviour
{
    #region CreatePoints
    
    [SerializeField] private Transform container;
   
    public int currentColor {  get { return _currentColor; } set { _currentColor = value; } }
    private int _currentColor;
    public int size = 0;
    public bool centered = false;
    private int offsetcentered = 0;
    
    public Camera _main;
    public Camera _3Dcamera;
    
    public List<Point2> PointList = new List<Point2>();
    public List<Point3> PointList3D = new List<Point3>();
    
    public double[] temp ;
    private List<Vector2> postaken = new List<Vector2>();
    private List<Vector3    > postaken3D = new List<Vector3>();
    public List<GameObject> _dataOjects = new List<GameObject>();
    private List<GameObject> _blankDataOjects = new List<GameObject>();

    [Header("Display")] 
    [SerializeField] 
    private GameObject _plot; 
    [SerializeField] 
    private GameObject _plot2; 
    [SerializeField] 
    public List<Material> _colors = new List<Material>();
    [SerializeField] 
    public List<Material> _brightColors = new List<Material>();

    public void AddData(GameObject plot)
    {
        Vector3 p = plot.transform.position;
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
        
        PointList.Add(new Point2(p,new Vector2(x,y)));
        
        p.GetComponent<PlotData>().colorId = idColor;
        p.GetComponent<PlotData>().normalizedColorId = -1;

        _dataOjects.Add(p); 
    }

    private void InstantiateData(float x,float y, float z, Material m, int idColor,int id)
    {
        postaken3D.Add(new Vector3(x,y,z));
        x = x- offsetcentered;
        //y = y- offsetcentered;
        z = z- offsetcentered;
        GameObject p = Instantiate(_plot, new Vector3(x, y, z), Quaternion.identity,container);
        p.GetComponent<MeshRenderer>().material = m;
        
        PointList3D.Add(new Point3(p,new Vector3(x,y,z)));
        
        p.GetComponent<PlotData>().colorId = idColor;
        p.GetComponent<PlotData>().normalizedColorId = -1;

        _dataOjects.Add(p); 
    }
    
    private void InstantiateBlankData(int x, int y, Material m, int id)
    {
        x = x- offsetcentered;
        y = y- offsetcentered;
        GameObject p = Instantiate(_plot2, new Vector3(x, 0, y), Quaternion.identity,container);
        
        p.GetComponent<MeshRenderer>().material = m;
        _blankDataOjects.Add(p);
    }

    public void DeleteAll()
    {
        ClearDiagrames();
        if (centered)
        {
            offsetcentered = size/2;
        }
        else
        {
            offsetcentered = 0;
        }
        
        if (_dataOjects.Count > 0)
        {
            foreach (GameObject g in _dataOjects)
            {
                DestroyImmediate(g);
            }
            _dataOjects.Clear();
            PointList.Clear();
            PointList3D.Clear();
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
        postaken3D.Clear();
    }

    public void CreateData()
    {
        _3Dcamera.gameObject.SetActive(false);
        _main.gameObject.SetActive(true);
        if (size == 0)
        {
            Debug.Log("size can't be 0");
            return;
        }

        DeleteAll();
        
        int numred, numblue, numgreen,x,y,alpha,beta;          
        
        for (int i = 0; i < 10; i++)
        {
            x = Random.Range(1, size-1);
            y = Random.Range(1, size-1);
            while (postaken.Contains(new Vector2(x,y)))
            {
                x = Random.Range(1, size-1);
                y = Random.Range(1, size-1);
            }
            InstantiateData(x,y,_colors[0], 0,i);
        }
        
        
    }
    
    public void CreateData3D()
    {
        _3Dcamera.gameObject.SetActive(true);
        _main.gameObject.SetActive(false);
        
        if (size == 0)
        {
            Debug.Log("size can't be 0");
            return;
        }
        DeleteAll();
        
        int numred, numblue, numgreen,x,y,z,alpha,beta;          
        
        for (int i = 0; i < 10; i++)
        {
            x = Random.Range(1, size-1);
            y = Random.Range(1, size-1);
            z = Random.Range(1, size-1);
            while (postaken3D.Contains(new Vector3(x,y,z)))
            {
                x = Random.Range(1, size-1);
                y = Random.Range(1, size-1);
                z = Random.Range(1, size-1);
            }
            InstantiateData(x,y,z,_colors[0], 0,i);
        }
        
        
    }
    
    #endregion
    
    public List<Point2> result = new List<Point2>();
    const int MaxPointsNumber = 14;    
    int minPoint, maxPoint, currPoint;
    private int n;


    public void TestAngle()
    {
        Debug.Log(Angle(new Vector2(0,2), new Vector2(0,1)));
    }
    
    public void Jarvis()
    {       
        ClearDiagrames();
        result.Clear();
        jarvis(3);
    }

    private void jarvis(int color)
    {
        
        PointList.Sort((a,b)=> a._pos.x.CompareTo(b._pos.x));   
        minPoint = 0;
        maxPoint = PointList.Count-1;
        
        result.Add(PointList[minPoint]); //add point to convex list
        currPoint = minPoint;
        PointList[currPoint]._object.GetComponent<MeshRenderer>().material = _colors[color];

        int sameX=-1;
        
        while (true)
        {
            for (int i = 0; i < PointList.Count; i++)
            {
                if (i != currPoint && !result.Contains(PointList[i]) && PointList[i]._pos.x == PointList[currPoint]._pos.x)
                {
                    result.Add(PointList[i]); //add point to convex list
                    currPoint = i;
                    PointList[currPoint]._object.GetComponent<MeshRenderer>().material = _colors[color];
                    sameX = i;
                }
            }

            if (sameX == -1)
            {
                break;
            }
            else
            {
                sameX = -1;
            }
        }
        result.Sort((a,b)=> a._pos.y.CompareTo(b._pos.y));   
        float last = -10;
        int breaker = 0;
        do
        {
            breaker++;
            int temp=0;
            List<IndexedAngle> Angles = new List<IndexedAngle>();
            for (int i = 0; i < PointList.Count; i++)
            {
                if (currPoint != i)
                {                 
                    float A = Angle(PointList[currPoint]._pos, PointList[i]._pos);
                    if (last <= A)
                    {
                        Angles.Add(new IndexedAngle(A,i,Vector2.Distance(PointList[currPoint]._pos, PointList[i]._pos)));
                    }                   
                }              
            }
          
            Angles.Sort((a,b)=> a.Angle.CompareTo(b.Angle));
            Debug.Log(Angles[0].Angle);
            IndexedAngle IA = Angles[0];
            for (int i = 1; i < Angles.Count; i++)
            {
                if (Math.Abs(Angles[0].Angle - Angles[i].Angle) < 0.1f && Angles[i].distance<Angles[0].distance)
                {
                    Debug.Log("Same Angle");
                    IA = Angles[i];
                }
            }
            
            temp = IA.index;
            last = IA.Angle;
            if (result.Contains(PointList[temp]))
            {
                result.Add(PointList[temp]);
                Debug.Log("double");
                break;
            }

            PointList[temp]._object.GetComponent<MeshRenderer>().material = _colors[color];
            result.Add(PointList[temp]);
            currPoint = temp;
            if (breaker > 200 )
            {
                Debug.Log("overload");
                break;
            }
        } while (currPoint != minPoint);
        Debug.Log("start point");
        for (int i = 0; i < result.Count-1; i++)
        {
            DrawLine(result[i]._object.transform.position,result[i+1]._object.transform.position,Color.green,0);
        }
    }
    public float Angle(Vector2 P1,Vector2 P2)
    {
        float Angle = -1;
        Vector2 FPoint = new Vector2(P1.x, P2.y);
        float A = Vector2.Distance(FPoint, P2);
        float B = Vector2.Distance(FPoint, P1);
        Angle = Mathf.Rad2Deg*Mathf.Atan(A/B);
        
        
        if (P1.y >= P2.y && P1.x<=P2.x)
        {

            Angle = 180-Angle;
            return Angle;
        }
        
        if (P1.y >= P2.y && P1.x>=P2.x)
        {

            Angle = 180+Angle;
            return Angle;
        }
        
        if (P1.y <= P2.y && P1.x>=P2.x)
        {

            Angle = 360-Angle;
            return Angle;
        }
        return Angle;
    }

    public List<GroupPoints> Gp = new List<GroupPoints>();
    public void Voronoi()
    {
        
        ClearDiagrames();
        
        Voronoi voroObject = new Voronoi (0.001f);
        
        double[] xVal = new double[PointList.Count];
        double[] yVal = new double[PointList.Count];

        for (int i = 0; i < PointList.Count; i++)
        {
            xVal[i] = PointList[i]._pos.x;
            yVal[i] = PointList[i]._pos.y;
        }

        
        List<GraphEdge> ge = voroObject.generateVoronoi ( xVal, yVal, -size, size, -size, size );
        foreach (GraphEdge GE in ge)
        {
            DrawLine(new Vector3((float)GE.x1,1,(float)GE.y1),new Vector3((float)GE.x2,1,(float)GE.y2),Color.red,0);
        }
        /*
        result.Clear();
        Gp.Clear();
        
        List<DistancedPoints> Dp = new List<DistancedPoints>(); 
        foreach (Point2 p in PointList)
        {
            Dp.Clear();
            foreach (Point2 P in PointList)
            {
                if (p != P)
                {
                    Dp.Add(new DistancedPoints(P,Vector2.Distance(p._pos,P._pos)));
                }
            }
            Dp.Sort((a,b)=> a._distance.CompareTo(b._distance));
            Gp.Add(new GroupPoints(p,Dp[0]._point2,Dp[1]._point2));
        }

        List<Vector3> Centers = new List<Vector3>();
        for (int i = 0; i < Gp.Count; i++)
        {
            GroupPoints G= Gp[i];
            Centers.Add(G.center);
            DrawLine(G.center,G.Points[0]._object.transform.position,Color.green,0);
            DrawLine(G.center,G.Points[1]._object.transform.position,Color.green,0);
            DrawLine(G.center,G.Points[2]._object.transform.position,Color.green,0);
        }
        
        List<DistancedPoints> DistCenters = new List<DistancedPoints>();
        foreach (Vector3 c in Centers)
        {
            DistCenters.Clear();
            foreach (Vector3 C in Centers)
            {
                if (C != c)
                {
                    DistCenters.Add(new DistancedPoints(C,Vector3.Distance(c,C)));
                }
            }
            DistCenters.Sort((a,b)=> a._distance.CompareTo(b._distance));
            //DrawLine(c,DistCenters[0]._point3,Color.red,0);
            //DrawLine(c,DistCenters[1]._point3,Color.red,0);
            //DrawLine(c,DistCenters[2]._point3,Color.red,0);
        }
        */
    }

    
    public List<GroupPoints> groupPoints = new List<GroupPoints>();
    public void Delaunay()
    {
        groupPoints.Clear();
        //result.Clear();
        ClearDiagrames();
        
        //jarvis(3);
        
        foreach (Point2 p in PointList)
        {
            foreach (Point2 p2 in PointList)
            {
                foreach (Point2 p1 in PointList)
                {
                    if (p != p1 && p1 != p2 && p != p2)
                    {
                        float x1 = p._pos.x;
                        float x2 = p1._pos.x;
                        float x3 = p2._pos.x;
                        float y1 = p._pos.y;
                        float y2 = p1._pos.y;
                        float y3 = p2._pos.y;
                        if (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2) != 0)
                        {
                            groupPoints.Add(new GroupPoints(p,p1,p2));
                        }
                        
                    }
                }
            }
        }
        
        
        
        List<GroupPoints> toRemove = new List<GroupPoints>();
        foreach (GroupPoints gp in groupPoints)
        {
            foreach (Point2 P in PointList)
            {
                if (Vector3.Distance(new Vector3(P._pos.x, 1, P._pos.y), gp.center) <= gp.radius)
                {
                    if (P != gp.Points[0] && P != gp.Points[1] && P != gp.Points[2])
                    {
                        toRemove.Add(gp);
                    }
                    
                }
            }
        }

        
        /*foreach (GroupPoints gp in groupPoints)
        {
            foreach (GroupPoints GP in groupPoints)
            {
                if (gp != GP && gp.center == GP.center && gp.radius == GP.radius)
                {
                    toRemove.Add(gp);
                }
            }
        }*/
        
        foreach (GroupPoints tr in toRemove)
        {
            groupPoints.Remove(tr);
        }
       
        for (int i = 0; i < groupPoints.Count; i++)
        {
            Debug.Log("found nan");
            if (float.IsNaN(groupPoints[i].radius) || groupPoints[i].radius>20)
            {
                Debug.Log("found nan");
                groupPoints.Remove(groupPoints[i]);
            }

            if (groupPoints[i].center.x < -size && groupPoints[i].center.x > size && groupPoints[i].center.z < -size &&
                groupPoints[i].center.z > size)
            {
                groupPoints.Remove(groupPoints[i]);
            }
        }
        
        foreach (GroupPoints G in groupPoints)
        {
            DoCircle(G.radius, G.center, Color.red);
            DrawLine(G.Points[0]._object.transform.position,G.Points[1]._object.transform.position,Color.green,0);
            DrawLine(G.Points[0]._object.transform.position,G.Points[2]._object.transform.position,Color.green,0);
            DrawLine(G.Points[1]._object.transform.position,G.Points[2]._object.transform.position,Color.green,0);
        }
    }
    
    public void Delaunay3D()
    {
        groupPoints.Clear();
        //result.Clear();
        ClearDiagrames();
        
        //jarvis(3);
        
        foreach (Point3 p in PointList3D)
        {
            foreach (Point3 p1 in PointList3D)
            {
                foreach (Point3 p2 in PointList3D)
                {
                    if (p != p1 && p1 != p2 && p != p2)
                    {
                        float x1 = p._pos.x;
                        float x2 = p1._pos.x;
                        float x3 = p2._pos.x;
                        float y1 = p._pos.y;
                        float y2 = p1._pos.y;
                        float y3 = p2._pos.y;
                        if (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2) != 0)
                        {
                            groupPoints.Add(new GroupPoints(p,p1,p2));
                        }
                        
                    }
                }
            }
        }
        
        
        
        List<GroupPoints> toRemove = new List<GroupPoints>();
        foreach (GroupPoints gp in groupPoints)
        {
            foreach (Point3 P in PointList3D)
            {
                if (Vector3.Distance(new Vector3(P._pos.x, 1, P._pos.y), gp.center) <= gp.radius)
                {
                    if (P != gp.Points3D[0] && P != gp.Points3D[1] && P != gp.Points3D[2])
                    {
                        toRemove.Add(gp);
                    }
                    
                }
            }
        }

        
        /*foreach (GroupPoints gp in groupPoints)
        {
            foreach (GroupPoints GP in groupPoints)
            {
                if (gp != GP && gp.center == GP.center && gp.radius == GP.radius)
                {
                    toRemove.Add(gp);
                }
            }
        }*/
        
        foreach (GroupPoints tr in toRemove)
        {
            groupPoints.Remove(tr);
        }
       
        for (int i = 0; i < groupPoints.Count; i++)
        {
            if (float.IsNaN(groupPoints[i].radius) || groupPoints[i].radius>20)
            {
                groupPoints.Remove(groupPoints[i]);
            }

            if (groupPoints[i].center.x < -size && groupPoints[i].center.x > size && groupPoints[i].center.z < -size &&
                groupPoints[i].center.z > size)
            {
                groupPoints.Remove(groupPoints[i]);
            }
        }
        
        foreach (GroupPoints G in groupPoints)
        {
            /*DrawLine(G.Points3D[0]._object.transform.position,G.Points3D[1]._object.transform.position,Color.green,0);
            DrawLine(G.Points3D[0]._object.transform.position,G.Points3D[2]._object.transform.position,Color.green,0);
            DrawLine(G.Points3D[1]._object.transform.position,G.Points3D[2]._object.transform.position,Color.green,0);*/
            GameObject t = DrawTriangle(G.center,new []{G.Points3D[0]._object.transform.position,G.Points3D[1]._object.transform.position,G.Points3D[2]._object.transform.position});
            G.Points3D[0].triangles.Add(t);
            G.Points3D[1].triangles.Add(t);
            G.Points3D[2].triangles.Add(t);
        }
            
        /*List<Point3> toRemove2 = new List<Point3>();
        foreach (Point3 p3 in PointList3D)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            bool temp= false,temp1= false,temp2= false,temp3= false,temp4= false,temp5 = false;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp = true;
                }             
            }
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp1 = true;
                }             
            }
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp2 = true;
                }             
            }
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp3 = true;
                }             
            }
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp4 = true;
                }             
            }
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp5 = true;
                }             
            }
            
            temp = temp && temp1 && temp2 && temp3 && temp4 && temp5;
            
            if (temp)
            {
                Debug.Log(temp +" / "+temp1 +" / "+temp2 +" / "+temp3 +" / "+temp4 +" / "+temp5);
                toRemove2.Add(p3);
            }
        }
        
        foreach (Point3 tr in toRemove2)
        {
            foreach (GameObject t in tr.triangles)
            {
                Destroy(t);
            }
        }
        
        foreach (GroupPoints G in groupPoints)
        {
            if (!toRemove2.Contains(G.Points3D[0]) && !toRemove2.Contains(G.Points3D[1]))
            {
                DrawLine(G.Points3D[0]._object.transform.position,G.Points3D[1]._object.transform.position,Color.green,0);
            }
            
            if (!toRemove2.Contains(G.Points3D[0]) && !toRemove2.Contains(G.Points3D[2]))
            {
                DrawLine(G.Points3D[0]._object.transform.position,G.Points3D[2]._object.transform.position,Color.green,0);
            }
            
            if (!toRemove2.Contains(G.Points3D[1]) && !toRemove2.Contains(G.Points3D[2]))
            {
                DrawLine(G.Points3D[1]._object.transform.position,G.Points3D[2]._object.transform.position,Color.green,0);
            }
            
        }
        
        List<GameObject> toRemove3 = new List<GameObject>();
        
        foreach (GameObject p3 in Triangles)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            bool temp= false,temp1= false,temp2= false,temp3= false,temp4= false,temp5 = false;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp = true;
                }             
            }
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp1 = true;
                }             
            }
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp2 = true;
                }             
            }
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp3 = true;
                }             
            }
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp4 = true;
                }             
            }
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 100))
            {
                if (hit.transform.name == "Triangle")
                {
                    temp5 = true;
                }             
            }
            
            temp = temp && temp1 && temp2 && temp3 && temp4 && temp5;
            
            if (temp)
            {
                Debug.Log(temp +" / "+temp1 +" / "+temp2 +" / "+temp3 +" / "+temp4 +" / "+temp5);
                toRemove3.Add(p3);
            }
        }
        
        foreach (GameObject t in toRemove3)
        {
            Destroy(t);

        }*/
        
    }
    
    private void Update()
    {
        for (int i = 0; i < PointList.Count; i++)
        {
            Vector3 p = _dataOjects[i].transform.position;
            PointList[i] = new Point2(_dataOjects[i],new Vector2(p.x,p.z));
        }
    }
    
    private List<GameObject> Lines = new List<GameObject>();
    private List<GameObject> Circles = new List<GameObject>();
    private List<GameObject> Triangles = new List<GameObject>();
    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        Lines.Add(myLine);
    }

    
    [Range(3, 256)]
    public int numSegments = 128;

    public void DoCircle(float radius,Vector3 start, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = myLine.GetComponent<LineRenderer>();
        Color c1 = color;
        lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lineRenderer.SetColors(c1, c1);
        lineRenderer.SetWidth(0.1f, 0.1f);
        lineRenderer.SetVertexCount(numSegments + 1);
        lineRenderer.useWorldSpace = false;

        float deltaTheta = (float) (2.0 * Mathf.PI) / numSegments;
        float theta = 0f;

        for (int i = 0; i < numSegments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, 0, z);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }

        Circles.Add(myLine);
        
    }


    public Material basic;
    public GameObject DrawTriangle(Vector3 start,Vector3[] newVertices)
    {
        GameObject myLine = new GameObject();
        myLine.name = "Triangle";
        //myLine.transform.position = start;
        myLine.AddComponent<MeshFilter>();
        myLine.AddComponent<MeshRenderer>();
        Mesh mesh = myLine.GetComponent<MeshFilter>().mesh;
        MeshRenderer meshrend= myLine.GetComponent<MeshRenderer>();    
        mesh.Clear();
        
        // make changes to the Mesh by creating arrays which contain the new values
        mesh.vertices = newVertices;
        mesh.triangles =  new int[] {0, 1, 2};
        meshrend.material = basic;
        
        Vector3 side1 = newVertices[1] - newVertices[0];
        Vector3 side2 = newVertices[2] - newVertices[0];

        myLine.AddComponent<MeshCollider>();
        
        Triangles.Add(myLine);
        return myLine;
    }

    public void CirclesActive(bool a)
    {
        foreach (GameObject circle in Circles)
        {
            circle.SetActive(a);
        }
    }
    
    public void LinesActive(bool a)
    {
        foreach (GameObject line in Lines)
        {
            line.SetActive(a);
        }
    }
    
    public void ClearDiagrames()
    {
        foreach (GameObject myLine in Lines)
        {
            GameObject.Destroy(myLine);
        }
        Lines.Clear();
        foreach (GameObject myLine in Circles)
        {
            GameObject.Destroy(myLine);
        }
        Circles.Clear();
        foreach (Point2 p in PointList)
        {
            p._object.GetComponent<MeshRenderer>().material = _colors[0];
        }
    }
    
}

[Serializable]
public class Point2
{
    public GameObject _object;
    public Vector2 _pos;

    public Point2(GameObject g, Vector2 pos)
    {
        _object = g;
        _pos = pos;
    }
}

[Serializable]
public class Point3
{
    public GameObject _object;
    public Vector3 _pos;
    public List<GameObject> triangles = new List<GameObject>();
    public Point3(GameObject g, Vector3 pos)
    {
        _object = g;
        _pos = pos;
    }
}

public class IndexedAngle
{
    public float Angle;
    public int index;
    public float distance;

    public IndexedAngle(float angle, int i,float _distance)
    {
        index = i;
        Angle = angle;
        distance = _distance;
    }
}

public class DistancedPoints
{
    public Vector3 _point3;
    public Point2 _point2;
    public float _distance;

    public DistancedPoints(Point2 p2, float distance)
    {
        _point2 = p2;
        _distance = distance;
    }

    public DistancedPoints(Vector3 p2, float distance)
    {
        _point3 = p2;
        _distance = distance;
    }
}

[Serializable]
public class GroupPoints
{
    public List<Point2> Points = new List<Point2>();
    public List<Point3> Points3D = new List<Point3>();
    public Vector3 center;
    public float radius;
    public GroupPoints(Point2 source, Point2 close1, Point2 close2)
    {
        Points.Add(source);
        Points.Add(close1);
        Points.Add(close2);
        Vector3 normal;
        center = CircleCenter(source._object.transform.position, close1._object.transform.position,
            close2._object.transform.position,out normal);

        radius = Vector3.Distance(new Vector3(source._pos.x, 1, source._pos.y), center);

    }
    
    public GroupPoints(Point3 source, Point3 close1, Point3 close2)
    {
        Points3D.Add(source);
        Points3D.Add(close1);
        Points3D.Add(close2);
        Vector3 normal;
        center = CircleCenter(source._object.transform.position, close1._object.transform.position,
            close2._object.transform.position,out normal);

        radius = Vector3.Distance(new Vector3(source._pos.x, source._pos.y, source._pos.z), center);

    }
    
    public static Vector3 CircleCenter(Vector3 aP0, Vector3 aP1, Vector3 aP2, out Vector3 normal)
    {
        // two circle chords
        var v1 = aP1 - aP0;
        var v2 = aP2 - aP0;

        normal = Vector3.Cross(v1, v2);
        if (normal.sqrMagnitude < 0.00001f)
            return Vector3.one * float.NaN;
        normal.Normalize();

        // perpendicular of both chords
        var p1 = Vector3.Cross(v1, normal).normalized;
        var p2 = Vector3.Cross(v2, normal).normalized;
        // distance between the chord midpoints
        var r = (v1 - v2) * 0.5f;
        // center angle between the two perpendiculars
        var c = Vector3.Angle(p1, p2);
        // angle between first perpendicular and chord midpoint vector
        var a = Vector3.Angle(r, p1);
        // law of sine to calculate length of p2
        var d = r.magnitude * Mathf.Sin(a * Mathf.Deg2Rad) / Mathf.Sin(c * Mathf.Deg2Rad);
        if (Vector3.Dot(v1, aP2 - aP1) > 0)
            return aP0 + v2 * 0.5f - p2 * d;
        return aP0 + v2 * 0.5f + p2 * d;
    }

}