using UnityEngine;

public struct SuspensionForce
{
    public Vector2 force;
    public float distance;

    public SuspensionForce(Vector2 zero, float suspensionRestDist) : this()
    {
        force = zero;
        distance = suspensionRestDist;
    }
}

public class CarSuspension2D : MonoBehaviour
{
    public Rigidbody2D carBody;
    public Transform tireTransform;
    public float suspensionRestDist = 0.2f;  // Resting distance of suspension
    public float wheelRadius = 0.5f;
    public float springStrength = 2.0f;    // How stiff the spring is
    public float springDamper = 1.5f;       // How much damping 
    public LayerMask groundLayer;
    public LineRenderer lineRenderer;
    public Transform suspensionTopTransform;

    private float forceMag;
    private float currentDist;  // current suspension distance

    public Vector2[] rayOffsets;

    private Vector2 startPos;

    private void Awake()
    {
        startPos = transform.position;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private SuspensionForce CalculateSuspensionForce(Vector2 rayOffset)
    {
        SuspensionForce suspensionForce = new SuspensionForce(Vector2.zero, suspensionRestDist);
        Vector2 springDir = tireTransform.up;

        // Start and direction of the suspension ray
        Vector2 rayOrigin = tireTransform.position;
        rayOrigin += rayOffset;
        Vector2 rayDir = -springDir; // usually downwards from the tire

        // raycast down to detect the ground
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDir, wheelRadius, groundLayer);

        if (hit)
        {
            Debug.DrawLine(rayOrigin, hit.point, Color.green, 0.1f);
            //Debug.DrawLine(tireTransform.position, tireTransform.position - tireTransform.up * forceMag, Color.green, 0.1f);
            
            // How much the suspension is compressed
            float offset = Mathf.Abs(wheelRadius - suspensionRestDist - hit.distance);

            // Get the tires world velocity (transform of the suspension to find the defualt point)
            Vector2 tireWorldVel = carBody.GetPointVelocity(rayOrigin); //carBody.GetPointVelocity is used for each corner as the center of mass will be different for each suspension. not carbody.velocity as that is for center of mess for the body of the car.

            // Velocity along the spring direction 
            float vel = Vector2.Dot(springDir, tireWorldVel);

            // Calculate spring force with damping (calucation of suspension working together as a whole)
            forceMag = (offset * springStrength) - (vel * springDamper);

            // Apply force upwards at tire position
            //carBody.AddForceAtPosition(springDir * forceMag, tireTransform.position);

            Debug.DrawLine(rayOrigin, new Vector3(rayOrigin.x, rayOrigin.y) + tireTransform.up * forceMag, Color.yellow, 0.1f);

            suspensionForce.distance = offset;
            suspensionForce.force = springDir * forceMag;

            return suspensionForce;

        }

        // Not grounded, wheel fully extended
        currentDist = suspensionRestDist;
        forceMag = 0;
        Debug.DrawRay(rayOrigin, rayDir, Color.red, 0.1f);
        //Debug.DrawLine(tireTransform.position, (tireTransform.position - tireTransform.up) * suspensionRestDist, Color.red, 0.1f);
        return suspensionForce;
        
    }

    private void FixedUpdate()
    {
        // Direction of suspension -local up of the tire
        Vector2 springDir = tireTransform.up;

        Vector2 accumulatedForce = Vector2.zero;
        float shortestDistance = suspensionRestDist;

        foreach (var offset in rayOffsets)// for each raycast at Calculate the suspension forces 
        {
            var suspensionForce = CalculateSuspensionForce(offset);
            if(suspensionForce.distance < shortestDistance)
                shortestDistance = suspensionForce.distance;

            accumulatedForce += suspensionForce.force;
        }
        //making the length of the raycast for position
        accumulatedForce /= rayOffsets.Length;
        carBody.AddForceAtPosition(accumulatedForce, tireTransform.position);

        Debug.Log("(" + gameObject.name + ") " + shortestDistance.ToString());
        Debug.Log("(" + gameObject.name + ") " + transform.localPosition.y + " - " + (transform.localPosition.y - shortestDistance));

        if (shortestDistance < suspensionRestDist)
        {
            //paramaters for the suspension spring
            transform.localPosition = new Vector3(transform.localPosition.x,
                                                  transform.localPosition.y - shortestDistance,
                                                  transform.localPosition.z);
        }
        // Update tire visual position
        //tireTransform.position = rayOrigin - springDir * currentDist;
        lineRenderer.SetPosition(0, suspensionTopTransform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

        void OnDrawGizmos()
        {
            // Draw ray in editor
            //if (tireTransform != null)
            //{
            //    Gizmos.color = Color.yellow;
            //    Gizmos.DrawLine(tireTransform.position, tireTransform.position - tireTransform.up * forceMag);
            //}
        }
}