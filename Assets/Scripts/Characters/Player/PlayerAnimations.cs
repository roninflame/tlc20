using PolloScripts.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PolloScripts
{
    public class PlayerAnimations : MonoBehaviour
    {

        [SerializeField] private Animator _animator;
        

        //======Animaciones del player
        private int _DeathHash = Animator.StringToHash("Death");
        private int _ShootingHash = Animator.StringToHash("Shooting");
        private int _MovingHash = Animator.StringToHash("MoveX");
        private int _HitHash = Animator.StringToHash("Hit");


        public void ShootingAnimation(bool value)
        {
            _animator.SetBool(_ShootingHash, value);
        }
        public void MoveXAnimation(float valueX)
        {
            _animator.SetFloat(_MovingHash, valueX);
        }

        public void DeathAnimation(bool value)
        {
            _animator.SetBool(_DeathHash, value);
        }

        public void ExpressionAnimation()
        {

        }
      
    }

}
