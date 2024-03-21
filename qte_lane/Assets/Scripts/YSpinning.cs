using UnityEngine;

public class YSpinning : MonoBehaviour
{
    public float rotationSpeed = 30f;

    public Vector3 rotationAxis = Vector3.forward;

    void Update()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;

        transform.Rotate(rotationAxis * rotationAmount);
    }
}