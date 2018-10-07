using System;
// using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514SerifuManager : MonoBehaviour {

	public static k514SerifuManager singleton = null;
	public Transform[] Pfab = null;

	private string[] Nick = {"낙원의 무녀","밤하늘의 요괴"};
	private string[] Actor = {"레이무","루미아"};
	private k514EventPanel[] target = null;
	private CharacterStatus[] charaInfo = null;
	private List<SerifuContainer> serifuList = null;
	private bool EndTrig = false, showNameTrig = true;

	// Use this for initialization
	void Start () {
		if(singleton == null) singleton = this;
		else if(this != singleton) Destroy(gameObject);

		target = new k514EventPanel[Pfab.Length];
		charaInfo = new CharacterStatus[Pfab.Length];
		serifuList = new List<SerifuContainer>();
		// DontDestroyOnLoad(this);
	}

	void Init(){
		GenerateTalkBar();
		SetText("TestScript");
		StartCoroutine(LifeCycle());
	}

	void Init2(){
		GenerateTalkBar();
		SetText("TestScript2");
		StartCoroutine(LifeCycle());
	}

	void Init3(){
		GenerateTalkBar();
		SetText("TestScript6");
		StartCoroutine(LifeCycle());
	}

	void Init4(){
		GenerateTalkBar();
		SetText("TestScript4");
		StartCoroutine(LifeCycle());
	}

	public void Act(bool s = true){
		showNameTrig = s;
		Init();
	}

	public void Act2(bool s = true){
		showNameTrig = s;
		Init2();
	}

	public void Act3(bool s = true){
		showNameTrig = s;
		Init3();
	}

	public void Act4(bool s = true){
		showNameTrig = s;
		Init4();
	}

	public bool isEnd(){
		return EndTrig;
	}

	public void EndProcess(){
		serifuList.Clear();
		EndTrig = false;
	}

	IEnumerator LifeCycle(){
		SerifuContainer serif_target = null,current = null;
		int step = serifuList.Count,turnTrig = -1;
		for(int i = 0 ; i < step ; i++){
			serif_target = serifuList[i];

			if(current==null){
				for(int j = 0 ; j < charaInfo.Length ; j++){
					if(serif_target.id != charaInfo[j].id) target[charaInfo[j].id].SetFade();
				}
			}

			if(current!=null && current.id != serif_target.id){
				target[current.id].SetFade();
				target[serif_target.id].DeFade();
				target[serif_target.id].animating(new Vector3(turnTrig*40,20,0),5f,0.5f);
				turnTrig *= -1;
			}

			current = serif_target;
			if(showNameTrig) target[serif_target.id].nameShowEvent(Nick[serif_target.id],Actor[serif_target.id],8f);
			target[serif_target.id].SetPortrait(charaInfo[serif_target.id].getFace(serif_target.e) , serif_target.scale );
			target[serif_target.id].SetSerifu(serif_target.serifu);
				
				switch(serif_target.talk_event){
					case 1 :
                    	k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.FURUFURU,false);
						target[serif_target.id].screenShakeEvent(10,0.03f);
					break;

					default :
                    	k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_UI.PAPER,false);
					break;
				}

			yield return new WaitForSeconds(0.3f);

			//busy waiting;
			while(true){
				yield return null;
				if(Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.X) && Time.timeScale != 0f) break;
			}
		}

		target[0].SetFade();
		target[1].SetFade();
		target[0].kill(new Vector3(turnTrig*1200,-750,0),3f,2f);
		target[1].kill(new Vector3(-turnTrig*1200,-750,0),3f,2f);
		yield return new WaitForSeconds(0.5f);
		EndTrig = true;
	}

	void GenerateTalkBar(){
		Transform tmp;
		for(byte i = 0 ; i < Pfab.Length ; i++){
			tmp = Instantiate(Pfab[i]);
			target[i] = tmp.GetComponentInChildren<k514EventPanel>();
			tmp.parent = transform;
			charaInfo[i] = new CharacterStatus(i,Actor[i]);
			charaInfo[i].LoadResource();
		}
	}
	
	public void SetText (string fileName) {
			string line;
			string fullText = Resources.Load<TextAsset>("Text/"+fileName).text;
			string[] lines = Regex.Split ( fullText, "\r\n" );
			// \n|\r|\r\n 정규식을 사용해도 되는 플랫폼이 있고, 아닌 플랫폼이 있는 듯
			
			for(int i = 0 ; i < lines.Length ; i++){
					line = lines[i];
					if (line != null)
					{
						if(line[0]=='/'){
							continue;
						}

						string[] entries = Regex.Split(line,",");
						if (entries.Length > 0){
							serifuList.Add(new SerifuContainer(Byte.Parse(entries[0]) , Byte.Parse(entries[1]) , entries[2], float.Parse(entries[3]), float.Parse(entries[4]), Byte.Parse(entries[5]) ));
						}
					}
			}

	}



	public class CharacterStatus{
		public enum EMOTION{
			ANGRY,SAD,BORING,TIRED,HAPPY,SMILE,END
		}

		public string NAME = null;
		public byte id;
		EMOTION Default = EMOTION.SMILE;
		Sprite[] face;
		byte[] targetEmotion;

		public CharacterStatus(byte id,string NAME){
			this.id = id;
			this.NAME = NAME;
			targetEmotion = new byte[(int)EMOTION.END];
			for(byte i = 0 ; i < targetEmotion.Length ; i++){
				targetEmotion[i] = i;
			}
		}

		void SetDefaultEmotion(EMOTION e){
			Default = e;
		}

		void SetAtoB(EMOTION a,EMOTION b){
			targetEmotion[(int)a] = (byte)b;
		}

		void SetDefault(EMOTION e){
			targetEmotion[(int)e] = (byte)Default;			
		}

		string getPicName(byte e){
			StringBuilder s = new StringBuilder("sprites/");
			s.Append(NAME);
			s.Append("_");
			switch(targetEmotion[e]){
				case 0 :
					s.Append("angry");
					break;
				case 1 :
					s.Append("sad");
					break;
				case 2 :
					s.Append("boring");
					break;
				case 3 :
					s.Append("tired");
					break;
				case 4 :
					s.Append("happy");
					break;
				case 5 :
					s.Append("smile");
					break;	
			}
			return s.ToString();
		}

		byte findLength(){
			byte result = 0;
			for(byte i = 0 ; i < targetEmotion.Length ; i++){
				if(targetEmotion[i]==i) result+=1;
			}
			return result;
		}

		public void LoadResource(){
			face = new Sprite[findLength()];
			for(byte i = 0 ; i < targetEmotion.Length ; i++){
				if(i==targetEmotion[i]) face[i] = Resources.Load<Sprite>(getPicName(i));
			}
		}

		public Sprite getFace(byte i){
			return face[targetEmotion[i]];
		}

	}

	public class SerifuContainer{
		public byte id;
		public byte e;
		public string serifu;
		public byte talk_event;
		public Vector3 scale;

		public SerifuContainer(byte id,byte e,string serifu,float x, float y,byte talk_event){
				this.id = id;
				this.e = e;
				this.serifu = serifu;
				this.talk_event = talk_event;
				scale = new Vector3(x,y,0);
		}
	}

}
