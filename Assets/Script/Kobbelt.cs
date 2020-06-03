using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;


public class Kobbelt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject plot;
    public GameObject plot1;
    public GameObject plot2;
    public List<Triangle> tri = new List<Triangle>();
    public List<Vertice> verticesGlobal;
    private List<Vertice> centers = new List<Vertice>();
    private List<GameObject> _objects = new List<GameObject>();
    void Start()
    {
         
        
    }

    public void Rectangle()
    {
        Vertice v0 = new Vertice(new Vector3(0, 0, 0));
        Vertice v1 = new Vertice(new Vector3(0, 20, 0));
        Vertice v2 = new Vertice(new Vector3(20, 20, 0));
        Vertice v3 = new Vertice(new Vector3(20, 0, 0));
        Vertice v4 = new Vertice(new Vector3(0, 0, 20));
        Vertice v5 = new Vertice(new Vector3(0, 20, 20));
        Vertice v6 = new Vertice(new Vector3(20, 20, 20));
        Vertice v7 = new Vertice(new Vector3(20, 0, 20));
        
        
        v0.AddNeighbours(new List<Vertice>() { v1, v3, v4, v7, v5});
        v1.AddNeighbours(new List<Vertice>() { v0, v3, v2, v5});
        v2.AddNeighbours(new List<Vertice>() { v1, v3, v6, v7, v5});
        v3.AddNeighbours(new List<Vertice>() { v0, v1, v2, v7});
        
        v4.AddNeighbours(new List<Vertice>() { v0, v7, v5});
        v5.AddNeighbours(new List<Vertice>() { v0, v1, v2, v4, v7, v6});
        v6.AddNeighbours(new List<Vertice>() { v5, v7, v3});
        v7.AddNeighbours(new List<Vertice>() { v0, v4, v3, v2, v5, v6});
        
        verticesGlobal = new List<Vertice>() { v0, v1, v2, v3 ,v4 , v5, v6, v7};
        
        Triangle t0 = new Triangle(new List<Vertice>() { v0, v1, v2 });
        Triangle t1 = new Triangle(new List<Vertice>() { v0, v2, v3 });
        Triangle t2 = new Triangle(new List<Vertice>() { v0, v1, v5 });
        Triangle t3 = new Triangle(new List<Vertice>() { v0, v4, v5 });

        Triangle t4 = new Triangle(new List<Vertice>() { v0, v4, v7 });
        Triangle t5 = new Triangle(new List<Vertice>() { v0, v3, v7 });
        
        Triangle t6 = new Triangle(new List<Vertice>() { v2, v5, v6 });
        Triangle t7 = new Triangle(new List<Vertice>() { v2, v5, v1 });
        
        Triangle t8 = new Triangle(new List<Vertice>() { v2, v7, v6 });
        Triangle t9 = new Triangle(new List<Vertice>() { v2, v7, v3 });
        
        Triangle t10 = new Triangle(new List<Vertice>() { v6, v4, v5 });
        Triangle t11 = new Triangle(new List<Vertice>() { v6, v4, v7 });
        
        /*t0.AddNeighbours(new List<Triangle>() { t1, t2, t3 });
        t1.AddNeighbours(new List<Triangle>() { t0, t2, t3 });
        t2.AddNeighbours(new List<Triangle>() { t0, t1, t3 });
        t3.AddNeighbours(new List<Triangle>() { t0, t1, t2 });
*/
        t0.Center = TriCenter(t0);
        t1.Center = TriCenter(t1);
        t2.Center = TriCenter(t2);
        t3.Center = TriCenter(t3);
        t4.Center = TriCenter(t4);
        t5.Center = TriCenter(t5);
        t6.Center = TriCenter(t6);
        t7.Center = TriCenter(t7);
        t8.Center = TriCenter(t8);
        t9.Center = TriCenter(t9);
        t10.Center = TriCenter(t10);
        t11.Center = TriCenter(t11);
        
        
        centers.Add(t0.Center);
        centers.Add(t1.Center);
        centers.Add(t2.Center);
        centers.Add(t3.Center);
        centers.Add(t4.Center);
        centers.Add(t5.Center);
        centers.Add(t6.Center);
        centers.Add(t7.Center);
        centers.Add(t8.Center);
        centers.Add(t9.Center);
        centers.Add(t10.Center);
        centers.Add(t11.Center);
        
        tri.Add(t0);
        tri.Add(t1);
        tri.Add(t2);
        tri.Add(t3);
        tri.Add(t4);
        tri.Add(t5);
        tri.Add(t6);
        tri.Add(t7);
        tri.Add(t8);
        tri.Add(t9);
        tri.Add(t10);
        tri.Add(t11);
    }
    public void Triangle()
    {
        Vertice v0 = new Vertice(new Vector3(0, 0, 0));
        Vertice v1 = new Vertice(new Vector3(64, 0, 0));
        Vertice v2 = new Vertice(new Vector3(32f, 0, 48f));
        Vertice v3 = new Vertice(new Vector3(32f, 48f, 24f));

        v0.AddNeighbours(new List<Vertice>() { v1, v2, v3 });
        v1.AddNeighbours(new List<Vertice>() { v0, v2, v3 });
        v2.AddNeighbours(new List<Vertice>() { v0, v1, v3 });
        v3.AddNeighbours(new List<Vertice>() { v0, v1, v2 });
        
        verticesGlobal = new List<Vertice>() { v0, v1, v2, v3 };
        
        Triangle t0 = new Triangle(new List<Vertice>() { v0, v1, v2 });
        Triangle t1 = new Triangle(new List<Vertice>() { v0, v1, v3 });
        Triangle t2 = new Triangle(new List<Vertice>() { v1, v2, v3 });
        Triangle t3 = new Triangle(new List<Vertice>() { v0, v2, v3 });

        t0.AddNeighbours(new List<Triangle>() { t1, t2, t3 });
        t1.AddNeighbours(new List<Triangle>() { t0, t2, t3 });
        t2.AddNeighbours(new List<Triangle>() { t0, t1, t3 });
        t3.AddNeighbours(new List<Triangle>() { t0, t1, t2 });

        t0.Center = TriCenter(t0);
        t1.Center = TriCenter(t1);
        t2.Center = TriCenter(t2);
        t3.Center = TriCenter(t3);

        centers.Add(t0.Center);
        centers.Add(t1.Center);
        centers.Add(t2.Center);
        centers.Add(t3.Center);
        
        tri.Add(t0);
        tri.Add(t1);
        tri.Add(t2);
        tri.Add(t3);
    }
    
    public void CreateShape( )
    {
        centers.Clear();
        Debug.Log(tri.Count);
        foreach (Triangle bs in tri)
        {                
            foreach (Vertice v in bs.Vertices)
            {
                foreach (Vertice V in bs.Vertices)
                {
                    if (v.Position != V.Position)
                    {
                        v.AddNeighbour(V);
                    }
                }                            
            }        
            bs.Center = TriCenter(bs);
            centers.Add(bs.Center);
           
        }
        Debug.Log(verticesGlobal.Count);
    }

    private void Update()
    {

        if (Input.GetKeyDown("r"))
        {
            Rectangle();
        }
        if (Input.GetKeyDown("t"))
        {
            Triangle();
        }
        if (Input.GetKeyDown("s"))
        {
            foreach (GameObject obj in _objects)
            {
                DestroyImmediate(obj);
            }
            _objects.Clear();
            ShowShape(verticesGlobal);
        }
        if (Input.GetKeyDown("c"))
        {
            ShowCenters();
        }
        if (Input.GetKeyDown("p"))
        {
            PertubeAndShow();
            CreateShape(); 
        }
    }

    public Vertice TriCenter(Triangle t)
    {
        Vertice v1 = t.Vertices[0];
        Vertice v2 = t.Vertices[1];
        Vertice v3 = t.Vertices[2];
        return new Vertice((v1.Position + v2.Position + v3.Position) / 3, new List<Vertice>() { v1, v2, v3 });
    }

    public List<Vertice> Perturbator(List<Vertice> vertices)
    {
        List<Vertice> Result = new List<Vertice>(vertices);
        float alpha = 0;
       
        for (int i = 0; i < vertices.Count; i++)
        {
            alpha = (1f / 9f) * (4f - 2f * Mathf.Cos((2f * Mathf.PI) / vertices[i].Neighbours.Count));
            
            Vector3 sum = new Vector3(0, 0, 0);
            for (int j = 0; j < vertices[i].Neighbours.Count; j++)
            {
                sum += vertices[i].Neighbours[j].Position;
            }
            Result[i] = new Vertice(vertices[i].Position * (1f - alpha) + (alpha / vertices[i].Neighbours.Count) * sum);
        }
        return Result;
    }

    public void ShowShape(List<Vertice> vertices)
    {

        foreach (Vertice v in vertices)
        {
            GameObject sphere = Instantiate(plot, v.Position, Quaternion.identity);
            _objects.Add(sphere);
            sphere.tag = "OrigineSphere";
        }
        
        foreach (Triangle triangle in tri)
        {

            DrawLine(triangle.Vertices[0].Position, triangle.Vertices[1].Position,Color.white,1000);
            DrawLine(triangle.Vertices[1].Position, triangle.Vertices[2].Position,Color.white,1000);
            DrawLine(triangle.Vertices[2].Position, triangle.Vertices[0].Position,Color.white,1000);
        }

    }

    public void ShowCenters()
    {
        foreach (Triangle triangle in tri)
        {
            DrawLine(triangle.Center.Position, triangle.Vertices[1].Position, Color.blue, 1000);
            DrawLine(triangle.Center.Position, triangle.Vertices[2].Position, Color.blue, 1000);
            DrawLine(triangle.Center.Position, triangle.Vertices[0].Position, Color.blue, 1000);

            GameObject sphere = Instantiate(plot1, triangle.Center.Position, Quaternion.identity);
            _objects.Add(sphere);
            sphere.tag = "NewSphere";
            /*foreach (Triangle TN in triangle.Neighbours)
            {
                DrawLine(triangle.Center.Position, TN.Center.Position, Color.blue, 1000);
            }*/
        }
    }

    public void PertubeAndShow()
    {
        List<Vertice> perturbed = new List<Vertice>();
        perturbed = Perturbator(verticesGlobal);
               
        foreach (Triangle T in tri)
        {
            for (int i = 0; i < T.Vertices.Count; i++)
            {                
                for (int j = 0; j < verticesGlobal.Count; j++)
                {
                    if (T.Vertices[i] == verticesGlobal[j])
                    {                    
                        T.Vertices[i] = perturbed[j];                      
                    }
                }
                T.Vertices[i].AddNeighbour(T.Center);
                T.Center.AddNeighbour(T.Vertices[i]);
            }
        }

        foreach (Vertice C in centers)
        {
            foreach (Vertice c in centers)
            {
                if (c != C)
                {
                    C.AddNeighbour(c);
                    c.AddNeighbour(C);
                }
            }
            foreach (Vertice vertex in verticesGlobal)
            {
                C.Neighbours.Remove(vertex);
            }
        }
        foreach (Vertice p in perturbed)
        {                 
            GameObject sphere = Instantiate(plot2, p.Position, Quaternion.identity);
            _objects.Add(sphere);
            sphere.tag = "NewSphere";
        }
        foreach (var line in GameObject.FindGameObjectsWithTag("Line"))
        {
            GameObject.Destroy(line);
        }
        foreach (var s in GameObject.FindGameObjectsWithTag("OrigineSphere"))
        {
            GameObject.Destroy(s);
        }
        
        verticesGlobal.Clear();
        verticesGlobal = perturbed;
    
        List<Triangle> newTri = new List<Triangle>(); 
        foreach (Triangle t in tri)
        {
            foreach (Triangle T in tri)
            {
                if (T != t)
                {
                    int count = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (T.Vertices[i] == t.Vertices[j])
                            {
                                count++;
                            }
                        }
                    }

                    if (count == 2)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (T.Vertices.Contains(t.Vertices[i]))
                            {
                                newTri.Add(new Triangle(new List<Vertice>(){t.Vertices[i],T.Center,t.Center}));

                            }
                            
                        }
                    }
                }
            }
            verticesGlobal.Add(t.Center);
        }
                   
        tri.Clear();
        tri = newTri;

        for (int i = 0; i < tri.Count; i++)
        {
            for (int j = 0; j < tri.Count; j++)
            {
                if (tri[i] != tri[j])
                {
                    bool isEqual = CompareLists(tri[i].Vertices, tri[j].Vertices);
                    if (isEqual)
                    {
                        tri.Remove(tri[j]);
                    }
                }
            }            
        }
        Debug.Log(tri.Count);
        
        foreach (Triangle triangle in tri)
        {
            DrawLine(triangle.Vertices[0].Position, triangle.Vertices[1].Position,Color.white,1000);
            DrawLine(triangle.Vertices[1].Position, triangle.Vertices[2].Position,Color.white,1000);
            DrawLine(triangle.Vertices[2].Position, triangle.Vertices[0].Position,Color.white,1000);
        }
        
        foreach (var s in GameObject.FindGameObjectsWithTag("NewSphere"))
        {
            s.tag = "OrigineSphere";
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        _objects.Add(myLine);
        myLine.tag = "Line";
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
    
    public static bool CompareLists<T>(List<T> aListA, List<T> aListB)
    {
        if (aListA == null || aListB == null || aListA.Count != aListB.Count)
            return false;
        if (aListA.Count == 0)
            return true;
        Dictionary<T, int> lookUp = new Dictionary<T, int>();
        // create index for the first list
        for(int i = 0; i < aListA.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListA[i], out count))
            {
                lookUp.Add(aListA[i], 1);
                continue;
            }
            lookUp[aListA[i]] = count + 1;
        }
        for (int i = 0; i < aListB.Count; i++)
        {
            int count = 0;
            if (!lookUp.TryGetValue(aListB[i], out count))
            {
                // early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
                return false;
            }
            count--;
            if (count <= 0)
                lookUp.Remove(aListB[i]);
            else
                lookUp[aListB[i]] = count;
        }
        // if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
        return lookUp.Count == 0;
    }
}



public class Vertice
{
    public Vector3 Position;
    public List<Vertice> Neighbours;

    public Vertice(Vector3 position, List<Vertice> neighbours)
    {
        Position = position;
        Neighbours = neighbours;
    }
    public Vertice(Vector3 position)
    {
        Position = position;
        Neighbours = new List<Vertice>();
    }

    public void AddNeighbour(Vertice v)
    {
        if (!Neighbours.Contains(v))
        {
            Neighbours.Add(v);
        }
    }

    public void AddNeighbours(List<Vertice> V)
    {
        foreach (Vertice v in V)
        {
            if (!Neighbours.Contains(v))
            {
                Neighbours.Add(v);
            }
        }

    }
}

public class Triangle
{
    public List<Vertice> Vertices;
    public List<Triangle> Neighbours;
    public Vertice Center;

    public Triangle(List<Vertice> vertices, List<Triangle> neighbours)
    {
        Vertices = vertices;
        Neighbours = neighbours;
    }

    public Triangle(List<Vertice> vertices)
    {
        Vertices = vertices;
        Neighbours = new List<Triangle>();
    }

    public void AddNeighbour(Triangle t)
    {
        if (!Neighbours.Contains(t))
        {
            Neighbours.Add(t);
        }
    }

    public void AddNeighbours(List<Triangle> T)
    {
        foreach (Triangle t in T)
        {
            if (!Neighbours.Contains(t))
            {
                Neighbours.Add(t);
            }
        }

    }

}

