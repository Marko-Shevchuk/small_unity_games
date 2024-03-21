using UnityEngine;

public class PendulumSwing : MonoBehaviour
{
    public float swingSpeed = 1f;
    public Vector3 swingAxis = Vector3.up;
    public float swingAmplitude = 90f;

    void Update()
    {

        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAmplitude;

        Quaternion rotation = Quaternion.AngleAxis(angle, swingAxis);

        transform.rotation = rotation;
    }
}