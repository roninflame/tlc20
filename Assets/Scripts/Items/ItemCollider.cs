using UnityEngine;

public class ItemCollider : MonoBehaviour
{
    //public delegate void Contact(Collider otro, ITomarItem item);
    //public event Contact OnContact;

    private void OnTriggerEnter(Collider other)
    {
        //ITomarItem iOther = other.GetComponentInParent<ITomarItem>();
        //if (iOther!=null)
        //{
        //    if (iOther.puedeTomarItem)
        //    {
        //        OnContact(other, iOther);
        //    }
        //}
    }
}
