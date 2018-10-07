using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514AudioSource : MonoBehaviour {

	[System.NonSerialized]public AudioSource src;
	bool randPitch = false,isLoop = false, bgm = false;

	public void Init(AudioClip clip,bool randPitch,bool loop){
		
		this.randPitch = randPitch;
		this.src = GetComponent<AudioSource>();
		this.src.clip = clip;
		this.src.pitch = 1f;
		this.src.volume = 0.7f;
		this.src.loop = isLoop = loop;
		this.bgm = bgm;
	}

	public void StartClip(){
		if(randPitch)
			src.pitch = Random.Range(0.95f,1.05f);
		src.Play();
		if(!bgm) StartCoroutine(LifeSpan());
	}

	public void SetBgm(bool b){
		bgm = b;
	}

	public void StopClip(){
		src.Stop();
		if(!bgm){
		}
		else{
			bgm = false;
			k514SoundManager.singleton.AddPooling(this);
		}
	}

	IEnumerator LifeSpan(){
		while(src.isPlaying){
			if(Time.timeScale == 0f){
				src.volume = 0f;				
				if(isLoop) this.src.loop = false;
			}else if(Time.timeScale != 0f){
				src.volume = k514SoundManager.singleton.SFXVolume;
				if(isLoop) this.src.loop = true;
			}

			yield return null;
		}
		k514SoundManager.singleton.AddPooling(this);
	}
}
