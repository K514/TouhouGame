using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514SystemManager : MonoBehaviour {

	public static k514BulletManager BulletMgr;
	public static k514MathManager MathMgr;
	public static k514InteractManager InteractMgr;
	public static k514EnemyManager EnemyMgr;
	public static k514StageBlockManager BlockMgr;
	public static k514ItemManager ItemMgr;
	public static k514GamePlayManager GameMgr;
	public static k514UISpellCardManager UI_SpecaMgr;
	public static k514SerifuManager SerifuMgr;
	public static k514SoundManager SoundMgr;
	public static k514EffectManager EffectMgr;
	public static k514OptionManager OptionMgr;
	
	public Transform snd;

	void Start(){
			BulletMgr = k514BulletManager.singleton;
			MathMgr = k514MathManager.singleton;
			InteractMgr = k514InteractManager.singleton;
			EnemyMgr = k514EnemyManager.singleton;
			BlockMgr = k514StageBlockManager.singleton;
			ItemMgr = k514ItemManager.singleton;
			GameMgr = k514GamePlayManager.singleton;
			UI_SpecaMgr = k514UISpellCardManager.singleton;
			SerifuMgr = k514SerifuManager.singleton;
			SoundMgr = k514SoundManager.singleton;
			EffectMgr = k514EffectManager.singleton;
			OptionMgr = k514OptionManager.singleton;
			if(SoundMgr == null){
				Instantiate(snd).parent = transform;
				SoundMgr = k514SoundManager.singleton;
			}
	}

}
