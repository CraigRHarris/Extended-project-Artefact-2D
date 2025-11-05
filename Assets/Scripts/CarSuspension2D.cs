using UnityEngine;

public class CarSuspension2D : MonoBehaviour
{
        public Rigidbody2D carBody;
        public Transform tireTransform;
        public float suspensionRestDist = 0.5f;  // Resting distance of suspension
        public float springStrength = 20000f;    // How stiff the spring is
        public float springDamper = 1500f;       // How much damping (resists oscillation)
        public LayerMask groundLayer;

        private void FixedUpdate()
        {
            // Direction of suspension (local up of the tire)
            Vector2 springDir = tireTransform.up;

            // Start and direction of the suspension ray
            Vector2 rayOrigin = tireTransform.position;
            Vector2 rayDir = -springDir; // usually downwards from the tire

            // Perform a raycast down to detect the ground
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDir, suspensionRestDist, groundLayer);

            if (hit)
            {
                // How much the suspension is compressed
                float offset = suspensionRestDist - hit.distance;

                // Get the tires world velocity
                Vector2 tireWorldVel = carBody.GetPointVelocity(tireTransform.position);

                // Velocity along the spring direction (project onto spring direction)
                float vel = Vector2.Dot(springDir, tireWorldVel);

                // Calculate spring force (Hookes Law) with damping
                float forceMag = (offset * springStrength) - (vel * springDamper);

                // Apply force upwards at tire position
                carBody.AddForceAtPosition(springDir * forceMag, tireTransform.position);
            }
        }

        void OnDrawGizmos()
        {
            // Draw ray in editor
            if (tireTransform != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(tireTransform.position, tireTransform.position - tireTransform.up * suspensionRestDist);
            }
        }
}