using UnityEngine;

namespace CaromBilliards.CameraSettings
{
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float distanceToPlayer = -3;
        [SerializeField, Range(0, 360)] private float rotationSpeed;
    
        private Vector3 previousPosition;
        private Transform playerTransform;
        private void Awake()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Start()
        {
        
            //Setting camera's position equal to the white ball position except for the Z value to give depth
            cam.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, distanceToPlayer);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                RotateCamera();
            }
        }
    
        /// <summary>
        /// Rotates the camera when the player is pressing the right mouse button.
        /// </summary>
        private void RotateCamera()
        {
            Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = previousPosition - newPosition;
            
            float rotationAroundYAxis = -direction.x * rotationSpeed; // camera moves horizontally
            float rotationAroundXAxis = direction.y * rotationSpeed; // camera moves vertically
            
            cam.transform.position = playerTransform.position;
            
            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); 
            
            cam.transform.Translate(new Vector3(0, 0, distanceToPlayer));
            
            previousPosition = newPosition;
        }
    }
}
