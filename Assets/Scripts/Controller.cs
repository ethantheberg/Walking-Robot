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

    void Start()
    {
        Ray ray = new Ray(transform.position, -transform.forward);
        Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value);
        currentNormal = info.normal;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value)) return;

        UpdateNormal(info);

        if (Vector2.Distance(To2d(waypoints[progress].position), To2d(transform.position)) < 0.5)
        {
            ++progress;
            progress %= waypoints.Length;
        }
        direction = To2d(waypoints[progress].position - transform.position).normalized;

        targetAngle = -Vector2.SignedAngle(Vector2.left, direction);
        currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, 0.5f);

        transform.Rotate(0, 0, currentAngle);

        float error = Mathf.Abs(currentAngle - targetAngle);
        if (error > 180.0f)
        {
            error -= 360.0f;
        }
        Debug.Log(error);
        transform.position += speed * Time.deltaTime * To3d(direction);// * Damp(error);

        //transform.position += speed * Time.deltaTime * Vector3.forward;
    }

    private float Damp(float value)
    {
        return 1 / (1 + Mathf.Exp(5 * (value - 1)));
    }

    private void UpdateNormal(RaycastHit info)
    {
        currentNormal.x = Mathf.SmoothDamp(currentNormal.x, info.normal.x, ref normalVelocity.x, 0.5f);
        currentNormal.y = Mathf.SmoothDamp(currentNormal.y, info.normal.y, ref normalVelocity.y, 0.5f);
        currentNormal.z = Mathf.SmoothDamp(currentNormal.z, info.normal.z, ref normalVelocity.z, 0.5f);
        transform.forward = currentNormal;
        transform.position = info.point + transform.forward * height;

        transform.Rotate(0, 0, -Vector2.SignedAngle(To2d(transform.up).normalized, Vector2.left));
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
    }
}
