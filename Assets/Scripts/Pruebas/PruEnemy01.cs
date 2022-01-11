using PolloScripts.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruEnemy01 : MonoBehaviour
{
    public enum Direction
    {
        right,
        left,
        up,
        down
    }
    public Transform holder;
    public LaserSmoke laserSmoke;
    public Direction direction;
    public float laserDelay = 1f;
    public float rotation = 5;
    private Vector3 _dir;

    private bool _activate;

    void Start()
    {
        Activate();
    }

    // Update is called once per frame
    void Update()
    {
        if(_activate)
            holder.Rotate(_dir, rotation * Time.deltaTime);
        
    }
    public void Activate()
    {
        _dir = GetDirection();
        laserSmoke.Activate();
        _activate = true;
        StartCoroutine(IenShoot());
    }
    private Vector3 GetDirection()
    {
        Vector3 res = Vector3.zero;
        if(direction == Direction.right)
        {
            res = Vector3.up;
        }
        else if (direction == Direction.left)
        {
            res = Vector3.down;
        }
        if (direction == Direction.up)
        {
            res = Vector3.left;
        }
        else if (direction == Direction.down)
        {
            res = Vector3.right;
        }
        return res;
    }
    public void Death()
    {
        Destroy(gameObject);
    }
    private IEnumerator IenShoot()
    {
        yield return new WaitForSeconds(laserDelay);
        laserSmoke.Shoot();
    }
}
