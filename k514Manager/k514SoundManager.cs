using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX_TYPE_BULLET{
	SHOT1,SHOT2,SHOT3,BEAM,END
}

public enum SFX_TYPE_BODY{
	FOOTSTEP,JUMP,ROLLING,SWING,RUNNING,FLYING,DIVE,DIVEOUT,FURUFURU,PICHU,DASH,END
}

public enum SFX_TYPE_EFFECT{
	FIRE1,MAGIC1,KIRA,MUSOU_START,MUSOU_HIT,SPELL_ACTIVE,GRAZE,BOSS_RETIRE,WIND,END
}

public enum SFX_TYPE_ENEMY{
	ENEMY_HIT,ENEMY_DEAD,END
}

public enum SFX_TYPE_UI{
	UI1,UI2,UI3,PAPER,PAUSE,END
}

public enum SFX_TYPE_BGM{
	BGM0,BGM1,BGM2,BGM3,END
}


public class k514SoundManager : MonoBehaviour {

	public static k514SoundManager singleton = null;
	public k514AudioSource Pfab;
	public AudioClip[] clips_bullet,clips_body,clips_effect,clips_enemy,clips_item,clips_ui,clips_bgm;
	private List<k514AudioSource> audioClipPool;

    
    [System.NonSerialized]public float BGMVolume = 0.5f;
    [System.NonSerialized]public float SFXVolume = 0.5f;

	// Use this for initialization
	void Awake () {
		if(singleton == null){
			singleton = this;
		}
		else if(singleton != this){
			Destroy(gameObject);
		}
		audioClipPool = new List<k514AudioSource>();
	}


	public void ClearSfx(){
		for(int i = 0 ; i < transform.childCount ; i++){
			transform.GetChild(i).GetComponent<k514AudioSource>().StopClip();
		}
	}


	k514AudioSource CastPooling(AudioClip clip, bool RandomPlay, bool LoopPlay){
		k514AudioSource pooled = null;
		int lastIndex = audioClipPool.Count - 1;
		if(lastIndex < 0){
			pooled = Instantiate<k514AudioSource>(Pfab);
			pooled.transform.SetParent(transform);
			pooled.transform.name = "pooledAudioClip";
		}else{
			pooled = audioClipPool[lastIndex];
			audioClipPool.RemoveAt(lastIndex);
			pooled.gameObject.SetActive(true);
		}

		pooled.Init(clip,RandomPlay,LoopPlay);
		pooled.StartClip();

		return pooled;
	}

	k514AudioSource CastPoolingbgm(AudioClip clip, bool RandomPlay, bool LoopPlay){
		k514AudioSource pooled = null;
		int lastIndex = audioClipPool.Count - 1;
		if(lastIndex < 0){
			pooled = Instantiate<k514AudioSource>(Pfab);
			pooled.transform.SetParent(transform);
			pooled.transform.name = "pooledAudioClip";
		}else{
			pooled = audioClipPool[lastIndex];
			audioClipPool.RemoveAt(lastIndex);
			pooled.gameObject.SetActive(true);
		}

		pooled.Init(clip,RandomPlay,LoopPlay);
		pooled.SetBgm(true);
		pooled.StartClip();
		if(k514SystemManager.OptionMgr != null) k514SystemManager.OptionMgr.SetBGM(pooled);

		return pooled;
	}

	public void AddPooling(k514AudioSource src){
		src.gameObject.SetActive(false);
		audioClipPool.Add(src);
	}

	public k514AudioSource PlayAudioClip(SFX_TYPE_BULLET sfx, bool RandomPlay = false, bool LoopPlay = false){
		return CastPooling(clips_bullet[(int)sfx],RandomPlay,LoopPlay);
	}

	public k514AudioSource PlayAudioClip(SFX_TYPE_BODY sfx, bool RandomPlay = false, bool LoopPlay = false){
		return CastPooling(clips_body[(int)sfx],RandomPlay,LoopPlay);
	}

	public k514AudioSource PlayAudioClip(SFX_TYPE_EFFECT sfx, bool RandomPlay = false, bool LoopPlay = false){
		return CastPooling(clips_effect[(int)sfx],RandomPlay,LoopPlay);
	}

	public k514AudioSource PlayAudioClip(SFX_TYPE_ENEMY sfx, bool RandomPlay = false, bool LoopPlay = false){
		return CastPooling(clips_enemy[(int)sfx],RandomPlay,LoopPlay);
	}

	public k514AudioSource PlayAudioClip(ITEM_TYPE sfx, bool RandomPlay = false, bool LoopPlay = false){
		return CastPooling(clips_item[(int)sfx],RandomPlay,LoopPlay);
	}

	public k514AudioSource PlayAudioClip(SFX_TYPE_UI sfx, bool RandomPlay = false, bool LoopPlay = false){
		return CastPooling(clips_ui[(int)sfx],RandomPlay,LoopPlay);
	}

	public k514AudioSource PlayAudioClip(SFX_TYPE_BGM sfx, bool RandomPlay = false, bool LoopPlay = false){
		return CastPoolingbgm(clips_bgm[(int)sfx],RandomPlay,LoopPlay);
	}

}
