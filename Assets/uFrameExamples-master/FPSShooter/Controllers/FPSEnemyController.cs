using System.Collections;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class FPSEnemyController : FPSEnemyControllerBase
{

    public override void InitializeFPSEnemy(FPSEnemyViewModel enemy)
    {
        enemy.Health = 30.0f;
        enemy.Speed = 5f;
    }
    
}
