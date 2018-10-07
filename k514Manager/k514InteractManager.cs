using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514InteractManager : MonoBehaviour {

    public static k514InteractManager singleton = null;
	public uThirdPersonCamera camera;
    private Transform player = null;
    private Transform playerHit = null;	
    private k514CrossHair crosshair_target = null;
	private k514CrossHairAnimate crosshair_target_anim = null;
	

    void Awake(){
		if(singleton == null){
			singleton = this;
		}
		else if(singleton != this) Destroy(gameObject);
    }

	void Start(){
		player = GameObject.Find("Player").transform;
		playerHit = GameObject.Find("PlayerHit").transform;
		crosshair_target = player.GetComponentInChildren<k514CrossHair>();
		crosshair_target_anim = player.GetComponentInChildren<k514CrossHairAnimate>();
	}

	public Vector3 GetCameraLookVector(){
		return camera.currentPos.position;
	}

	public Vector3 GetCameraCurrentLookVector(){
		return camera.transform.forward;
	}

	public uThirdPersonCamera GetCamera(){
		return camera;
	}

	public Vector3 GetPlayerVector(){
		return player.position;
	}

	public Vector3 GetPlayerVectorFrom(Vector3 from){
		return (player.position - from).normalized;
	}

	public Transform GetPlayerTransform(){
		return player;
	}

	public Vector3 GetPlayerHitVector(){
		return playerHit.position;
	}

	public Vector3 GetPlayerHitVectorFrom(Vector3 from){
		return (playerHit.position - from).normalized;
	}
	
	public Vector3 GetPlayerVectorXZFrom(Vector3 from){
		return Vector3.Scale(new Vector3(1,0,1),(player.position - from)).normalized;
	}

	public k514CrossHair GetCrossHair(){
		return crosshair_target;
	}

	public Transform GetPlayerHitTransform(){
		return playerHit;
	}

	public Vector3 GetCrossHairTargetVector(){
		return crosshair_target.targetPlace;
	}

	public Transform GetCrossHairTargetTransform(){
		return crosshair_target.targetTransform;
	}

	public bool GetTargetDetectedTrig(){
		return crosshair_target_anim.targetDetected;
	}

	public void SetTargetDetectedTrig(bool trig){
		crosshair_target_anim.targetDetected = trig;
	}
}