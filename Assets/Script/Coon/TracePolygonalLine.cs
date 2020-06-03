using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracePolygonalLine : MonoBehaviour
{
    public GameObject plot;
    public GameObject plotred;
    public GameObject plotyellow;
    public GameObject plotgreen;
    public GameObject plotblue;


    private Vector3 mousePos;

    public float profondeur;

    public List<Vector3> Points;
    public List<Vector3> PointsClone;
    public List<Vector3> PointsChaikin;

    public List<Vector3> SurfacePoints;

    public LineRenderer line;
    public LineRenderer lineCurbe;
    public LineRenderer lineSurface;

    private bool Chaikin;
    public int nbiterations;// On va faire les test pour 4 courbes.

    //Pour Coon
    public int NbPointsCurbe;
    public List<Vector3> CurbeC1;
    public List<Vector3> CurbeC2;
    public List<Vector3> CurbeD1;
    public List<Vector3> CurbeD2;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            mousePos = GetMousePoint();
            //Debug.Log("x: " + mousePos.x + "  y  " + mousePos.y + " z : " + mousePos.z);
            Instantiate(plot, mousePos,Quaternion.identity);
            Points.Add(mousePos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3[] PointsVect = Points.ToArray();
            line.SetVertexCount(Points.Count);
            line.SetPositions(PointsVect);
            PointsClone = Points;
            NbPointsCurbe = Points.Count;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            
            if (!Chaikin)
            {
                Chaikin = true;
                for(int i = 0;i< nbiterations; i++)
                {
                    NbPointsCurbe *= 2;
                    Debug.Log("NB : " + NbPointsCurbe);
                    ChaikinCurbe();
                }
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CoonSurface();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            profondeur += 1;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            profondeur -= 1;
        }
    }

    Vector3 GetMousePoint()
    {
        
        var ray = this.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * profondeur;// pouvoir modifier la profondeur serait bien pour la 3D.
    }

    void ChaikinCurbe()
    {
        Vector3 nextPoint;
        Vector3 LineVector;
        Vector3 Normalise;
        Vector3 NewPoints1;
        Vector3 NewPoints2;
        float distance;
        float distance1;
        float distance2;
        
        //Pour chaque edge on placer les points u et v ( a 1/3 et 3/4 ) 
        for (int i = 0;i < PointsClone.Count; i++)
        {
            if (i + 1 == PointsClone.Count)
            {
                nextPoint = PointsClone[0];
            }
            else{
                nextPoint = PointsClone[i+1];
            }
            distance = Vector3.Distance(PointsClone[i], nextPoint);
            distance1 = distance / 4;
            distance2 = distance1 * 3;
            LineVector = nextPoint - PointsClone[i];
            Normalise = LineVector.normalized;
            NewPoints1 = PointsClone[i] + (distance1 * Normalise);
            Debug.Log("x: " + NewPoints1.x + "  y  " + NewPoints1.y + " z : " + NewPoints1.z);
            NewPoints2 = PointsClone[i] + (distance2 * Normalise);
            //Instantiate(plot, NewPoints1, Quaternion.identity);
            //Instantiate(plot, NewPoints2, Quaternion.identity);
            PointsChaikin.Add(NewPoints1);
            PointsChaikin.Add(NewPoints2);
        }
        //PointsClone = PointsChaikin;
        Vector3[] PointsVect = PointsChaikin.ToArray();
        lineCurbe.SetVertexCount(PointsChaikin.Count);
        lineCurbe.SetPositions(PointsVect);
        PointsClone.Clear();
        PointsClone = new List<Vector3>(PointsChaikin);
        PointsChaikin.Clear();
        //PointsChaikin.Clear();
    }



    void CoonSurface()
    {
        int SizeCurbe = NbPointsCurbe / 4;
        Debug.Log("Size : " + SizeCurbe);
        //Création des quatres courbes ( C1 de 0 à nb-1, D1 de nb à nb+nb-1, C2 de 2nb à 2nb+nb-1 et D2 de 3nb à PointsChaikin.Count-1)
        for (int i = 0;i< SizeCurbe; i++)
        {
            CurbeC1.Add(PointsClone[i]);
        }
        for (int i = SizeCurbe; i < 2* SizeCurbe; i++)
        {
            CurbeD1.Add(PointsClone[i]);
        }
        for (int i = 2* SizeCurbe; i < 3* SizeCurbe; i++)
        {
            CurbeC2.Add(PointsClone[i]);
        }
        for (int i = 3*SizeCurbe; i < NbPointsCurbe; i++)
        {
            CurbeD2.Add(PointsClone[i]);
        }
        for (int i = 0; i < CurbeC1.Count; i++)
        {
            Instantiate(plotred, CurbeC1[i], Quaternion.identity);
            Instantiate(plotyellow, CurbeC2[i], Quaternion.identity);
            Instantiate(plotblue, CurbeD1[i], Quaternion.identity);
            Instantiate(plotgreen, CurbeD2[i], Quaternion.identity);
        }
        //Calcule de la surface de Coon
        Vector3 rc;
        Vector3 rd;
        Vector3 rcd;
        Vector3 Surfacep;
        for(int v = 0; v < SizeCurbe; v++)
        {
            for(int u = 0; u < SizeCurbe; u++)
            {
                //Surfaces réglés interpolantes
                rc = (1 - v * 1.0f/(float)SizeCurbe) * CurbeC1[u] + (v * 1.0f/ (float)SizeCurbe) * CurbeC2[SizeCurbe -1 - u];
                rd = (1 - u * 1.0f / (float)SizeCurbe) * CurbeD1[v] + (u * 1.0f / (float)SizeCurbe) * CurbeD2[SizeCurbe - 1 - v];
                rcd = (1 - v* 1.0f / (float)SizeCurbe) * ((1 - u * 1.0f / (float)SizeCurbe) * CurbeD2[SizeCurbe - 1] + (u * 1.0f / (float)SizeCurbe) * CurbeD1[0]) + (v * 1.0f / (float)SizeCurbe) * ((1 - u * 1.0f / (float)SizeCurbe) * CurbeD2[0] + (u * 1.0f / (float)SizeCurbe) * CurbeD1[SizeCurbe - 1]);
                Surfacep = rc + rd - rcd;
                //Instantiate(plot, Surfacep, Quaternion.identity);
                SurfacePoints.Add(Surfacep);
            }
        }
        Vector3[] PointsVect = SurfacePoints.ToArray();
        lineSurface.SetVertexCount(SurfacePoints.Count);
        lineSurface.SetPositions(PointsVect);
    }
}
