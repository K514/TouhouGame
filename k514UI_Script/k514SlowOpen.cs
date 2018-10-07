using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class k514SlowOpen : MonoBehaviour {

	public Image img;
	public Text txt;
	void Start(){
		StartCoroutine(Open());
	}

	IEnumerator Open(){
		
		float start = 0f, normalized = 0f,time = 1f, rtime = 1f/1f;
		
		while(start < time){
			start += Time.deltaTime;
			normalized = start*rtime;
			img.color = Color.Lerp(new Color(1f,1f,1f,0f),new Color(1f,1f,1f,1f),normalized);
			txt.color = Color.Lerp(new Color(1f,1f,1f,0f),new Color(1f,1f,1f,1f),normalized);
			yield return null;
		}
		float t = 0f,t2 = 6f;
		yield return new WaitForSeconds(1.5f);
		while(!Input.GetKey(KeyCode.X) && t < t2){
			t += Time.deltaTime;
			yield return null;
		}

		k514Loader.sceneName = "k514TitleScene";
		SceneManager.LoadScene("k514LoadingScene");
    }
}
