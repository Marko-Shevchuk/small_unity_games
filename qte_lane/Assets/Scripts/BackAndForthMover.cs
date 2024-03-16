using UnityEngine;

public class BackAndForthMover : MonoBehaviour
{
    public float speed = 1f;
    public float distance = 4f; 
    public Axis axis = Axis.Z; 

    private float startPosition;
    private bool movingRight = true; 

    
    public enum Axis
    {
        X,
        Y,
        Z
    }

    void Start()
    {

        switch (axis)
        {
            case Axis.X:
                startPosition = transform.position.x;
                break;
            case Axis.Y:
                startPosition = transform.position.y;
                break;
            case Axis.Z:
                startPosition = transform.position.z;
                break;
        }
    }

    void Update()
    {

        float newPosition = 0f;
        switch (axis)
        {
            case Axis.X:
                newPosition = transform.position.x + (movingRight ? speed * Time.deltaTime : -speed * Time.deltaTime);
                break;
            case Axis.Y:
                newPosition = transform.position.y + (movingRight ? speed * Time.deltaTime : -speed * Time.deltaTime);
                break;
            case Axis.Z:
                newPosition = transform.position.z + (movingRight ? speed * Time.deltaTime : -speed * Time.deltaTime);
                break;
        }


        if (movingRight && newPosition >= startPosition + distance)
        {
            movingRight = false;
        }
        else if (!movingRight && newPosition <= startPosition - distance)
        {
            movingRight = true;
        }

   
        switch (axis)
        {
            case Axis.X:
                transform.position = new Vector3(newPosition, transform.position.y, transform.position.z);
                break;
            case Axis.Y:
                transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);
                break;
            case Axis.Z:
                transform.position = new Vector3(transform.position.x, transform.position.y, newPosition);
                break;
        }
    }
}