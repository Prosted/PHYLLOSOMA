﻿using System;
using Runtime.Weapons;
using UnityEngine;
using UnityEngine.VFX;

namespace Tests.Weapons
{
    //본래는 무기 클래스 구현 후 상속 받은 뒤에 구현되어야 하지만 테스트를 위해 빈 클래스로 구현
    public class Laser : Weapon<Laser>
    {
        [Header("Laser Info")]
        [SerializeField] private LineRenderer _laserLine = null;
        [SerializeField] private VisualEffect _laserEffect = null;
        [SerializeField] private float _chargeTime = 1f;
        private float _lastAttackTime = 0f;
        private float _chargeValue = 0f;
        private bool _isEnable = false;
        public override void Attack(GameObject target, Vector3 attackPoint)
        {
            _chargeValue += Time.deltaTime;
            _laserLine.material.SetFloat("_LaserPower", _chargeValue/_chargeTime);
            _laserLine.SetPositions(new []{transform.position, attackPoint});
            _laserEffect.transform.position = attackPoint;
            
            if (_chargeValue < _chargeTime) {
                return;
            }
            _chargeValue = _chargeTime;
            
            if (Time.time - _lastAttackTime < _delay) return;
            _lastAttackTime = Time.time;
            //공격하는 부분
            //Debug.Log("Laser Attack!");
            //공격할 타겟에게 
            //Calculate hit point
            // shot raycast
            //공격 데미지를 주는 부분
        }
        public override void Ready()
        {
            InternalEnableLaser(true);
        }
        public override void Finish()
        {
            InternalEnableLaser(false);
        }
        /// <summary>
        /// Laser Enable/Disable
        /// </summary>
        /// <param name="isEnable"></param>
        private void InternalEnableLaser(bool isEnable)
        {
            if (_isEnable == isEnable) {
                return;
            }
            _isEnable = isEnable;
            _laserLine.gameObject.SetActive(isEnable);
            if (isEnable) {
                _laserEffect.Play();
            }
            else{
                _laserEffect.Stop();
                _chargeValue = 0f;
                _laserLine.material.SetFloat("_LaserPower", _chargeValue);
                _laserLine.SetPositions(new []{transform.position, transform.position});
            }
        }
    }
}