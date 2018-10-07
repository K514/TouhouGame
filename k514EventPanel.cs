using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514EventPanel : MonoBehaviour {

	Image talkBoard,portrait,nameSpace;
	Text serifu,nick,name;
	Vector3 home;
	RectTransform rectTransform;
	WaitForSeconds wait = new WaitForSeconds(0.1f);

	bool nameShowOnce = false;
	bool eventEnd = false;

	// Use this for initialization
	void Awake () {

		rectTransform = GetComponent<RectTransform>();
		home = rectTransform.anchoredPosition;

		talkBoard = GetComponent<Image>();
		portrait = GetComponentsInChildren<Image>()[1];
		nameSpace = GetComponentsInChildren<Image>()[2];
		serifu = GetComponentsInChildren<Text>()[0];	
		nick = GetComponentsInChildren<Text>()[1];		
		name = GetComponentsInChildren<Text>()[2];					
		portrait.enabled = false;
		nameSpace.enabled = false;		

		Sprite sprite = Resources.Load<Sprite>("3");
	}

	public void SetPortrait(Sprite sprite,Vector3 scale){
		portrait.enabled = true;
		portrait.transform.localScale = scale;
		portrait.sprite = sprite;		
	}

	public void SetFade(int span = 5){
		transform.parent.GetComponent<Canvas>().sortingOrder = 0;
		StartCoroutine(fadeOut(span));
	}

	public void DeFade(int span = 5){
		transform.parent.GetComponent<Canvas>().sortingOrder = 1;
		StartCoroutine(fadeOut(span,1));
	}

	public void animating(Vector3 target,float moveSpeed, float moveTime = 1f){
		StartCoroutine(animateTalkingBoard(target,moveSpeed,moveTime));		
	}

	public void kill(Vector3 target,float moveSpeed, float moveTime = 1f){
		StartCoroutine(animateBoardKill(target,moveSpeed,moveTime));		
	}


	public void SetSerifu(string serifu){
		this.serifu.text = serifu;
	}

	public void nameShowEvent(string nick,string name,float time = 1f){
		if(nameShowOnce) return;
		nameShowOnce= true;
		StartCoroutine(faceNameSpace(nick,name,time));		
	}

	public void screenShakeEvent(int cnt, float interval){
		StartCoroutine(screenShake(cnt,interval));
	}



	IEnumerator fadeOut(int span,int direction = -1){
		float target = (direction==-1) ? 1f : 0.2f;
		float offset = 1f/span;
		eventEnd = true;
		for(int i = 0 ; i < span ; i++){
			talkBoard.color = portrait.color = serifu.color = new Color(1f,1f,1f,target+direction*i*offset);
			yield return wait;
		}
		eventEnd = false;		
	}

	IEnumerator faceNameSpace(string lnick,string lname,float time){
		time *= 0.2f;
		nameSpace.enabled = true;
		nick.text = lnick;
		name.text = lname;	

		float start = 0f, reversedMoveTime = 1f/time , normalized = 0f;
		while(start < time){
			start += Time.deltaTime;
			nameSpace.color = nick.color = name.color = new Color(1f,1f,1f,start*reversedMoveTime);
			yield return null;
		}
		yield return new WaitForSeconds(time*2f);

		start = 0f;
		while(start < time){
			start += Time.deltaTime;
			nameSpace.color = nick.color = name.color = new Color(1f,1f,1f,1f - start*reversedMoveTime);
			yield return null;
		}
		nameSpace.enabled = false;		
	}	

	IEnumerator screenShake(int cnt,float interval){

		for(int i = 0 ; i < cnt ; i++){
			// while(eventEnd){
			// 	yield return null;
			// }
			rectTransform.anchoredPosition = (i%2==0) ? home + Vector3.left*18f : home + Vector3.left*-18f;
			yield return new WaitForSeconds(interval);
		}
		rectTransform.anchoredPosition = home;
	}

	IEnumerator animateTalkingBoard(Vector3 ltarget, float moveSpeed,float moveTime){
		Vector3 target = home+ltarget;
		moveTime *= 0.5f;
		float start = 0f,reversedMoveTime = 1f/moveTime,normalized = 0f;
		while(start < moveTime){
			start += Time.deltaTime;
			normalized = start*reversedMoveTime;
			rectTransform.anchoredPosition = Vector3.Lerp(home,target,moveSpeed*normalized);
			yield return null;
		}
		start = 0f;
		while(start < moveTime){
			start += Time.deltaTime;
			normalized = start*reversedMoveTime;
			rectTransform.anchoredPosition = Vector3.Lerp(target,home,moveSpeed*normalized);
			yield return null;
		}
	}

	IEnumerator animateBoardKill(Vector3 ltarget, float moveSpeed,float moveTime){
		Vector3 target = home+ltarget;
		moveTime *= 0.5f;
		float start = 0f,reversedMoveTime = 1f/moveTime,normalized = 0f;
		while(start < moveTime){
			start += Time.deltaTime;
			normalized = start*reversedMoveTime;
			rectTransform.anchoredPosition = Vector3.Lerp(home,target,moveSpeed*normalized);
			yield return null;
		}
		Destroy(gameObject);
	}



}
