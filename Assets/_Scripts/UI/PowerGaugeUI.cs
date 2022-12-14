using System;
using CaromBilliards.CoreMechanic;
using CaromBilliards.Player_Input;
using UnityEngine;
using UnityEngine.UI;

namespace CaromBilliards.CoreMechanic.UI.Visuals
{
    public class PowerGaugeUI : MonoBehaviour
    {
        [SerializeField] private GameObject _powerGaugeUI;
        [SerializeField] private Image _powerGauge;
        private BallController _ballController;
        private bool _isInstanceNull;

        private void Awake()
        {
            _ballController = GameObject.FindGameObjectWithTag("Player").GetComponent<BallController>();
        }

        private void Start()
        {
            _isInstanceNull = GameManager.Instance == null;
            ResetAmount();
        }

        private void ResetAmount()
        {
            _powerGauge.fillAmount = 0;
            _powerGaugeUI.SetActive(false);
        }

        private void Update()
        {
            if (_isInstanceNull) return;
            if (!GameManager.Instance.scoreManager.isBallMoving && _ballController.isShooting)
            {
                _powerGaugeUI.SetActive(true);
                _powerGauge.fillAmount = Mathf.Clamp01(_ballController.timePower / 20f);
            }
            else
            {
                ResetAmount();
            }
        }
    }
}
