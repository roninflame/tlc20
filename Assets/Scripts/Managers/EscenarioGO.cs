
using PolloScripts.Enums;
using UnityEngine;

namespace PolloProyect
{
    public class EscenarioGO : MonoBehaviour
        //, IEscenario
    {
        //public E_TipoEscenarioGo Tipo = E_TipoEscenarioGo.Ninguno;
        public GameObject Pivot;

        public bool RotAutomatica;
        public DirectionType DirRotacion;
        public float RapidezRot;
        private Vector3 _resRotacion;
		void Start (){
			Iniciar();
		}
        public void Iniciar()
        {
            if (RotAutomatica)
            {
                int r = Random.Range(0, 5);

                if (r == 0)
                    _resRotacion = Vector3.right;
                else if (r == 1)
                    _resRotacion = Vector3.left;
                else if (r == 2)
                    _resRotacion = Vector3.down;
                else if (r == 3)
                    _resRotacion = Vector3.up;
                else if (r == 4) // arriba
                    _resRotacion = Vector3.back;
                else if (r == 5) // abajo
                    _resRotacion = Vector3.forward;
            }
            else
            {
                if (DirRotacion == DirectionType.ForwardX)
                    _resRotacion = Vector3.right;
                else if (DirRotacion == DirectionType.BackwardX)
                    _resRotacion = Vector3.left;
                else if (DirRotacion == DirectionType.RightY)
                    _resRotacion = Vector3.down;
                else if (DirRotacion == DirectionType.LeftY)
                    _resRotacion = Vector3.up;
                else if (DirRotacion == DirectionType.LeftZ)
                    _resRotacion = Vector3.back;
                else if (DirRotacion == DirectionType.RightZ)
                    _resRotacion = Vector3.forward;
            }
        }

        void Update()
        {
            if (Pivot.activeInHierarchy)
            {
                Pivot.transform.localRotation *= Quaternion.AngleAxis(RapidezRot * Time.deltaTime, _resRotacion);
            }
            
        }
        public void SetActive(bool b)
        {
            Pivot.SetActive(b);
        }

        
    }
}

