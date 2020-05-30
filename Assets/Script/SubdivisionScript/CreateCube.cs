using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCube : MonoBehaviour
{


    public GameObject spherePref;
    public GameObject quadPref;


    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> PointsList = new List<Vector3>();
        PointsList.Add(new Vector3(-10, 10, 20));
        PointsList.Add(new Vector3(0, 10, 20));
        PointsList.Add(new Vector3(0, 0, 20));
        PointsList.Add(new Vector3(-10, 0, 20));
        PointsList.Add(new Vector3(-10, 10, 10));
        PointsList.Add(new Vector3(0, 10, 10));
        PointsList.Add(new Vector3(0, 0, 10));
        PointsList.Add(new Vector3(-10, 0, 10));



        //
        

        foreach (var p in PointsList)
        {
            GameObject sphere = Instantiate(spherePref, new Vector3(p.x, p.y, p.z), Quaternion.identity);
        }

        //mesh
        GameObject test = Instantiate(quadPref);

        Mesh mesh = test.GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        Vector3[] vertices = new Vector3[] {
            new Vector3(-10, 10, 10),
            new Vector3(0, 10, 10),
            new Vector3(-10, 0, 10),
            new Vector3(0, 0, 10),
            new Vector3(-10, 10, 20),
            new Vector3(0, 10, 20),
            new Vector3(-10, 0, 20),
            new Vector3(0, 0, 20)
        };


        int[] triangles = new int[]
        {
            0,1,2,
            2,1,3,
            4,0,6,
            6,0,2,
            5,4,6,
            5,6,7,
            1,5,3,
            3,5,7,
            4,5,0,
            0,5,1,
            2,3,6,
            6,3,7
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
