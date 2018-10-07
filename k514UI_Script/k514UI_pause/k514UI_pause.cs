using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class k514UI_pause : MonoBehaviour {

    public AudioClip[] ac;
    private AudioSource aS = null;
    public Slider s1,s2;
    private bool b1 = true,b2 = true;
    public Transform img1,img2;

    void OnEnable(){
        aS = transform.GetComponent<AudioSource>();
        aS.volume = 0.5f;        
        aS.clip = ac[0];
        aS.Play();
        if(k514SystemManager.SoundMgr != null){
            if(b1) s1.value = k514SystemManager.SoundMgr.BGMVolume;
            if(b2) s2.value = k514SystemManager.SoundMgr.SFXVolume;            
        }
    }

    void OnDisable(){
        if(aS.isPlaying) aS.Stop();
    }

    public void Retry(){
		k514Loader.sceneName = "k514Stage1";
		SceneManager.LoadScene("k514LoadingScene");
    }

    public void Title(){
        k514Loader.sceneName = "k514TitleScene";
		SceneManager.LoadScene("k514LoadingScene");
    }

    public void OnBGMChanged(){
        k514SystemManager.SoundMgr.BGMVolume = aS.volume = s1.value; 
        aS.clip = ac[1];
        aS.Play(); 
    }
     
    public void OnSFXChanged(){
        k514SystemManager.SoundMgr.SFXVolume = aS.volume = s2.value; 
        aS.clip = ac[2];
        aS.Play(); 
    }

    public void OnBgmMuteClicked(){
        b1 = !b1;
        img1.gameObject.SetActive(!b1);
        s1.enabled = b1;
        if(b1){
            k514SystemManager.SoundMgr.BGMVolume = aS.volume = s1.value;
        }else{
            k514SystemManager.SoundMgr.BGMVolume = aS.volume = 0f;            
        }
    }

    public void OnSfxMuteClicked(){
        b2 = !b2;
        img2.gameObject.SetActive(!b2);
        s2.enabled = b2;
        if(b2){
            k514SystemManager.SoundMgr.SFXVolume = aS.volume = s2.value;
        }else{
            k514SystemManager.SoundMgr.SFXVolume = aS.volume = 0f;            
        }
    }
}
