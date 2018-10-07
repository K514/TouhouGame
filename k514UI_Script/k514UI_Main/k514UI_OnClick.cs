using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class k514UI_OnClick : MonoBehaviour, IPointerClickHandler {

	public Transform UI_Main_Panel,p2,p3;
	private RectTransform UI_Main_Panel_RectTransform = null;
	public Image UI_Main_TitleImage,UI_Main_TitleImage_Mask;
	private bool OnceTrig = false;
	public bool fa = false, animate = false, key = false, menu = false;
	public Text[] t;
	public Image[] im;	
	int step = 0;
	void Start()
	{
			UI_Main_Panel_RectTransform = UI_Main_Panel.GetComponent<RectTransform>();
			if(fa){ 
				StartCoroutine(drip());
				animate = true;
			}else{
				StartCoroutine(Open());
			}
	}

	IEnumerator Open(){
        yield return new WaitForSeconds(1f);
		
		float start = 0f, normalized = 0f,time = 2f, rtime = 1f/2f;
		
		while(start < time){
			start += Time.deltaTime;
			normalized = start*rtime;
			for(int i = 0 ; i < t.Length ; i++){
				im[i].color = Color.Lerp(new Color(1f,1f,1f,0f),new Color(1f,1f,1f,1f),normalized);
				t[i].color = Color.Lerp(new Color(t[i].color.r,t[i].color.g,t[i].color.b,0f),new Color(t[i].color.r,t[i].color.g,t[i].color.b,1f),normalized);
			}
			yield return null;
		}
        yield return new WaitForSeconds(0.5f);
		key = true;
    }

	// Update is called once per frame
  	public void OnPointerClick(PointerEventData eventData)
    {
		DoAct();
	}

	void Update(){
		if(Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Escape))
		{
			if(!menu)
				DoAct();
			else if(Input.GetKey(KeyCode.X)){
				GameObject.Find("EventSystem").GetComponent<k514UI_Main>().OnMenuButtonClicked();
			}else if(Input.GetKey(KeyCode.Escape)){
				GameObject.Find("EventSystem").GetComponent<k514UI_Main>().OnMenuButtonClickedE();
			}
		}
			
		
	}

	void DoAct(){
		if(!fa&&!key) return;
		if((!fa)&&OnceTrig) return;
		OnceTrig = true;
		
		if(!fa){
			UI_Main_Panel.gameObject.SetActive(true);
			StartCoroutine(MenuPanelAnimating(new Vector3(400,-62,0) , 0.2f ));
		}else{
			if(!animate){
				animate = true;
				step ++;
				Debug.Log(3);
				switch(step){
					case 1:
					StartCoroutine(drink());					
					break;
					case 2:
					StartCoroutine(draw());
					break;
				}
			}
		}
	}

	IEnumerator MenuPanelAnimating(Vector3 InitPos,float moveTime)
	{
		k514SoundManager.singleton.PlayAudioClip(SFX_TYPE_UI.UI2);
		Vector3 home = UI_Main_Panel_RectTransform.anchoredPosition;
		float start = 0f , reversedMoveTime = 1f/moveTime , normalized = 0f;
		while(start < moveTime){
			start += Time.deltaTime;
			normalized = start*reversedMoveTime;
			UI_Main_TitleImage_Mask.color = new Color(1f - 2*start,1f - 2*start,1f - 2*start,1f);
			UI_Main_TitleImage_Mask.fillAmount = 0.5f*normalized;
			UI_Main_Panel_RectTransform.anchoredPosition = Vector3.Lerp(InitPos,home,normalized);
			yield return null;
		}
		menu = true;
	}

    IEnumerator drip(){
        yield return new WaitForSeconds(3f);
		k514SoundManager.singleton.PlayAudioClip(SFX_TYPE_BGM.BGM3,false,true);
        yield return new WaitForSeconds(1f);		
		Image img = UI_Main_Panel.GetComponent<Image>();
		
		float start = 0f, normalized = 0f,time = 5f, rtime = 1f/5f;
		
		while(start < time){
			start += Time.deltaTime;
			normalized = start*rtime;
			img.color = Color.Lerp(new Color(1f,1f,1f,0f),new Color(1f,1f,1f,1f),normalized);
			yield return null;
		}
		animate = false;
		Debug.Log(2);    
	}


    IEnumerator drink(){
		Debug.Log(1);
        yield return new WaitForSeconds(1f);
		p3.gameObject.SetActive(true);
		k514SoundManager.singleton.PlayAudioClip(SFX_TYPE_UI.PAPER);
		Text img = p3.GetComponent<Text>();
		
		float start = 0f, normalized = 0f,time = 3f, rtime = 1f/3f;
		
		while(start < time){
			start += Time.deltaTime;
			normalized = start*rtime;
			img.color = Color.Lerp(new Color(0f,0f,0f,0f),new Color(0f,0f,0f,1f),normalized);
			yield return null;
		}
		animate = false;
    }

    IEnumerator draw(){
        yield return new WaitForSeconds(1f);
        p2.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
		while(true){
			yield return null;
			if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X)) break;
		}
		k514Loader.sceneName = "k514TitleScene";
		SceneManager.LoadScene("k514LoadingScene");
    }
}
