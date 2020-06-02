using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kobbelt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject plot;
    public GameObject plot1;
    public List<Triangle> tri=new List<Triangle>();
    void Start()
    {
       Vertice v0 = new Vertice(new Vector3(0,0,0));
       Vertice v1 = new Vertice(new Vector3(7,-1,0));
       Vertice v2 = new Vertice(new Vector3(4,3,0));
       Vertice v3 = new Vertice(new Vector3(3,9,0));
       Vertice v4 = new Vertice(new Vector3(-3,5,0));
       Vertice v5 = new Vertice(new Vector3(-7,1,0));
       
       v0.AddNeighbours(new List<Vertice>(){v1,v2,v4,v5});
       v1.AddNeighbours(new List<Vertice>(){v0,v2});
       v2.AddNeighbours(new List<Vertice>(){v0,v1,v3,v4});
       v3.AddNeighbours(new List<Vertice>(){v2,v4});     
       v4.AddNeighbours(new List<Vertice>(){v0,v2,v3,v5});
       v4.AddNeighbours(new List<Vertice>(){v0,v4});

       Triangle t0 = new Triangle(new List<Vertice>() {v0, v1, v2});
       Triangle t1 = new Triangle(new List<Vertice>() {v0, v2, v4});
       Triangle t2 = new Triangle(new List<Vertice>() {v0, v4, v5});
       Triangle t3 = new Triangle(new List<Vertice>() {v2, v3, v4});

       t0.AddNeighbour(t1);
       t1.AddNeighbours(new List<Triangle>(){t0,t2,t3});
       t2.AddNeighbour(t1);
       t3.AddNeighbour(t1);

       Debug.Log(t0.Vertices.Count);
       
       t0.Center = TriCenter(t0);
       t1.Center = TriCenter(t1);
       t2.Center = TriCenter(t2);
       t3.Center = TriCenter(t3);
       
       tri.Add(t0);
       tri.Add(t1);
       tri.Add(t2);
       tri.Add(t3);
       ShowShape();
    }

    public Vertice TriCenter(Triangle t)
    {
        Vertice v1 = t.Vertices[0];
        Vertice v2 = t.Vertices[1];
        Vertice v3 = t.Vertices[2];
        return new Vertice((v1.Position+v2.Position+v3.Position)/3,new List<Vertice>(){v1,v2,v3});      
    }

    public void ShowShape()
    {
        foreach (Triangle triangle in tri)
        {
            foreach (Vertice v in triangle.Vertices)
            {
                Instantiate(plot, v.Position, Quaternion.identity);
            }
            DrawLine(triangle.Vertices[0].Position, triangle.Vertices[1].Position,Color.white,1000);
            DrawLine(triangle.Vertices[1].Position, triangle.Vertices[2].Position,Color.white,1000);
            DrawLine(triangle.Vertices[2].Position, triangle.Vertices[0].Position,Color.white,1000);
            
            DrawLine(triangle.Center.Position, triangle.Vertices[1].Position,Color.blue,1000);
            DrawLine(triangle.Center.Position, triangle.Vertices[2].Position,Color.blue,1000);
            DrawLine(triangle.Center.Position, triangle.Vertices[0].Position,Color.blue,1000);

            bool a = true;
            foreach (Triangle triangle2 in tri)
            {
                if (triangle2 == triangle)
                {
                    continue;
                }
                
                if (triangle2.Vertices.Contains(triangle.Vertices[1]) && triangle2.Vertices.Contains(triangle.Vertices[2]))
                {
                    a = false;
                }
            }

            if (a)
            {
                Vector3 Lerped = Vector3.Lerp(triangle.Vertices[1].Position, triangle.Vertices[2].Position, 0.5f);
                DrawLine(triangle.Center.Position, Lerped+(Lerped-triangle.Center.Position)*2,Color.green,1000);
            }
            
            a = true;
            foreach (Triangle triangle2 in tri)
            {
                if (triangle2 == triangle)
                {
                    continue;
                }
                
                if (triangle2.Vertices.Contains(triangle.Vertices[0]) && triangle2.Vertices.Contains(triangle.Vertices[2]))
                {
                    a = false;
                }
            }

            if (a)
            {
                Vector3 Lerped2 = Vector3.Lerp(triangle.Vertices[0].Position, triangle.Vertices[2].Position, 0.5f);
                DrawLine(triangle.Center.Position, Lerped2+(Lerped2-triangle.Center.Position)*2,Color.green,1000);
            }
            
            a = true;
            foreach (Triangle triangle2 in tri)
            {
                if (triangle2 == triangle)
                {
                    continue;
                }
                
                if (triangle2.Vertices.Contains(triangle.Vertices[0]) && triangle2.Vertices.Contains(triangle.Vertices[1]))
                {
                    a = false;
                }
            }

            if (a)
            {
                Vector3 Lerped3 = Vector3.Lerp(triangle.Vertices[1].Position, triangle.Vertices[0].Position, 0.5f);
                DrawLine(triangle.Center.Position, Lerped3+(Lerped3-triangle.Center.Position)*2,Color.green,1000);            
            }

            Instantiate(plot1, triangle.Center.Position, Quaternion.identity);
            
            foreach (Triangle TN in triangle.Neighbours)
            {
                DrawLine(triangle.Center.Position, TN.Center.Position,Color.blue,1000);
            }
            
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
