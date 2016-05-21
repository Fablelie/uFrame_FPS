using System.Globalization;
using Invert.StateMachine;
using UniRx;
using UnityEngine;

public partial class FPSWeaponView
{
    private ModelCollection<CollectionMagazineViewModel> ob;
    public FPSPlayerViewModel oo;
    //public int countMag;
    /// Invokes ReloadExecuted when the Reload command is executed.
    public override void ReloadExecuted() {
        base.ReloadExecuted();
    }
 

    public override void DamageChanged(int value) 
    {
        base.DamageChanged(value);
    }
 

    public float _CrossHairScale = 0.5f;
 
    public Transform _ZoomPositions;
    public Texture2D _HudTexture;
    private Vector3 _StartPosition;
    private Quaternion _StartRotation;
    public Transform _ModelTransform;
    public AudioClip ReloadSound;
    public AudioClip EmptySound;

    public override void AmmoChanged(int value)
    {
        base.AmmoChanged(value);
    }

    public override void ZoomIndexChanged(int zoomIndex)
    {
        base.ZoomIndexChanged(zoomIndex);
        FPSCrosshair.ResetCrosshair();
        var zoomTransform = transform.FindChild(zoomIndex.ToString(CultureInfo.InvariantCulture));
        if (zoomTransform != null)
        {
            _StartPosition = zoomTransform.localPosition;
            _StartRotation = zoomTransform.localRotation;
        }
    }
    
    public override void StateChanged(State value)
    {
        if (!this.gameObject.activeInHierarchy) return;
        if (!this.gameObject.activeSelf) return;
        // If we are reloading hide the gun
        //_ModelTransform.gameObject.SetActive(!(value is Reloading));
        if(value is Reloading)
        {
          
            if (FPSWeapon.CollectionMag > 0)
            {
                AudioSource.PlayClipAtPoint( ReloadSound, this.transform.position);
                for (int i = 0; i < oo.Magazine.Count; i++)
                {
                    if (oo.Magazine[i].MagazineType.ToString() == FPSWeapon.WeaponType.ToString())
                    {
                        //Debug.Log("dsadasdsadasdsadad");
                        FPSWeapon.Ammo = oo.Magazine[i].Amount;
                        FPSWeapon.RoundSize = oo.Magazine[i].RoundSize;
                        oo.CollectedMag--;
                        FPSWeapon.CollectionMag--;
                        oo.Magazine.Remove(oo.Magazine[i]);
                        break;
                    }
                }
            }
            //if(FPSplayer._MagazineList.Count > 0 && FPSplayer._BindMagazine)
            //{
            //    FPSWeapon.Ammo = FPSplayer._MagazineList[0]._Amount;


            //}
            //if(FPSWeapon.CollectionMag > 0 )
            //{
            //    Debug.Log("dsadasdsadasdsadad");
            //    FPSWeapon.Ammo = FPSWeapon.RoundSize;
            //    //FPSWeapon.Magazine[0].Amount;
            //    FPSWeapon.CollectionMag--;
            //}
            Invoke("FinishedReload", FPSWeapon.ReloadTime);
            //FPSWeapon.Magazine.RemoveAt(0);
        }

        if (value is Firing)
        {
            //MuzzleFX.gameObject.SetActive(true);
            // Tell the Weapon Fire View Component to start firing
            FPSWeaponFire.Fire();
        }
        else
        {
            //MuzzleFX.gameObject.SetActive(false);
            // Tell the Weapon Fire View Component to stop firing
            FPSWeaponFire.StopFiring();
        }
    }

    private void FinishedReload()
    {
        ExecuteCommand(FPSWeapon.FinishedReloading);
    }

    public override void Bind()
    {
     
        base.Bind();
        //this.BindKey(FPSWeapon.NextZoom, KeyCode.LeftShift);
        this.BindKey(FPSWeapon.Reload, KeyCode.R);

        //this.BindKey(ExecuteReload(), KeyCode.R);

        //this.BindViewTriggerWith<CollectionMagazineView>(CollisionEventType.Enter, Magazineview =>
        //{
        //    Debug.Log("ddddddddd");
        //    Magazineview.ExecuteAddMag();
        //    ExecuteAddMagazine();

        //});


        FPSCrosshair.ResetCrosshair();
    }


    public override void Start()
    {
        oo = FPSWeapon.ParentFPSPlayer;
        //cmvm = FPSWeapon.ParentCollectionMagazine;
        //FPSWeapon.CollectionMag = 0;

        //for (int i = 0; i < oo.Magazine.Count; i++)
        //{
        //    if (oo.Magazine[i].MagazineType.ToString() == FPSWeapon.WeaponType.ToString())
        //    {
        //        FPSWeapon.CollectionMag++;
        //    }
        //}
        //Transform tt = FPSWeaponFire._MuzzleTransform;
        //foreach (Transform t in tt)
        //{
        //    if (t.name == "WFX_MF FPS RIFLE1")
        //    {
        //        Debug.Log("Child Child Child");
        //        FPSWeaponFire.MuzzleFX = t.transform;
        //    }
        //}
    }
    
    public override void Update()
    {
        base.Update();
        if (_ModelTransform == null) return;
        _ModelTransform.localPosition = Vector3.Lerp(_ModelTransform.localPosition, _StartPosition, FPSWeapon.RecoilSpeed);
        _ModelTransform.localRotation = Quaternion.Lerp(_ModelTransform.localRotation, _StartRotation, FPSWeapon.RecoilSpeed);

        FPSWeapon.CollectionMag = 0;
        for (int i = 0; i < oo.Magazine.Count; i++)
        {
            if (oo.Magazine[i].MagazineType.ToString() == FPSWeapon.WeaponType.ToString())
            {
                FPSWeapon.CollectionMag++;
            }
        }

        // then start fire
        if (gameObject.activeSelf && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl)))
        {
            if (FPSWeapon.State is Idle)
            {
                ExecuteBeginFire();
            }
            else if (FPSWeapon.State is Empty)
            {
                AudioSource.PlayClipAtPoint( EmptySound, this.transform.position);
                ExecuteReload();
            }
        }

        // then end fire
        if (gameObject.activeSelf && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.LeftControl)))
        {
            ExecuteEndFire();
        }

        //if(gameObject.activeSelf && (Input.GetKeyDown(KeyCode.E)))
        //{

        //    Debug.Log("E E E E E");
        //    var ray = Camera.main.ViewportPointToRay(
        //    new Vector3(
        //        Random.Range(0, 0) + 0.5f,
        //        Random.Range(0, 0) + 0.5f, 0f));

        //    RaycastHit hit;
        //    Vector3 targetPoint;
        //    if (Physics.Raycast(ray, out hit, 100f))
        //    {
        //        targetPoint = hit.point;
        //        if(hit.transform.tag == "Magazine")
        //        {
        //            Debug.Log("Mag mag mag");
        //            m = hit.transform.gameObject.GetComponent<CollectionMagazineView>();
        //            //oo.PickupMagazine(oo, cmvm.CreateCollectionMagazine(m.name,
        //            //    m.CollectionMagazine.MagazineType,
        //            //    m.CollectionMagazine.Amount,
        //            //    m.CollectionMagazine.RoundSize));
        //            pc.PickupMagazine(oo, cmvm.CreateCollectionMagazine(m.name,
        //                m.CollectionMagazine.MagazineType,
        //                m.CollectionMagazine.Amount,
        //                m.CollectionMagazine.RoundSize));
        //        }
        //    }
        //    else
        //        targetPoint = ray.GetPoint(100f);
        //}
    
    }

}