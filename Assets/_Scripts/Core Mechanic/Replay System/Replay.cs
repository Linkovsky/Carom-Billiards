using System.Collections.Generic;
using CaromBilliards.CoreMechanic.PlayState;
using CaromBilliards.Player_Input;
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
        private List<RecordPlay> recordPlay = new();
        private bool _playBack;
        private bool _startRecord;
        private int _currentIndex;
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
        /// <summary>
        /// After the player shoots the cue ball, the controller invokes the event and this class is awaiting its signal,
        /// We check if the list is empty to avoid any errors related to the replay, if it's not we then clear the list.
        /// And set the recording to true, to start recording new positions.
        /// </summary>
        private void BallControllerOnOnShoot()
        {
            if(recordPlay.Count > 0)
                recordPlay.Clear();
            _startRecord = true;
        }
        /// <summary>
        /// If the playback boolean is set to true means that we are currently watching the replay of the shot
        /// the player made. We pass in by parameter the currentindex followed by a ref so we can easily modify the value
        /// without making a copy of it.
        /// If the record boolean is set to true that means we are recording all the positions made by all the balls and
        /// saving those positions in the list as a new object class.
        /// </summary>
        private void Update()
        {
            if(_playBack)
            {
                ReplayShot(ref _currentIndex);
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
        
        /// <summary>
        /// We modify the kinemetic of all the rigidbodies to true so we can move more freely without the physics interfering
        /// with any of the upcoming collisions, because unity physics results in different values everytime even though
        /// we apply the same power we have to set kinemetic to true.
        /// We then check to see if the total count of the list is greater than 0 and the count greater than the current
        /// past index + 1 so we can determine if the list has more position for us to loop through. If not we set everything
        /// back to it's original state, Invoke the event to all the listeners saying we finished replaying the shot,
        /// and set the current index back to default value '0'.
        /// </summary>
        /// <param name="index">The current past index</param>
        private void ReplayShot(ref int index)
        {
            rigidbody[0].isKinematic = true;
            rigidbody[1].isKinematic = true;
            rigidbody[2].isKinematic = true;
            if (recordPlay.Count > 0 && recordPlay.Count > _currentIndex + 1)
                ReplayPositions(ref index);
            else
            {
                rigidbody[0].isKinematic = false;
                rigidbody[1].isKinematic = false;
                rigidbody[2].isKinematic = false;
                OnPlayBackState?.Invoke();
                index = 0;
                _playBack = false;
            }
        }

        public void PreventPlayerFromShooting()
        {
            OnPlayBackState?.Invoke();
        }

        public void StartOnFirstPosition()
        {
            ReplayPositions(ref _currentIndex);
        }

        public void StartPlayBack()
        {
            _playBack = true;
        }
        
        /// <summary>
        /// We create a temporary object of type RecordPlay as it's the name of the struct we were using to store the values
        /// so we can be able to assign the values from the list which had the object saved for us to be able to assign
        /// to the different variables the values we wanted.
        /// </summary>
        /// <param name="index">the current index</param>
        private void ReplayPositions(ref int index)
        {
            RecordPlay recordedPlay = recordPlay[index];
            transform[0].position = recordedPlay.ballPosition;
            transform[1].position = recordedPlay.ballPosition2;
            transform[2].position = recordedPlay.ballPosition3;
            index++;
        }
    }
}