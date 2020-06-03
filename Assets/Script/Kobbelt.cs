using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kobbelt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject plot;
    public GameObject plot1;
    public GameObject plot2;
    public List<Triangle> tri=new List<Triangle>();
    
    void Start()
    {
       Vertice v0 = new Vertice(new Vector3(0,0,0));
       Vertice v1 = new Vertice(new Vector3(8f,0,0));
       Vertice v2 = new Vertice(new Vector3(4f,0,6f));
       Vertice v3 = new Vertice(new Vector3(4f,6f,3f));
       
       v0.AddNeighbours(new List<Vertice>(){v1,v2,v3});
       v1.AddNeighbours(new List<Vertice>(){v0,v2,v3});
       v2.AddNeighbours(new List<Vertice>(){v0,v1,v3});
       v3.AddNeighbours(new List<Vertice>(){v0,v1,v2});     
                
       Triangle t0 = new Triangle(new List<Vertice>() {v0, v1, v2});
       Triangle t1 = new Triangle(new List<Vertice>() {v0, v1, v3});
       Triangle t2 = new Triangle(new List<Vertice>() {v1, v2, v3});
       Triangle t3 = new Triangle(new List<Vertice>() {v0, v2, v3});

       t0.AddNeighbours(new List<Triangle>(){t1,t2,t3});
       t1.AddNeighbours(new List<Triangle>(){t0,t2,t3});
       t2.AddNeighbours(new List<Triangle>(){t0,t1,t3});
       t3.AddNeighbours(new List<Triangle>(){t0,t1,t2});
       
       t0.Center = TriCenter(t0);
       t1.Center = TriCenter(t1);
       t2.Center = TriCenter(t2);
       t3.Center = TriCenter(t3);
       
       List<Vertice> vertices = new List<Vertice>(){v0,v1,v2,v3};
       List<Vertice> basic = new List<Vertice>(){v0,v1,v2,v3};
       vertices = Perturbator(vertices); 
       
       Triangle T0 = new Triangle(new List<Vertice>() {vertices[0], vertices[1], vertices[2]});
       Triangle T1 = new Triangle(new List<Vertice>() {vertices[0], vertices[1], vertices[3]});
       Triangle T2 = new Triangle(new List<Vertice>() {vertices[1], vertices[2], vertices[3]});
       Triangle T3 = new Triangle(new List<Vertice>() {vertices[0], vertices[2], vertices[3]});
 
       T0.AddNeighbours(new List<Triangle>(){T1,T2,T3});
       T1.AddNeighbours(new List<Triangle>(){T0,T2,T3});
       T2.AddNeighbours(new List<Triangle>(){T0,T1,T3});
       T3.AddNeighbours(new List<Triangle>(){T0,T1,T2});
       
       T0.Center = t0.Center;
       T1.Center = t1.Center;
       T2.Center = t2.Center;
       T3.Center = t3.Center;
       
       tri.Add(T0);
       tri.Add(T1);
       tri.Add(T2);
       tri.Add(T3);
       
       ShowShape(vertices,basic);
    }

    public Vertice TriCenter(Triangle t)
    {
        Vertice v1 = t.Vertices[0];
        Vertice v2 = t.Vertices[1];
        Vertice v3 = t.Vertices[2];
        return new Vertice((v1.Position+v2.Position+v3.Position)/3,new List<Vertice>(){v1,v2,v3});      
    }

    public List<Vertice> Perturbator(List<Vertice> vertices)
    {
        List<Vertice> Result = vertices;
        
        float alpha = 0;
        Vector3 V;
        for (int i = 0; i < vertices.Count; i++)
        {
            alpha = (1f / 9f) * (4f - 2f * Mathf.Cos((2f*Mathf.PI) / vertices[i].Neighbours.Count));

            Vector3 sum = new Vector3(0,0,0);
            for (int j = 0; j < vertices[i].Neighbours.Count; j++)
            {
                sum += vertices[i].Neighbours[j].Position;
            }
            Result[i] = new Vertice(vertices[i].Position * (1f - alpha) + (alpha / vertices[i].Neighbours.Count) * sum);
        }

        return Result;
    }

    public void ShowShape(List<Vertice> perturbed,List<Vertice> basic)
    {
        
        foreach (Vertice v in basic)
        {
            Instantiate(plot, v.Position, Quaternion.identity);
        }
        
        foreach (Triangle triangle in tri)
        {          
            
            //DrawLine(triangle.Vertices[0].Position, triangle.Vertices[1].Position,Color.white,1000);
            //DrawLine(triangle.Vertices[1].Position, triangle.Vertices[2].Position,Color.white,1000);
            //DrawLine(triangle.Vertices[2].Position, triangle.Vertices[0].Position,Color.white,1000);
            
            DrawLine(triangle.Center.Position, triangle.Vertices[1].Position,Color.blue,1000);
            DrawLine(triangle.Center.Position, triangle.Vertices[2].Position,Color.blue,1000);
            DrawLine(triangle.Center.Position, triangle.Vertices[0].Position,Color.blue,1000);     

            Instantiate(plot1, triangle.Center.Position, Quaternion.identity);
            
            foreach (Triangle TN in triangle.Neighbours)
            {
                DrawLine(triangle.Center.Position, TN.Center.Position,Color.blue,1000);
            }
            
        }

        foreach (Vertice p in perturbed)
        {
            Debug.Log(p.Position);
            Instantiate(plot2, p.Position, Quaternion.identity);
        }
    }
    
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
        GameObject.Destroy(myLine, duration);
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
