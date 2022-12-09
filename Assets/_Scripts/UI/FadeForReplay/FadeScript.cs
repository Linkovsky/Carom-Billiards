using System;
using CaromBilliards.CoreMechanic.ReplayState;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CaromBilliards.CoreMechanic.UI.Visuals
{
    public class FadeScript : MonoBehaviour
    {
        public UnityEvent OnStartingFade;
        public UnityEvent OnFullFade;
        public UnityEvent OnBlankFade;
        [SerializeField] private float fadePercentage = 1;
        [SerializeField] private bool canFade;
        private Image _image;
        
        private static readonly int CircleSize = Shader.PropertyToID("_circleSize");
        private static readonly int Background = Shader.PropertyToID("_background");

        private void Awake()
        {
            _image = GetComponent<Image>();
        }
        /// <summary>
        /// Set the material float and color to its default state
        /// </summary>
        private void Start()
        {
            _image.material.SetFloat(CircleSize, fadePercentage);
            _image.material.SetColor(Background, Color.black);
        }
        /// <summary>
        /// Update the float value of the material each frame
        /// </summary>
        private void Update()
        {
            if(canFade)
                _image.material.SetFloat(CircleSize, fadePercentage);
        }
        /// <summary>
        /// The animation started, starting to fade and invoke saying it's starting
        /// </summary>
        public void StartFade()
        {
            OnStartingFade.Invoke();
        }
        /// <summary>
        /// It's now full black and invoke saying it is done
        /// </summary>
        public void FullBlackFade() {
            OnFullFade.Invoke();
        }
        /// <summary>
        /// The end of the animation meaning the material is at its default values and invokes.
        /// </summary>
        public void FullBlankFade()
        {
            OnBlankFade.Invoke();
        }
    }
}

