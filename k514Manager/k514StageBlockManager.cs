using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514StageBlockManager : MonoBehaviour {

	// eventchecking
	public k514EventChecker EventChecker;
	public bool EventCheckTrig = false;
	private bool WaitEventStartKey = false;

	// for block rotate
	public static k514StageBlockManager singleton = null;
	public Light DirectionalLight;
	public Material Day,Night;
	private bool startBlockTrig = false;

	public Transform BackGroundTerrain, EventBox, moon;
	public k514StageBlock[] blocks;
	public k514EnemySpawner[] spawner;

	[System.NonSerialized]public int nowBlockCount = 0;

	void Awake(){
		if(singleton == null){
			singleton = this;
		}else if(singleton != this){
			Destroy(gameObject);
		}
	}

	void Start(){
		EventChecker.gameObject.SetActive(false);
		
		for(int i = 0 ; i < 4 ; i++){
			k514EnemySpawner[] tmp = blocks[i].transform.parent.GetComponentsInChildren<k514EnemySpawner>();
			spawner[i] = tmp[0];
			spawner[i+4] = tmp[1];			
		}
		
		MakeDay();
	}

	public void PlayBgm(SFX_TYPE_BGM s){
		k514SystemManager.SoundMgr.PlayAudioClip(s,false,true);
	}

	void PlayFirstBgm(){
		PlayBgm(SFX_TYPE_BGM.BGM1);
		k514SystemManager.UI_SpecaMgr.PumpBgmName(0);
	}

	void Update () {
		BackGroundTerrain.position = Vector3.Lerp(BackGroundTerrain.position,new Vector3(-360f,-10f,k514SystemManager.InteractMgr.GetPlayerVector().z),Time.deltaTime);
		
		for(int i = 0 ; i < blocks.Length ; i++){

			if(blocks[i].GetState()){
				blocks[i].SetState(false);
				spawner[i].outter_spwCondition = true;
				spawner[i+4].outter_spwCondition = true;
				
				// 최초 1회에 한해 0번 블록은 움직이지 않도록 한다.
				if(i == 0 && !startBlockTrig){
					Invoke("PlayFirstBgm",2f);
					startBlockTrig = true;
					return;
				}

				blocks[nowBlockCount%blocks.Length].transform.parent.position += Vector3.forward*100*blocks.Length;
				spawner[nowBlockCount%blocks.Length].Init();
				spawner[nowBlockCount%blocks.Length+4].Init();				
				nowBlockCount ++ ;
				if(nowBlockCount<7){
					MakeDarker(0.13f);
				}else if(nowBlockCount == 7){
					EventCheckTrig = true;
					DirectionalLight.intensity =  0.2f;
				}
			}
		}		

		if(EventCheckTrig){
			EventChecker.gameObject.SetActive(true);
			WaitEventStartKey = true;
			EventCheckTrig = false;
		}

		if(WaitEventStartKey){
			if(!EventChecker.Progress()){
				return;
			}
	        k514SystemManager.SoundMgr.ClearSfx();
			StartCoroutine(k514SystemManager.UI_SpecaMgr.UI_Slide_out(3f));
			StartCoroutine(k514SystemManager.UI_SpecaMgr.UI_Black_out(1f,2f));
			EventChecker.EndProcess();
			StartCoroutine(EventBox.GetComponent<k514RumiaEncounterEvent>().RumiaStageOn(4f));
		}



	}

	void MakeDarker(float f){
		DirectionalLight.intensity -=  f;
		if(DirectionalLight.intensity < 0.2f){
			DirectionalLight.intensity =  0.2f;
			MakeNight();			
		}else{
			MakeDay();
		}
	}

	public void MakeNight(){
		RenderSettings.skybox=Night;
		moon.gameObject.SetActive(true);
	}

	public void MakeDay(){
		RenderSettings.skybox=Day;
	}

}
