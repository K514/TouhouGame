using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514PlayerMotionSound : MonoBehaviour {
	public void RunningSound(){
		k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.RUNNING,true);
	}

	public void RollingSound(){
		k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.ROLLING,true);
	}

	public void FootStepSound(){
		k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.FOOTSTEP,true);
	}

	public void ArmSwingSound(){
		k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.SWING,true);
	}
}
