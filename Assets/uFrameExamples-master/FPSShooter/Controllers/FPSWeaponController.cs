using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Invert.StateMachine;
using UniRx;
using UnityEngine;

public class FPSWeaponController : FPSWeaponControllerBase
{
    
    //public FPSPlayerViewModel oo;
    public FPSWeaponViewModel CreateFPSWeapon(string identifier, WeaponType fpsWeaponType)
    {
        var weapon = CreateFPSWeapon();
        weapon.Identifier = identifier;
        weapon.WeaponType = fpsWeaponType;
        return weapon;
    }

    public override void NextZoom(FPSWeaponViewModel fpsWeapon)
    {
        if (fpsWeapon.MaxZooms - 1 == fpsWeapon.ZoomIndex)
        {
            fpsWeapon.ZoomIndex = 0;
        }
        else
        {
            fpsWeapon.ZoomIndex++;
        }
    }

    //private void Reloading(FPSWeaponViewModel fpsWeapon, State state)
    //{
    //    if (state is Reloading && fpsWeapon.Magazine.Count > 0)
    //    {
    //        Debug.Log("fdfdfdfdfdfdfdfd");
    //        fpsWeapon.Ammo = fpsWeapon.Magazine[0].Amount;
    //        ExecuteCommand(fpsWeapon.FinishedReloading);
    //        fpsWeapon.Magazine.RemoveAt(0);
    //    }
    //}

    //public override void AddMagazine(FPSWeaponViewModel fPSWeapon)
    //{
    //    base.AddMagazine(fPSWeapon);
    //    //oo = fPSWeapon.ParentFPSPlayer;
    //    //oo.CollectedMag++;
    //}

    public override void InitializeFPSWeapon(FPSWeaponViewModel fpsWeapon)
    {
        //var enemy = FPSEnemyController.CreateFPSEnemy();
        //enemy.Identifier = Guid.NewGuid().ToString();
        //enemy.HealthStateProperty.Where(p => p == FPSPlayerState.Dead)
        //    .Subscribe(p => EnemyDied(enemy)).DisposeWith(enemy);
        //FPSGame.Enemies.Add(enemy);
        //fpsWeapon.StateProperty.Subscribe(state => Reloading(fpsWeapon, state));
        //fpsWeapon.StateProperty
        //    .Where(p => p is Reloading && fpsWeapon.Magazine.Count > 0) // Subscribe only to when the state is changed to reloading
        //    .Subscribe(r =>
        //    {
        //        // When we've entered the reloading state create a timer that moves it to finished reloading
        //        Observable.Timer(TimeSpan.FromSeconds(fpsWeapon.ReloadTime))
        //            .Subscribe(l =>
        //            {
        //                Debug.Log("dsdsdsdsdsd");
        //                fpsWeapon.Ammo = fpsWeapon.Magazine[0].Amount;
        //                    //fpsWeapon.RoundSize;
        //                ExecuteCommand(fpsWeapon.FinishedReloading);
        //                fpsWeapon.Magazine.RemoveAt(0);
        //            });
                
        //    }).DisposeWith(fpsWeapon); // Make sure this is disposed with the weapon
        //ExecuteCommand(fpsWeapon.FinishedReloading);
    }

    public override void BulletFired(FPSWeaponViewModel fPSWeapon)
    {
        base.BulletFired(fPSWeapon);
        fPSWeapon.Ammo -= 1;
    }

    //public override void Reload(FPSWeaponViewModel fPSWeapon)
    //{
    //    base.Reload(fPSWeapon);
    //}

}