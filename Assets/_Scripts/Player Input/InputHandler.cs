using System;
using UnityEngine;

namespace CaromBilliards.Player_Input
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float forceSpeed;

        private LineRenderer _lineRenderer;
        private Rigidbody _myRigidbody;
        private void Awake()
        {
            _myRigidbody = GetComponent<Rigidbody>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position, cam.transform.forward);
                if (Physics.Raycast(ray, out hit))
                {
                   _lineRenderer.SetPosition(0, transform.position);
                   _lineRenderer.SetPosition(1, new Vector3(ray.direction.x, transform.position.y, ray.direction.z));
                }
            }
            if (Input.GetKey(KeyCode.Space))
            {
                _myRigidbody.AddForce(cam.transform.forward * forceSpeed, ForceMode.Impulse);
            }
        }
    }
}
