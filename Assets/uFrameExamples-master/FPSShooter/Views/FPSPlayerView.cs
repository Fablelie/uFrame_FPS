using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class FPSPlayerView
{ 

    /// Invokes NextWeaponExecuted when the NextWeapon command is executed.
    public override void NextWeaponExecuted() {
        base.NextWeaponExecuted();
    }

    /// Invokes PreviousWeaponExecuted when the PreviousWeapon command is executed.
    public override void PreviousWeaponExecuted() {
        base.PreviousWeaponExecuted();
    }

    /// Invokes SelectWeaponExecuted when the SelectWeapon command is executed.
    public override void SelectWeaponExecuted() {
        base.SelectWeaponExecuted();
    }


    void Start()
    {
        FPSPlayer.FirstTime = true;
    }

    //public FPSPlayerControllerBase pcb;
    /// Subscribes to collection modifications.  Add & Remove methods are invoked for each modification.
    public override void MagazineAdded(CollectionMagazineViewModel item) 
    {
        base.MagazineAdded(item);
        FPSPlayer.CollectedMag++;
        _MagazineList.Add(item);
    }
    
    /// Subscribes to collection modifications.  Add & Remove methods are invoked for each modification.
    public override void MagazineRemoved(CollectionMagazineViewModel item) 
    {
        base.MagazineRemoved(item);
        //Destroy(item);
        //DropMag(item);
        _MagazineList.Remove(item);

    }

    public ViewBase DropMag(CollectionMagazineViewModel Magazine)
    {
        var prefabName = Magazine.MagazineType.ToString() + "Magazine";
        var mag = InstantiateView(prefabName, Magazine);
        mag.transform.parent = null;
        mag.transform.position = transform.position;
        mag.InitializeData(Magazine);
        return mag;
    }
 
    public List<FPSWeaponViewBase> _WeaponsList = new List<FPSWeaponViewBase>();
    public List<CollectionMagazineViewModel> _MagazineList = new List<CollectionMagazineViewModel>();

    public override void Awake()
    {
        base.Awake();
    }

    ///// This binding will add or remove views based on an element/viewmodel collection.
    //public override ViewBase CreateMagazineView(CollectionMagazineViewModel item)
    //{
    //    var prefabName = item.MagazineType.ToString() + "Magazine";
    //    var mag = InstantiateView(prefabName, item);
    //    mag.transform.parent = _MagazineContainer;
    //    //mag.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    //    //mag.transform.localPosition = new Vector3(0f, 0f, 0f);
    //    mag.gameObject.SetActive(false);
    //    mag.InitializeData(item);
    //    return mag;
    //}

    ///// This binding will add or remove views based on an element/viewmodel collection.
    //public override void MagazineAdded(ViewBase item)
    //{
    //    base.MagazineAdded(item);
    //    FPSPlayer.CollectedMag++;
    //    _MagazineList.Add(item as CollectionMagazineViewBase);
    //}

    ///// This binding will add or remove views based on an element/viewmodel collection.
    //public override void MagazineRemoved(ViewBase item)
    //{
    //    base.MagazineRemoved(item);
    //    Destroy(item.gameObject);
    //    _MagazineList.Remove(item as CollectionMagazineViewBase);
    //}

    public override ViewBase CreateWeaponsView(FPSWeaponViewModel fPSWeapon)
    {
        var prefabName = fPSWeapon.WeaponType.ToString() + "Weapon";
        var weapon = InstantiateView(prefabName, fPSWeapon);
        weapon.transform.parent = _WeaponsContainer;
        weapon.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        weapon.transform.localPosition = new Vector3(0f, 0f, 0f);
        weapon.InitializeData(fPSWeapon);
        return weapon;
    }

    public override void WeaponsAdded(ViewBase item)
    {
        base.WeaponsAdded(item);
        _WeaponsList.Add(item as FPSWeaponViewBase);
        CurrentWeaponIndexChanged(FPSPlayer.CurrentWeaponIndex);
    }

    public override void WeaponsRemoved(ViewBase item)
    {
        base.WeaponsRemoved(item);
        Destroy(item.gameObject);
        _WeaponsList.Remove(item as FPSWeaponViewBase);
        CurrentWeaponIndexChanged(FPSPlayer.CurrentWeaponIndex);
    }

    public override void Bind()
    {
        base.Bind();
        //this.BindKey(FPSPlayer.SelectWeapon, KeyCode.Alpha1, 0);
        //this.BindKey(FPSPlayer.SelectWeapon, KeyCode.Alpha2, 1);
        //this.BindKey(FPSPlayer.SelectWeapon, KeyCode.Alpha3, 2);
        //this.BindKey(FPSPlayer.NextWeapon, KeyCode.E);
        //this.BindKey(FPSPlayer.PreviousWeapon, KeyCode.Q);
        //this.BindViewCollision(CollisionEventType.Enter, _ => ExecuteApplyDamage(10));

    }

    public void ApplyDamage()
    {
        ExecuteApplyDamage(20);
    }

    public override void AfterBind()
    {
        base.AfterBind();
        ExecuteSelectWeapon(0);
    }

    public override void CurrentWeaponIndexChanged(int value)
    {
        base.CurrentWeaponIndexChanged(value);
        //Debug.Log(value);
        for (var i = 0; i < this._WeaponsList.Count; i++)
            _WeaponsList[i].gameObject.SetActive(i == value);
    }

    public override void CollectedMagChanged(Int32 value)
    {
        base.CollectedMagChanged(value);
        
    }

    public override void PickupMagazineExecuted()
    {
        base.PickupMagazineExecuted();
    }

    public override void Write(ISerializerStream stream)
    {
        base.Write(stream);


    }

    public override void Update()
    {

        if(Input.GetKeyDown(KeyCode.Q))
        {
            ExecutePreviousWeapon();
        }

        if (gameObject.activeSelf && (Input.GetKeyDown(KeyCode.E)))
        {

            //Debug.Log("E E E E E");
            var ray = Camera.main.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f));

            RaycastHit hit;
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                targetPoint = hit.point;
                if (hit.transform.tag == "Magazine")
                {
                    //Debug.Log("Mag mag mag");
                    CollectionMagazineView m = hit.transform.gameObject.GetComponent<CollectionMagazineView>();
                    //Debug.Log(m.name);
                    //Debug.Log(m.CollectionMagazine.MagazineType);
                    //Debug.Log(m.CollectionMagazine.Amount);
                    //oo.PickupMagazine(oo, cmvm.CreateCollectionMagazine(m.name,
                    //    m.CollectionMagazine.MagazineType,
                    //    m.CollectionMagazine.Amount,
                    //    m.CollectionMagazine.RoundSize));
                    //CollectionMagazineController d = null;
                    //CollectionMagazineViewModel r;
                    //r = d.CreateCollectionMagazine(m.name, m.CollectionMagazine.MagazineType, m.CollectionMagazine.Amount, m.CollectionMagazine.RoundSize);

                    ExecutePickupMagazine(bobo(m.name, m.CollectionMagazine.MagazineType, m.CollectionMagazine.Amount, m.CollectionMagazine.RoundSize));
                    hit.transform.gameObject.SendMessage("Destroy");
                }
            }
            else
                targetPoint = ray.GetPoint(100f);
        }
    }

    public CollectionMagazineViewModel bobo(string identifier, CollectionMagazineType MagType, int Amount, int RoundSize)
    {
        CollectionMagazineViewModel Mag = new CollectionMagazineViewModel();
        Mag.Identifier = identifier;
        Mag.MagazineType = MagType;
        Mag.Amount = Amount;
        Mag.RoundSize = RoundSize;
        return Mag;
    }

    public override void Read(ISerializerStream stream)
    {
        base.Read(stream);


    }
}