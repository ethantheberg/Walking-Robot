using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
  [SerializeField] Transform[] waypoints;
  [SerializeField] float speed = 3.0f;
  int progress = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if((waypoints[progress].position - transform.position).magnitude < 0.5) ++progress;
      //Vector3 direction = (waypoints[progress].position - transform.position).normalized;
      //transform.up = direction;
      //transform.position += direction*speed*Time.deltaTime;
      transform.position += Vector3.forward*speed*Time.deltaTime;
        
    }
}
