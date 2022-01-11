using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PruEnemy02 : MonoBehaviour
{
    public Transform Spot;
    public GameObject ringPrefab;
    public float tiempo;
    public Transform _parent;
    public float esperaEntreRing = 1.5f;
    public bool loop;
    public List<Vector3> RingPositionList;
    private void Start()
    {
        StartCoroutine(IenStart());
    }

    IEnumerator IenStart()
    {
        transform.localPosition = new Vector3(0, 50, 60);
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        yield return new WaitForSeconds(tiempo);
        Tween tween;
        tween = transform.DOLocalMove(new Vector3(0, 10, 60), 2);

        yield return tween.WaitForCompletion();

        while (loop)
        {
            foreach (var item in RingPositionList)
            {
                CrearRing(item);
                yield return new WaitForSeconds(esperaEntreRing);
            }
        }

        if(!loop)
        {
            foreach (var item in RingPositionList)
            {
                CrearRing(item);
                yield return new WaitForSeconds(esperaEntreRing);
            }
        }
              
 
    }

    private void CrearRing(Vector3 position)
    {
        GameObject go = Instantiate(ringPrefab, Spot.position, Quaternion.identity) as GameObject;
        go.transform.parent = _parent;

        go.transform.localRotation = Quaternion.Euler(0, 0, 0);
        RingController ring = go.GetComponent<RingController>();
        ring.Move(position);
    }
}
