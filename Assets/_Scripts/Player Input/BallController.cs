using System.Collections;
using CaromBilliards.BallType;
using CaromBilliards.CoreMechanic;
using CaromBilliards.CoreMechanic.PlayState;
using CaromBilliards.CoreMechanic.ReplayState;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace CaromBilliards.Player_Input
{
    public class BallController : MonoBehaviour
    {
        public delegate void Shoot();

        public static event Shoot OnShoot;
        [field: HideInInspector] public float timePower { get; private set; }
        public bool isShooting { get; private set; }

        public bool CanPlay => _canPlay;
        [SerializeField] private Transform parentOfCamTransform;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private BallSO respectiveBall;
        [SerializeField] private float distanceRay;
        [FormerlySerializedAs("forceSpeed")] [SerializeField] private float powerShot;
        [SerializeField] private AudioClip[] audioClips;
        [SerializeField] private AudioMixer audioMixer;
        private Transform _transform;
        private Rigidbody _myRigidbody;
        private SphereCollider _sphereCollider;
        private AudioSource _audioSource;
        private float _rotation;
        private float _actualRadius;
        private bool _canPlay;
        private bool _onReplay;
        private bool _isGameManagerNull;
        private void Awake()
        {
            respectiveBall.ApplyMaterial(GetComponent<MeshRenderer>());
            _myRigidbody = GetComponent<Rigidbody>();
            _sphereCollider = GetComponent<SphereCollider>();
            _transform = GetComponent<Transform>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
                PlayStateManager.OnAllRigidbodysStopped += InstanceOnOnAllRigidbodysStopped;
                Replay.OnPlayBackState += ReplayOnOnPlayBackState;
        }

        private void ReplayOnOnPlayBackState()
        {
            _onReplay = !_onReplay;
            lineRenderer.enabled = false;
        }

        private void OnDisable()
        {
                PlayStateManager.OnAllRigidbodysStopped -= InstanceOnOnAllRigidbodysStopped;
                Replay.OnPlayBackState -= ReplayOnOnPlayBackState;
        }
        private void Start()
        {
            _canPlay = true;
            _isGameManagerNull = GameManager.Instance; 
            _actualRadius = _sphereCollider.radius * Mathf.Max(_transform.lossyScale.x,_transform.lossyScale.y,_transform.lossyScale.z);
        } 

        private void Update()
        {
            if (!_isGameManagerNull) return;
            if (GameManager.Instance.scoreManager.gameCompleted) return;
            if (Input.GetMouseButton(0))
                RotatePlayerWithCamera();
            if (_canPlay && !_onReplay)
            {
                DrawLineRenderer();
                CuePower();
            }
        }

        private void DrawLineRenderer()
        {
            lineRenderer.enabled = true;
            if (Physics.SphereCast(ShootRay(distanceRay), _actualRadius, out RaycastHit hit))
            {
                lineRenderer.SetPosition(0, ShootRay(distanceRay).origin);
                lineRenderer.SetPosition(1, hit.point);
            }
        }
        /// <summary>
        /// Gets the amount of power to hit the cue bal as long as the player has Spacebar pressed.
        /// Also draws the Line with LineRenderer
        /// </summary>
        private void CuePower()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                isShooting = true;
                if (timePower >= 20.0f) return;
                timePower += 10f * Time.deltaTime;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                _canPlay = false;
                OnShoot?.Invoke();
                StartCoroutine(ApplyForce());
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
        
        private void InstanceOnOnAllRigidbodysStopped() => _canPlay = true;

        private IEnumerator ApplyForce()
        {
            isShooting = false;
            lineRenderer.enabled = false;
            yield return null;
            _myRigidbody.AddForce(_transform.forward * (powerShot * timePower), ForceMode.Impulse);
            _audioSource.PlayOneShot(audioClips[0]);
            timePower = 0;
            GameManager.Instance.totalShotsManager.SetTotalShots();
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
                _audioSource.PlayOneShot(audioClips[1]);
                GameManager.Instance.scoreManager.AddCollidedScore(collision.gameObject.name);
            }
            else if (collision.gameObject.CompareTag("Wall"))
            {
                audioMixer.GetFloat("MasterVolume", out float value);
                _audioSource.volume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 20f);
                _audioSource.clip = audioClips[2];
                _audioSource.Play();
            }
        }
    }
}
