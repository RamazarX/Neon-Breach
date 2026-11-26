using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 50f;
    void Update()
    {
        Vector3 centerPos = new Vector3(0, 0, transform.position.z);
        transform.RotateAround(centerPos, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
