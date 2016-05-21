using System;
using System.Collections;
using Invert.StateMachine;
using UniRx;
using UnityEngine;

public class WavesFPSGameController : WavesFPSGameControllerBase
{
    public WavesFPSGameViewModel WavesGame
    {
        get { return FPSGame as WavesFPSGameViewModel; }
    }
    
    public override void Spawn(WavesFPSGameViewModel wavesFPSGame)
    {
        base.Spawn(wavesFPSGame);
        
        if (WavesGame.EnemiesSpawned < wavesFPSGame.KillsToNextWave)
        {
            //Debug.Log("Spawn" + wavesFPSGame.KillsToNextWave);
            SpawnEnemy();
            WavesGame.EnemiesSpawned++;
        }
    }

    public override void NextWaveReady(WavesFPSGameViewModel wavesFPSGame)
    {
        base.NextWaveReady(wavesFPSGame);
        WavesGame.WaveKills = 0;
        WavesGame.CurrentWave++;
        WavesGame.KillsToNextWave += _NumberOfEnemiesMultiplier;
        WavesGame.SpawnWaitSeconds = Mathf.RoundToInt(WavesGame.SpawnWaitSeconds * _SpawnWaitMultiplier);
        WavesGame.EnemiesSpawned = 0;
    }
    
    public override void EnemyDied(FPSEnemyViewModel enemy)
    {
        base.EnemyDied(enemy);
        WavesGame.WaveKills++;
    }

    private bool FristTime = true;
    
    public override void InitializeWavesFPSGame(WavesFPSGameViewModel wavesFPSGame)
    {
        Debug.Log("Waves Game Initialized");
        LocalPlayer.DiedAsObservable.Subscribe(wavesFPSGame.PlayerDied).DisposeWith(wavesFPSGame);

        wavesFPSGame.WavesState.StateMachine.SetState("Wave");



        // Bind the local players death to the command
        //wavesFPSGame.CurrentPlayerProperty.First().Subscribe(player => player
        //        .DiedAsObservable.Subscribe(wavesFPSGame.PlayerDied)
        //        .DisposeWith(wavesFPSGame)
        //    );
        wavesFPSGame.WavesStateProperty
            .Subscribe(s => StateChanged(wavesFPSGame, s))
            .DisposeWith(wavesFPSGame);
        
        ExecuteCommand(wavesFPSGame.NextWaveReady);
    }

    private void StateChanged(WavesFPSGameViewModel wavesFPSGame, State state)
    {
        if (state is Wave)
        {
            Observable.Interval(TimeSpan.FromSeconds(wavesFPSGame.SpawnWaitSeconds))
                .Take(wavesFPSGame.KillsToNextWave)
                .Select(_ => Unit.Default)
                .Subscribe(wavesFPSGame.Spawn); 
        }
        else if (state is WaitForNextWave)
        {
            Observable.Timer(TimeSpan.FromSeconds(3))
                .Subscribe(_ => ExecuteCommand(wavesFPSGame.NextWaveReady));

        }
        else if (state is GameOver)
        {
            _NumberOfEnemiesAtStart = 2;
            _NumberOfEnemiesMultiplier = 2;
            _SpawnWait = 5f;
            _SpawnWaitMultiplier = 0.9f;
            //WavesGame.CurrentWave = 1;
            wavesFPSGame.Enemies.Clear();
        }
        
    }
}