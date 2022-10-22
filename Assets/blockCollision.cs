using UnityEngine;

public class blockCollision : MonoBehaviour
{
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "fall")
        {
            transform.parent = col.transform;
            Vector3 positionobj = transform.position;
            positionobj.x = col.transform.position.x;
            positionobj.z = col.transform.position.z;
            transform.position = positionobj;
        }
    }
}
