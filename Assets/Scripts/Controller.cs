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
    Vector3 currentNormal;
    Vector3 normalVelocity;
    float targetAngle;
    float currentAngle;
    float angleVelocity;
    Vector2 direction;
    bool scanning = false;
    Vector2 scanDirection;
    [SerializeField] LaserController laserController1;
    [SerializeField] LaserController laserController2;

    void Start()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value);
        currentNormal = info.normal;
        StopScan();
    }

    public void StartScan(Vector2 direction)
    {
        scanning = true;
        scanDirection = direction;
        laserController1.enable();
        laserController2.enable();
    }

    public void StopScan()
    {
        scanning = false;
        laserController1.disable();
        laserController2.disable();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        if (!Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value)) return;

        UpdateNormal(info);

        if (scanning)
        {
            laserController1.Scan();
            laserController2.Scan();
        }

        if (Vector2.Distance(To2d(waypoints[progress].position), To2d(transform.position)) < 0.5)
        {
            ++progress;
            progress %= waypoints.Length;
            StartScan(Vector2.left);
            return;
        }
        
        Move();
    }

    private void Move()
    {
        if (scanning)
        {
            direction = To2d(scanDirection);
        }
        else
        {
            direction = To2d(waypoints[progress].position - transform.position).normalized;
        }

        targetAngle = -Vector2.SignedAngle(Vector2.left, direction);
        currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, 0.5f);

        transform.Rotate(0, currentAngle, 0);
        if (!scanning) transform.position += speed * Time.deltaTime * To3d(direction);

        //transform.position += speed * Time.deltaTime * Vector3.forward;
    }

    private void UpdateNormal(RaycastHit info)
    {
        currentNormal.x = Mathf.SmoothDamp(currentNormal.x, info.normal.x, ref normalVelocity.x, 0.5f);
        currentNormal.y = Mathf.SmoothDamp(currentNormal.y, info.normal.y, ref normalVelocity.y, 0.5f);
        currentNormal.z = Mathf.SmoothDamp(currentNormal.z, info.normal.z, ref normalVelocity.z, 0.5f);
        transform.up = currentNormal;
        transform.position = info.point + transform.up * height;

        transform.Rotate(0, Vector2.SignedAngle(To2d(transform.forward).normalized, Vector2.left), 0);
    }

    Vector2 To2d(Vector3 input)
    {
        return new Vector2(input.x, input.z);
    }

    Vector3 To3d(Vector2 input)
    {
        return new Vector3(input.x, 0, input.y);
    }
}
