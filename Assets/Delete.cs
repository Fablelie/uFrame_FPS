using UnityEngine;
using System.Collections;

public class Delete : MonoBehaviour {

    private MouseLook mouselook;
    private FPSPlayerView fpsPlayerView; 
    private CharacterMotor charcterMotor;
	// Use this for initialization
	void Start () 
    {
        charcterMotor = this.GetComponent<CharacterMotor>();
        mouselook = this.GetComponent<MouseLook>();
        fpsPlayerView = this.GetComponent<FPSPlayerView>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(fpsPlayerView.FPSPlayer.Health <= 0)
        {
            charcterMotor.canControl = false;
            mouselook.sensitivityX = 0;
            mouselook.sensitivityY = 0;
        }
	}
}
