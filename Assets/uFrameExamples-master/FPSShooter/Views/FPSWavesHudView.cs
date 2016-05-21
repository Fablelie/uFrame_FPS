
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public partial class FPSWavesHudView
{

    /// Invokes MainMenuExecuted when the MainMenu command is executed.
    public override void MainMenuExecuted() {
        base.MainMenuExecuted();
    }
 

    /// Invokes RetryExecuted when the Retry command is executed.
    public override void RetryExecuted() {
        base.RetryExecuted();
    }
 
    public Text _HealthText;
    public Text _KillsText;
    public Text _WaveText;
    public Text _WaveStartText;
    public Text _TotalKill;
    public Text _TotalWavetext;
    public GameObject DestroyAll;
    private int score;

    /// Subscribes to the state machine property and executes a method for each state.
    public override void WavesStateChanged(Invert.StateMachine.State value) {
        base.WavesStateChanged(value);
    }
    
    public override void OnWave() {
        base.OnWave();
        //_TotalKill.gameObject.SetActive(false);
        //_TotalWavetext.gameObject.SetActive(false);
    }
    
    public override void OnGameOver() {
        base.OnGameOver();
    }
    
    public override void OnWaitForNextWave() {
        base.OnWaitForNextWave();
    }

    //public override void KillsChanged(int value)
    //{
    //    _TotalKillsLabel.text = string.Format("Kills: {0}", value);
    //}


    public void Update()
    {
        score = WavesFPSGame.Kills;
        PlayerPrefs.SetInt("Total Score", score);
        PlayerPrefs.SetInt("Total Waves", WavesFPSGame.CurrentWave);

        if(WavesFPSGame.WavesState.ToString() == "GameOver")
        {
            string s = "Total Kills : " + PlayerPrefs.GetInt("Total Score").ToString();
            _TotalKill.text = s;
            string w = "Total Waves : " + PlayerPrefs.GetInt("Total Waves").ToString();
            _TotalWavetext.text = w;
            _TotalKill.gameObject.SetActive(true);
            _TotalWavetext.gameObject.SetActive(true);
        }
        else
        {
            _TotalKill.gameObject.SetActive(false);
            _TotalWavetext.gameObject.SetActive(false);
        }

        //WavesFPSGame
        //_TotalKill.text = string.Format("Total Kills : {0}", score);
    }

    //private FPSShooterGamePlay FPSshooterGamePlay; // look down, this use in OnGUI

    public void OnGUI()
    {
        if (WavesFPSGame.WavesState.ToString() == "GameOver")
        {
            if (GUI.Button(new Rect((Screen.width / 2f) - 75, (Screen.height / 2f) - 15, 150, 30), "Exit!"))
            {
                //WavesFPSGame.WavesState = FPSshooterGamePlay.Wave;
                //Application.Quit();
                //ExecuteMainMenu();
                WavesFPSGame.CurrentPlayer.Health = 100;
                WavesFPSGame.CurrentWave = 1;
                
                //Destroy(DestroyAll);
                Application.LoadLevel(0);
            }
        }
    }

    public override void WaveKillsChanged(int kills)
    {
        _KillsText.text = string.Format("Kills: {0}", kills);
    }

    public override void CurrentWaveChanged(int wave)
    {
        if(WavesFPSGame.WavesState.ToString() != "GameOver")
        {
            _WaveStartText.gameObject.SetActive(true);
            _WaveStartText.text = string.Format("Wave {0} started!", wave);
            StartCoroutine(HideLabelInSeconds());
        }
        _WaveText.text = string.Format("Wave: {0}", wave);
        WaveKillsChanged(WavesFPSGame.WaveKills);
    }

    public override void KillsToNextWaveChanged(int value)
    {
        base.KillsToNextWaveChanged(value);
        CurrentWaveChanged(WavesFPSGame.CurrentWave);
    }

    public IEnumerator HideLabelInSeconds(float seconds = 3f)
    {
        yield return new WaitForSeconds(seconds);
        _WaveStartText.gameObject.SetActive(false);
    }
}
