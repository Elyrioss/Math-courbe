using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathPlotRaycaster : MonoBehaviour
{

    private Camera _camera;
    [SerializeField] private MathData _data;
    
    private void Start()
    {
        _camera = Camera.main;
    }
    
    
    private bool _mouseState;
    private GameObject target;
    public Vector3 screenSpace;
    public Vector3 offset;
 
    // Use this for initialization
    // Update is called once per frame
    void Update ()
    {
        if (!_camera.gameObject.activeSelf)
        {
            return;
        }
        // Debug.Log(_mouseState);
        if (Input.GetMouseButtonDown (0)) {
 
            RaycastHit hitInfo;
            target = GetClickedObject (out hitInfo);
            if (target != null) {
                if (target.transform.position.y == 0f)
                {
                    _data.AddData(target.gameObject);
                }
                else
                {
                    
                    target.GetComponent<Renderer>().material =
                        _data._brightColors[target.GetComponent<PlotData>().colorId];
                    _mouseState = true;
                    screenSpace = Camera.main.WorldToScreenPoint (target.transform.position);
                    offset = target.transform.position - _camera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
                }                
            }
        }
        if (Input.GetMouseButtonUp (0)) {
            _mouseState = false;
            if (target != null)
            {
                target.GetComponent<Renderer>().material =
                    _data._colors[target.GetComponent<PlotData>().colorId];
            }
            
        }
        if (_mouseState) {
            //keep track of the mouse position
            var curScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
 
            //convert the screen mouse position to world point and adjust with offset
            var curPosition = Camera.main.ScreenToWorldPoint (curScreenSpace) + offset;
 
            //update the position of the object in the world
            target.transform.position = curPosition;
        }
    }
   
   
    GameObject GetClickedObject (out RaycastHit hit)
    {
        GameObject target = gameObject;
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast (ray.origin, ray.direction * 10, out hit)) {
            if (hit.transform.transform.GetComponent<PlotData>())
            {             
                target = hit.collider.gameObject;
                
            }
            
        }
 
        return target;
    }
}
