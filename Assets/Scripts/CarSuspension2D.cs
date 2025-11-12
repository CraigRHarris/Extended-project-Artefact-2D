using UnityEngine;

public class CarSuspension2D : MonoBehaviour
{
        public Rigidbody2D carBody;
        public Transform tireTransform;
        public float suspensionRestDist = 0.5f;  // Resting distance of suspension
        public float springStrength = 2.0f;    // How stiff the spring is
        public float springDamper = 1.5f;       // How much damping 
        public LayerMask groundLayer;

        private float forceMag;
        private float currentDist;  // current suspension distance

    public Vector2[] rayOffsets;

    private Vector2 CalculateSuspensionForce(Vector2 rayOffset)
    {
        Vector2 springDir = tireTransform.up;

        // Start and direction of the suspension ray
        Vector2 rayOrigin = tireTransform.position;
        rayOrigin += rayOffset;
        Vector2 rayDir = -springDir; // usually downwards from the tire

        // raycast down to detect the ground
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDir, suspensionRestDist, groundLayer);

        if (hit)
        {
            Debug.DrawLine(rayOrigin, hit.point, Color.green, 0.1f);
            //Debug.DrawLine(tireTransform.position, tireTransform.position - tireTransform.up * forceMag, Color.green, 0.1f);
            // How much the suspension is compressed
            float offset = suspensionRestDist - hit.distance;

            // Get the tires world velocity
            Vector2 tireWorldVel = carBody.GetPointVelocity(rayOrigin);

            // Velocity along the spring direction - project onto spring direction
            float vel = Vector2.Dot(springDir, tireWorldVel);

            // Calculate spring force with damping
            forceMag = (offset * springStrength) - (vel * springDamper);

            // Apply force upwards at tire position
            //carBody.AddForceAtPosition(springDir * forceMag, tireTransform.position);

            Debug.DrawLine(rayOrigin, new Vector3(rayOrigin.x, rayOrigin.y) + tireTransform.up * forceMag, Color.yellow, 0.1f);

            return springDir * forceMag;
        }
        else
        {
            // Not grounded, wheel fully extended
            currentDist = suspensionRestDist;
            forceMag = 0;
            Debug.DrawRay(rayOrigin, rayDir, Color.red, 0.1f);
            //Debug.DrawLine(tireTransform.position, (tireTransform.position - tireTransform.up) * suspensionRestDist, Color.red, 0.1f);
            return Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        // Direction of suspension -local up of the tire
        Vector2 springDir = tireTransform.up;

        Vector2 accumulatedForce = Vector2.zero;

        foreach (var offset in rayOffsets)
        {
            accumulatedForce += CalculateSuspensionForce(offset);
        }

        accumulatedForce /= rayOffsets.Length;
        carBody.AddForceAtPosition(accumulatedForce, tireTransform.position);

        // Update tire visual position
        //tireTransform.position = rayOrigin - springDir * currentDist;

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