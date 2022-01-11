using PolloScripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackShield : MonoBehaviour
{
    [SerializeField] private Renderer _meshRender;
    [SerializeField] private SphereCollider _sphereCollider;

    public int damage = 1;

    public void Show()
    {
        _sphereCollider.enabled = true;
        _meshRender.enabled = true;
    }

    public void Hide()
    {
        _meshRender.enabled = false;
        _sphereCollider.enabled = false;
    }

    protected void OnTriggerEnter(Collider other)
    {
        IPlayer player = other.GetComponentInParent<IPlayer>();
        if (player != null)
        {
            player.DamageReceived(damage);
        }
    }
}
