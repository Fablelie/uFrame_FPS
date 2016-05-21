using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Random = UnityEngine.Random;

public partial class FPSBulletDamage 
{
    public override void Bind(ViewBase view)
    {
        base.Bind(view);
    }

    public void crash(Collider value)
    {
        Debug.Log("fadfdasfaf");
        value.gameObject.GetView<FPSDamageableViewBase>().ExecuteApplyDamage(FPSWeapon.Damage);
    }

   

}
