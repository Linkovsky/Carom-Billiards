using System;
using Unity.VisualScripting;
using UnityEngine;

namespace CaromBilliards.CameraSettings
{
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField, Range(0, 360)] private float rotationSpeed;
        
        private Transform _parentTransform;
        private Transform _playerTransform;
        
        private Vector3 _previousPosition;
        private Vector3 _offSet;
        
        private float _getInputValue;
        private void Awake()
        {
            _parentTransform = GetComponent<Transform>();
            _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        
        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                RotateCamera();
            }
            _parentTransform.position = _playerTransform.position;
        }
    
        /// <summary>
        /// Rotates the camera when the player is pressing the right mouse button.
        /// </summary>
        private void RotateCamera()
        {
            Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = _previousPosition - newPosition;
            
            float rotationAroundYAxis = -direction.x * rotationSpeed; // camera moves horizontally
            float rotationAroundXAxis = direction.y * rotationSpeed; // camera moves vertically
            
            _parentTransform.position = _playerTransform.position;
            
            _parentTransform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            _parentTransform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);
            
            _previousPosition = newPosition;
        }
    }
}
