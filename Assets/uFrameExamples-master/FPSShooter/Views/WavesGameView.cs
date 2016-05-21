using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;


public partial class WavesGameView {

    public override void Bind()
    {
        base.Bind(); 
        UpdateAsObservable()
             .Subscribe(_ =>
             { }).DisposeWith(this.gameObject);
    }

    public override void Update()
    {
        if(WavesFPSGame.CurrentPlayer.Health <= 0)
        {
            WavesFPSGame.WavesState.StateMachine.SetState("GameOver");
            WavesFPSGame.CurrentPlayer.Magazine.Clear();
            //WavesFPSGame.CurrentWave = 1;
        }
    }

}
