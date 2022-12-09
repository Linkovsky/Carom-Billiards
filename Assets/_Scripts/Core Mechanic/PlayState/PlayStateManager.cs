using System;
using System.Collections;
using System.Collections.Generic;
using CaromBilliards.Player_Input;
using UnityEngine;

namespace CaromBilliards.CoreMechanic.PlayState
{
    public class PlayStateManager : MonoBehaviour
    {
        public delegate void AllRigidbodysStopped();
        public static event AllRigidbodysStopped OnAllRigidbodysStopped;
        
        private WaitForSeconds _waitTime;
        private Rigidbody[] _allRigidbodys;
        private bool _allStopped;
        private bool _stopThreshHold;
        
        private void Awake()
        {
            _allRigidbodys = FindObjectsOfType<Rigidbody>();
        }

        private void OnEnable()
        {
            BallController.OnShoot += BallControllerOnOnShoot;
        }

        private void OnDisable()
        {
            BallController.OnShoot -= BallControllerOnOnShoot;
        }

        private void BallControllerOnOnShoot()
        {
            _allStopped = false;
            StartCoroutine(CheckIfRigidbodysAreMoving());
        }

        private void Start()
        {
            _waitTime = new WaitForSeconds(0.5f);
        }
        
        private IEnumerator CheckIfRigidbodysAreMoving()
        {
            while (!_allStopped)
            {
                _allStopped = true;
                foreach (Rigidbody currentRb in _allRigidbodys)
                {
                    if (currentRb.velocity.sqrMagnitude > 0.0001f)
                        _allStopped = false;
                    yield return null;
                }
            }
            yield return _waitTime;
            OnAllRigidbodysStopped?.Invoke();
        }
    }
}