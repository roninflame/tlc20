using PolloProyect;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace PolloProyect
{
    public class PerroManager : MonoBehaviour
    {
        [Header("LeanTween")]
        //public PerroLeanTween LeanTweenParms;
        //public LeanTweenType LeanTweenX = LeanTweenType.easeInOutSine;
        //public float MovX = 4f;
        //public float TimeX = 1f;
        //public bool LoopX = true;
        //public LeanTweenType LeanTweenY = LeanTweenType.easeInOutSine;
        //public float MovY = 1f;
        //public float TimeY = 0.5f;
        //public bool LoopY = true;
        //public LeanTweenType LeanTweenZ = LeanTweenType.easeInQuad;
        //public float MovZ = 40f;
        //public float TimeZ = 5f;
        //private LTDescr _tweenObject;
        public ParticleSystem Particles;
        public GameObject Pivot;
        //public EscudoEnergia EscudoEnergia;
        [ColorUsage(true, true)]
        [SerializeField] private Color32 _escudoColors;
        public float Rapidez = -30f;
        private bool _playerContactado;
        //===== Animaciones ===
        //private PlayerAnimations playerAnima;
        [HideInInspector] public Rigidbody Rb;
        void Awake()
        {
            Pivot.GetComponent<PerroCollider>().OnContact += Contacto;
        }
        //private void Start()
        //{
        //    SetPlayerManager();
        //}
        private void Update()
        {
            if(!_playerContactado)
                transform.position += transform.forward * Rapidez * Time.deltaTime;
        }
        //public void SetPlayer(PerroLeanTween perro, float rapidez)
        //{
        //    _playerContactado = false;
        //    Rb = Pivot.GetComponent<Rigidbody>();
        //    playerAnima = GetComponent<PlayerAnimation>();
        //    Rapidez = rapidez;
        //    EscudoEnergia.SetActive(false);
        //    EscudoEnergia.SetColor(_escudoColors);
        //    LeanTweenParms = perro;
        //    gameObject.SetActive(true);
           

        //}
        private void Avanzar()
        {
            //if(LeanTweenParms.LoopX)
            //    LeanTween.moveX(gameObject, LeanTweenParms.MovX, LeanTweenParms.TimeX).setEase(LeanTweenParms.LeanTweenX).setLoopPingPong();
            //else
            //    LeanTween.moveX(gameObject, LeanTweenParms.MovX, LeanTweenParms.TimeX).setEase(LeanTweenParms.LeanTweenX);
            //if (LeanTweenParms.LoopY)
            //    LeanTween.moveY(gameObject, LeanTweenParms.MovY, LeanTweenParms.TimeY).setEase(LeanTweenParms.LeanTweenY).setLoopPingPong();
            //else
            //    LeanTween.moveY(gameObject, LeanTweenParms.MovY, LeanTweenParms.TimeY).setEase(LeanTweenParms.LeanTweenY);
            //LeanTween.moveZ(gameObject, LeanTweenParms.MovZ, LeanTweenParms.TimeZ).setEase(LeanTweenParms.LeanTweenZ).setOnComplete(Desactivar);
            
        }
        public void Desactivar()
        {
            gameObject.SetActive(false);
            //EscudoEnergia.SetActive(false);
            _playerContactado = false;
            //LeanTween.cancelAll();
        }
        private void Contacto(Collider other)
        {
            //PlayerManager player = other.GetComponentInParent<PlayerManager>();
            //if(player != null && !_playerContactado)
            //{
            //    _playerContactado = true;
            //    EscudoEnergia.SetActive(true);
            //    Particles.Stop();
            //    Avanzar();
            //    MisionManager.Instancia.AmigoRescatado();
            //}
        }
    }
}

