using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PyramidMesh : MonoBehaviour
{
    public float width = 1f;
    public float height = 1f;

    private Rigidbody rb;
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[5];
        vertices[0] = new Vector3(0, 0, 0);                            // Base center
        vertices[1] = new Vector3(-width / 2, 0, width / 2);           // Base corner 1
        vertices[2] = new Vector3(width / 2, 0, width / 2);            // Base corner 2
        vertices[3] = new Vector3(width / 2, 0, -width / 2);           // Base corner 3
        vertices[4] = new Vector3(-width / 2, 0, -width / 2);          // Base corner 4

        vertices[0] = new Vector3(0, height, 0);                       // Apex

        int[] triangles = new int[]
        {
            // Base
            1, 0, 2,
            2, 0, 3,
            3, 0, 4,
            4, 0, 1,
            // Sides
            1, 2, 4,
            2, 3, 4
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();


        rb = GetComponent<Rigidbody>();
        Vector3 forceDirection = new Vector3(0.5f, 0, 0.5f);
        rb.AddForce(forceDirection.normalized * 5);
        
    }
    public float constantSpeed = 2f;

    void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // If the velocity is zero, set it to a random X-Z vector
        if (rb.velocity.magnitude < 0.01f)
        {
            rb.velocity = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * constantSpeed;
        }

        // Normalize the velocity and then scale it to the constant speed
        Vector3 newVelocity = rb.velocity.normalized * constantSpeed;
        newVelocity.y = 0; // Set Y velocity to 0
        rb.velocity = newVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Reflect the cube's velocity when it collides with a wall
        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 reflectedVelocity = Vector3.Reflect(GetComponent<Rigidbody>().velocity.normalized, contact.normal);
            GetComponent<Rigidbody>().velocity = reflectedVelocity;
        }
    }
}