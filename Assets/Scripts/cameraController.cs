using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
  [SerializeField] Transform subject = default;
  Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - subject.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = subject.position + offset;
    }
}
