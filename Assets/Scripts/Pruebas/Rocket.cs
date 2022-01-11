using PolloScripts;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Pruebas
{
    public class Rocket : MonoBehaviour
    {
        public float speed;
        public float degree;
        private float _timeToFollow;
        private float _timeToDie;

        private bool _die;
        void Start()
        {
            
        }

  
        void Update()
        {

            if (Vector3.Distance(transform.position, PlayerManager.Instance.player.hitPosition) > 10 && !_die)
            {
                if (_timeToFollow < 1)
                {
                    _timeToFollow += Time.deltaTime;
                }
                else
                {
                    Quaternion rotPlayer = Quaternion.LookRotation(PlayerManager.Instance.player.hitPosition - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotPlayer, degree * Time.deltaTime);
                }
                transform.position = transform.position + transform.forward * Time.deltaTime * speed;
            }
            else
            {
                _die = true;
                transform.position = transform.position + transform.forward * Time.deltaTime * speed;
                if(_timeToDie < 2)
                {
                    _timeToDie += Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
               
        }
    }
}