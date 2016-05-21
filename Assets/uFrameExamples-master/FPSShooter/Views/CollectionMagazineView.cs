using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;


public partial class CollectionMagazineView 
{ 

    /// Subscribes to the property and is notified anytime the value changes.
    public override void AmountChanged(Int32 value) {
        base.AmountChanged(value);
    }

    /// Subscribes to the property and is notified anytime the value changes.
    public override void RoundSizeChanged(Int32 value) {
        base.RoundSizeChanged(value);
    }

    /// Subscribes to the property and is notified anytime the value changes.
    public override void MagazineTypeChanged(CollectionMagazineType value) {
        base.MagazineTypeChanged(value);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}
