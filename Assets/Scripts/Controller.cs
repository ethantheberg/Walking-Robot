using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
  [SerializeField] LayerMask terrainLayer = default;
  [SerializeField] Transform[] waypoints;
  [SerializeField] float speed = 3.0f;
  [SerializeField] float height = default;
  int progress = 0;
  Vector3 hitpoint;
  Vector3 goalNormal;
  float normalVelocity = 0.0f;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    Ray ray = new Ray(transform.position, Vector3.down);
    bool hit = Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value);
    if(hit){
      hitpoint = info.point;

      transform.position = info.point + info.normal*height;
      goalNormal = info.normal;
    }
    transform.forward = Mathf.SmoothDamp(transform.forward, goalNormal, ref normalVelocity, 0.5f);
    //transform.forward = info.normal;

    //if ((waypoints[progress].position - transform.position).magnitude < 0.5) ++progress;
    //Vector3 direction = (waypoints[progress].position - transform.position).normalized;
    //transform.up = direction;
    //transform.position += direction*speed*Time.deltaTime;
    transform.position += Vector3.forward * speed * Time.deltaTime;

  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.blue;
    Gizmos.DrawSphere(hitpoint, 0.1f);
  }
}
