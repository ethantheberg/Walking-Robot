using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    Vector3 initialPosition;
    [SerializeField] private bool invert = false;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        initialPosition = lineRenderer.GetPosition(1);
    }

    public void enable(){
        lineRenderer.enabled = true;
    }

    public void disable(){
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    public void Scan()
    {
        lineRenderer.SetPosition(1, new Vector3(initialPosition.x, initialPosition.y + Mathf.Sin(Time.time * 5) * 0.5f * (invert ? -1.0f : 1.0f), initialPosition.z));
    }
}
