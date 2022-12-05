using System;
using UnityEditor;
using UnityEngine;

namespace CaromBilliards.Player_Input
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private Transform parentOfCamTransform;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float distanceRay;
        [SerializeField] private float forceSpeed;

        private Transform _transform;
        private Rigidbody _myRigidbody;
        private SphereCollider _sphereCollider;
        private float _rotation;
        private float _actualradius;
        private void Awake()
        {
            _myRigidbody = GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();
            _sphereCollider = GetComponent<SphereCollider>();
        }

        private void Start()
        {
            _actualradius = _sphereCollider.radius * Mathf.Max(_transform.lossyScale.x,_transform.lossyScale.y,_transform.lossyScale.z);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RotatePlayerWithCamera();
                lineRenderer.enabled = true;
                if (Physics.SphereCast(ShootRay(distanceRay), _actualradius, out RaycastHit hit))
                {
                    lineRenderer.SetPosition(0, ShootRay(distanceRay).origin);
                    lineRenderer.SetPosition(1, hit.point);
                }
            }
            else
            {
                lineRenderer.enabled = false;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                _myRigidbody.AddForce(_transform.forward * forceSpeed, ForceMode.Impulse);
            }
        }
        
        /// <summary>
        /// Rotates the player with the parent Y rotation of the camera.
        /// </summary>
        private void RotatePlayerWithCamera()
        {
            _rotation = parentOfCamTransform.eulerAngles.y;
            _transform.eulerAngles = new Vector3(0, _rotation,0);
        }
        
        /// <summary>
        /// Creates a Ray with its origin being the cue ball transform and the destination as forward.
        /// </summary>
        /// <param name="value">The length of the ray</param>
        private Ray ShootRay(float value)
        {
            return new Ray(_transform.position, _transform.forward * value);
        }
    }
}
