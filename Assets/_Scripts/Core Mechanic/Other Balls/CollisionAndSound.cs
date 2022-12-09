using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaromBilliards.CoreMechanic.BallsMechanic.Sound
{
    public class CollisionAndSound : MonoBehaviour
    {
        [SerializeField] private AudioClip[] audioClips;
    
        private AudioSource _audioSource;
        private const string _ball = "Ball";
        private const string _wall = "Wall";
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        /// <summary>
        /// Determines in a switch statement if the name of the object we just collided matches both ball or wall
        /// If it matches then, assign to the audiosource.volume clamping the value of the velocity the object was travelling
        /// divided by max velocity
        /// </summary>
        /// <param name="nameOfBall">The name we get from the rigidbody.gameObject.name</param>
        /// <param name="collision">The object we just collided with</param>
        private void WhichAudioToPlay(string nameOfBall, Collision collision)
        {
            switch (nameOfBall)
            {
                case _ball:
                    _audioSource.volume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 20f); // 20 being the maximum velocity allowed
                    _audioSource.clip = audioClips[0];
                    _audioSource.Play();
                    break;
                case _wall:
                    _audioSource.volume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 20f); // 20 being the maximum velocity allowed
                    _audioSource.clip = audioClips[1];
                    _audioSource.Play();
                    break;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(_ball))
            {
                WhichAudioToPlay(_ball, collision);
            }
            else if(collision.gameObject.CompareTag(_wall))
            {
                WhichAudioToPlay(_wall, collision);
            }
        }
    }
}

