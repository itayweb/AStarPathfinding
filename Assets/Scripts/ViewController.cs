using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    private Camera _camera;
    
    void Start()
    {
        _camera = GetComponent<Camera>();
    }
    
    void Update()
    {
        
    }

    public void ZoomOut()
    {
        _camera.orthographicSize++;
    }

    public void ZoomIn()
    {
        _camera.orthographicSize--;
    }
}
