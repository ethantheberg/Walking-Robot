using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    [SerializeField] IKFootSolver otherFoot = default;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    [SerializeField] Vector3 footOffset = default;
    Vector3 shoulderOffset;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp;

    private void Start()
    {
        shoulderOffset = new Vector3(transform.position.x - body.position.x, 0, transform.position.z - body.position.z);

        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    // Update is called once per frame

    void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;
        
        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            //tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            transform.position = tempPosition;
            transform.up = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
            return;
        }

        Ray ray = new Ray(body.position + shoulderOffset, Vector3.down);
        bool hit = Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value);
        if (hit && Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving()){
            oldPosition = transform.position;
            oldNormal = transform.up;
            lerp = 0;}{
            int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(transform.position).z ? 1 : -1;
            newPosition = info.point + (Vector3.forward * stepLength * direction) + footOffset;
            newNormal = info.normal;
        } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.1f);
        Gizmos.DrawRay(body.position+shoulderOffset, Vector3.down);
    }



    public bool IsMoving()
    {
        return false;//return lerp < 1;
    }
}
