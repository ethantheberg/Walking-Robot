using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float speed = 3.0f;
    [SerializeField] float height = default;
    int progress = 0;
    Vector3 hitpoint;
    Vector3 hitnormal;
    Vector3 currentNormal;
    Vector3 normalVelocity;
    float targetAngle;
    float currentAngle;
    float angleVelocity;
    Vector2 direction;

    void Start() {
        Ray ray = new Ray(transform.position, -transform.forward);
        bool hit = Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value);
        currentNormal = info.normal;
    }
    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.forward);
        bool hit = Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value);
        if (!hit) return;

        hitpoint = info.point;
        hitnormal = info.normal;

        currentNormal.x = Mathf.SmoothDamp(currentNormal.x, info.normal.x, ref normalVelocity.x, 0.5f);
        currentNormal.y = Mathf.SmoothDamp(currentNormal.y, info.normal.y, ref normalVelocity.y, 0.5f);
        currentNormal.z = Mathf.SmoothDamp(currentNormal.z, info.normal.z, ref normalVelocity.z, 0.5f);
        transform.forward = currentNormal;
        transform.position = info.point + currentNormal * height;
        
        if ((To2d(waypoints[progress].position) - To2d(transform.position)).magnitude < 0.5) 
        {
            ++progress;
            progress %= waypoints.Length;
        }
        direction = To2d((waypoints[progress].position - transform.position).normalized);

        targetAngle = -Vector2.SignedAngle(To2d(transform.up), direction);
        currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, 0.5f);

        transform.Rotate(0, 0, targetAngle);

        transform.position += speed * Time.deltaTime * To3d(direction);
        //transform.position += speed * Time.deltaTime * Vector3.forward;
    }

    Vector2 To2d(Vector3 input)
    {
        return new Vector2(input.x, input.z);
    }

    Vector3 To3d(Vector2 input)
    {
        return new Vector3(input.x, 0, input.y);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(hitpoint, 0.1f);
        Gizmos.DrawRay(transform.position + Vector3.up * 2, direction);
        Gizmos.DrawRay(transform.position + Vector3.up * 2, To2d(transform.up));

        Gizmos.DrawRay(hitpoint, hitnormal);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(hitpoint, currentNormal);
    }
}
