using CaromBilliards.BallType;
using CaromBilliards.CoreMechanic;
using CaromBilliards.CoreMechanic.ScoreBoard;
using UnityEngine;

namespace CaromBilliards.Player_Input
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private Transform parentOfCamTransform;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private BallSO respectiveBall;
        [SerializeField] private float distanceRay;
        [SerializeField] private float forceSpeed;
        
        private Transform _transform;
        private Rigidbody _myRigidbody;
        private SphereCollider _sphereCollider;
        private float _rotation;
        private float _actualRadius;
        private void Awake()
        {
            respectiveBall.ApplyMaterial(GetComponent<MeshRenderer>());
            _myRigidbody = GetComponent<Rigidbody>();
            _sphereCollider = GetComponent<SphereCollider>();
            _transform = GetComponent<Transform>();
        }

        private void Start() => _actualRadius = _sphereCollider.radius * Mathf.Max(_transform.lossyScale.x,_transform.lossyScale.y,_transform.lossyScale.z);

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RotatePlayerWithCamera();
                lineRenderer.enabled = true;
                if (Physics.SphereCast(ShootRay(distanceRay), _actualRadius, out RaycastHit hit))
                {
                    lineRenderer.SetPosition(0, ShootRay(distanceRay).origin);
                    lineRenderer.SetPosition(1, hit.point);
                }
            }
            else
                lineRenderer.enabled = false;
            
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _myRigidbody.AddForce(_transform.forward * forceSpeed, ForceMode.Impulse);
                if(GameManager.Instance != null)
                    GameManager.Instance.totalShots.SetTotalShots();
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

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 6)
            {
                _transform.position = new Vector3(_transform.position.x, 0.0325f, _transform.position.z);
                _myRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }
            if(collision.gameObject.CompareTag("Ball"))
            {
                if(GameManager.Instance != null) 
                    GameManager.Instance.scoreManager.AddCollidedScore(collision.gameObject.name);
            }
        }
    }
}
