using UnityEngine;

public class FPSBulletView : MonoBehaviour
{
    public float _BulletSpeed = 10f;
    private FPSBulletDamage bulletDamage;

    public void Awake()
    {
        hideFlags = HideFlags.HideInHierarchy;
    }
    public void Start()
    {
        bulletDamage = gameObject.GetComponent<FPSBulletDamage>();
        Destroy(this.gameObject,2f);
        //Debug.Log(bulletDamage);
    }
    public void Update()
    {
        this.transform.position += (this.transform.forward * _BulletSpeed * Time.deltaTime);
        //int d = bulletDamage.FPSWeapon.Damage;
        //var damageable = transform.collider.gameObject.GetView<FPSDamageableViewBase>();
        //if (damageable != null)
        //    damageable.ExecuteApplyDamage(bulletDamage.FPSWeapon.Damage);

    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.name == "FPSEnemy(Clone)")
    //    {
    //        Debug.Log("Carsh");
    //        Destroy(gameObject);
    //        bulletDamage.crash(other);
    //        other.gameObject.GetView<FPSDamageableViewBase>().ExecuteApplyDamage(bulletDamage.FPSWeapon.Damage);
    //        Debug.Log("Carsh");
    //    }
    //}
}
