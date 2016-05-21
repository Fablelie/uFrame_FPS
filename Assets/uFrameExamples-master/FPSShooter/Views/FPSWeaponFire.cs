using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(FPSWeaponView))]
public partial class FPSWeaponFire
{
    public Transform _MuzzleTransform;
    public GameObject _BulletPrefab;
    public GameObject BulletImpact;
    public AudioClip FXFireSound;
    private Vector3 _StartPosition;
    private Quaternion _StartRotation;
    private FPSBulletDamage FPSBulletDamage;
    private Transform MuzzleFX;
    
    public float _Spread;

    public void Start()
    {
        //Transform t = _MuzzleTransform.gameObject.GetComponentInChildren<Transform>();
        foreach (Transform s in _MuzzleTransform)
        {
            if (s.name == "WFX_MF FPS RIFLE1")
            {
                //Debug.Log("Child Child Child");
                MuzzleFX = s;
            }
        }
    }

    public override void Bind(ViewBase view)
    {
        base.Bind(view);
        _Spread = FPSWeapon.MinSpread;

        //view.BindInputButton(() => FPSWeapon.BeginFire, "Fire1", InputButtonEventType.ButtonDown)
        //    .When(() => view.gameObject.activeInHierarchy && FPSWeapon.State == FPSWeaponState.Active).Subscribe(Fire);

        //view.BindInputButton(() => FPSWeapon.EndFire, "Fire1", InputButtonEventType.ButtonUp)
        //    .When(() => view.gameObject.activeInHierarchy && FPSWeapon.State == FPSWeaponState.Firing);
    }

   
    protected IEnumerator FireWeapon()
    {

        if (FPSWeapon.Ammo > 0)
            MuzzleFX.gameObject.SetActive(true);
        else
            MuzzleFX.gameObject.SetActive(false);
        
        var bulletsToFire = Math.Min(FPSWeapon.BurstSize, FPSWeapon.Ammo);
        for (var i = 0; i < bulletsToFire; i++)
        {
            //FPSBulletDamage.start();
            FireBullet();
            yield return new WaitForSeconds(FPSWeapon.BurstSpeed);
            _Spread += FPSWeapon.SpreadMultiplier;

            MuzzleFX.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(FPSWeapon.FireSpeed);

        if (FPSWeapon.State is Firing && FPSWeapon.BurstSize < 2)
            StartCoroutine("FireWeapon");
    }

    public void FireBullet()
    {
        AudioSource.PlayClipAtPoint(FXFireSound, _MuzzleTransform.position);
        var bullet = Instantiate(_BulletPrefab) as GameObject;
        ExecuteBulletFired();

        Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));

        var ray = Camera.main.ViewportPointToRay(
            new Vector3(
                Random.Range(-_Spread, _Spread) + 0.5f,
                Random.Range(-_Spread, _Spread) + 0.5f, 0f));
        
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            targetPoint = hit.point;

            var damageable = hit.collider.gameObject.GetView<FPSDamageableViewBase>();
            if (damageable != null)
                damageable.ExecuteApplyDamage(FPSWeapon.Damage);

            if (hit.transform.tag == "anything" || hit.transform.tag == "Magazine")
            {
                var hitRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                Instantiate(BulletImpact, hit.point, hitRotation);
            }
        }
        else
            targetPoint = ray.GetPoint(100f);

        var finalPoint = new Vector3(
            targetPoint.x,
            targetPoint.y,
            targetPoint.z
            );

        bullet.transform.parent = null;
        bullet.transform.position = _MuzzleTransform.position;

        bullet.gameObject.transform.LookAt(finalPoint);
    }

    public IEnumerator SpreadDown()
    {
        while (_Spread > FPSWeapon.MinSpread)
        {
            yield return new WaitForSeconds(FPSWeapon.RecoilSpeed);
            _Spread -= FPSWeapon.RecoilSpeed;
        }
        _Spread = FPSWeapon.MinSpread;
    }
  
    public void Fire()
    {
        StartCoroutine("FireWeapon");
    }

    public void StopFiring()
    {
        if(MuzzleFX != null)
            MuzzleFX.gameObject.SetActive(false);
        StartCoroutine(SpreadDown());
        StopCoroutine("FireWeapon");
    }
}