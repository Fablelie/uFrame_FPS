using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;


public class CollectionMagazineController : CollectionMagazineControllerBase 
{
    public CollectionMagazineViewModel CreateCollectionMagazine(string identifier, CollectionMagazineType MagType, int Amount, int RoundSize)
    {
        var Mag = CreateCollectionMagazine();
        Mag.Identifier = identifier;
        Mag.MagazineType = MagType;
        Mag.Amount = Amount;
        Mag.RoundSize = RoundSize;
        return Mag;
    }
    
    public override void InitializeCollectionMagazine(CollectionMagazineViewModel collectionMagazine) 
    {
    }
}
