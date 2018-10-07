using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514EasterEgg : MonoBehaviour {

	public AudioClip[] serifu;
	public Sprite[] icon;
	private AudioSource aS = null;
	private Image img = null;
	private bool boolTrig = false;
	void Start(){
		aS = GetComponent<AudioSource>();
		img = GetComponent<Image>();
	}

	public void EasterVoice(){
		if(!boolTrig)
			StartCoroutine(Drop());
	}

	public void SetIcon(int index){
		img.sprite = icon[index];
	}

	IEnumerator Drop(){
		boolTrig = true;
		aS.clip = serifu[Random.Range(0,serifu.Length)];
		aS.Play();
		float interval = aS.clip.length;
		yield return new WaitForSeconds(interval);
		boolTrig = false;
	}

}
