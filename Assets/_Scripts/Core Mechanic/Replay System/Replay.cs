using System;
using System.Collections.Generic;
using CaromBilliards.CoreMechanic.PlayState;
using CaromBilliards.CoreMechanic.UI;
using CaromBilliards.Player_Input;
using Unity.VisualScripting;
using UnityEngine;

namespace CaromBilliards.CoreMechanic.ReplayState
{
    public class Replay : MonoBehaviour
    {
        public delegate void StartedReplay();
        public static event StartedReplay OnPlayBackState;
        public bool Playback
        {
            get => _playBack;
        }
        [SerializeField] private new Transform[] transform;
        [SerializeField] private new Rigidbody[] rigidbody;
        private List<RecordPlay> recordPlay = new List<RecordPlay>();
        private BallController _ballController;
        private UIManager _uiManager;
        private int _numberOfShotsAllowed;
        private bool _playBack;
        private bool _startRecord;
        private int _currentIndex;
        
        private void Awake()
        {
            _uiManager = FindObjectOfType<UIManager>();
        }
        private void OnEnable()
        {
            BallController.OnShoot += BallControllerOnOnShoot;
            PlayStateManager.OnAllRigidbodysStopped += PlayStateManagerOnOnAllRigidbodysStopped;
        }

        private void PlayStateManagerOnOnAllRigidbodysStopped()
        {
            _startRecord = false;
        }

        private void OnDisable()
        {
            BallController.OnShoot -= BallControllerOnOnShoot;
            PlayStateManager.OnAllRigidbodysStopped -= PlayStateManagerOnOnAllRigidbodysStopped;
        }
        private void Start()
        {
            _numberOfShotsAllowed = 1;
            
        }

        private void BallControllerOnOnShoot()
        {
            if (_numberOfShotsAllowed > 1)
            {
                recordPlay.Clear();
                _numberOfShotsAllowed = 1;
            }
            _startRecord = true;
            _numberOfShotsAllowed++;
        }

        private void Update()
        {
            if(_playBack)
            {
                ReplayShot();
            }
            else if (_startRecord)
            {
                recordPlay.Add(new RecordPlay
                {
                    ballPosition = transform[0].position,
                    ballPosition2 = transform[1].position,
                    ballPosition3 = transform[2].position
                });
            }
        }

        private void ReplayShot()
        {
            rigidbody[0].isKinematic = true;
            rigidbody[1].isKinematic = true;
            rigidbody[2].isKinematic = true;
            if (recordPlay.Count > 0)
                ReplayPositions(_currentIndex);
            else
            {
                rigidbody[0].isKinematic = false;
                rigidbody[1].isKinematic = false;
                rigidbody[2].isKinematic = false;
                _numberOfShotsAllowed = 1;
                OnPlayBackState?.Invoke();
                _playBack = false;
            }
        }

        public void StartPlayback()
        {
            OnPlayBackState?.Invoke();
            _playBack = true;
        }
        private void ReplayPositions(int index)
        {
            RecordPlay recordedPlay = recordPlay[index];
            transform[0].position = recordedPlay.ballPosition;
            transform[1].position = recordedPlay.ballPosition2;
            transform[2].position = recordedPlay.ballPosition3;
            recordPlay.RemoveAt(index);
        }
    }
}