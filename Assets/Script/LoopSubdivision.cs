using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Subdiv
{
    [RequireComponent (typeof(MeshFilter))]
    public class LoopSubdivision : MonoBehaviour
    {
        [SerializeField, Range(1, 4)] protected int details = 1;
        [SerializeField] bool weld = false;

        void Start()
        {
            var filter = GetComponent<MeshFilter>();
            var source = filter.mesh;
            var mesh = SubdivisionSurface.Subdivide(SubdivisionSurface.Weld(source, float.Epsilon, source.bounds.size.x), details, weld);
            filter.sharedMesh = mesh;
        }
    
    }

}    

