using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum MENU_BUTTON{
	START,d,END
}

public class k514UI_Main : MonoBehaviour{


	public Transform UI_Main_Panel;
	public Image UI_Main_TitleImage,UI_Main_TitleImage_Mask2;
	private RectTransform UI_Main_Panel_RectTransform = null;
	public Text toHide,toHide2;
	private bool OnceTrig = false;

	// Use this for initialization
	void Start () {
		UI_Main_Panel.gameObject.SetActive(false);
		k514SoundManager.singleton.PlayAudioClip(SFX_TYPE_BGM.BGM0);
		UI_Main_Panel_RectTransform = UI_Main_Panel.GetComponent<RectTransform>();
	}

	IEnumerator StartAnimation(){
		float moveTime = 0.5f;
		Vector3 InitPos = UI_Main_Panel_RectTransform.anchoredPosition;

		k514SoundManager.singleton.PlayAudioClip(SFX_TYPE_UI.UI2);
		Vector3 home = UI_Main_Panel_RectTransform.anchoredPosition;
		float start = 0f , reversedMoveTime = 1f/moveTime , normalized = 0f;
		while(start < moveTime){
			toHide.enabled = false;
			start += Time.deltaTime;
			normalized = start*reversedMoveTime;
			UI_Main_TitleImage.fillAmount = 0.52f + 0.48f*normalized;
			UI_Main_TitleImage_Mask2.fillAmount = 0.63f*normalized;			
			UI_Main_Panel_RectTransform.anchoredPosition = Vector3.Lerp(InitPos,InitPos + Vector3.right * 380f ,normalized);
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		k514SoundManager.singleton.PlayAudioClip(SFX_TYPE_UI.UI3);
		toHide2.gameObject.SetActive(true);
		yield return new WaitForSeconds(4f);		

		k514SoundManager.singleton.ClearSfx();
		k514Loader.sceneName = "k514Stage1";
		SceneManager.LoadScene("k514LoadingScene");
	}

	public void OnMenuButtonClicked(k514UI_EnumSet sc){
		k514SoundManager.singleton.PlayAudioClip(SFX_TYPE_UI.UI1);
		switch(sc.GetIndex()){
			case MENU_BUTTON.START :
				if(!OnceTrig){
					StartCoroutine(StartAnimation());
					OnceTrig = true;
				}
			break;
			case MENU_BUTTON.END :
				Application.Quit();
			break;
		}
	}

	public void OnMenuButtonClicked(){
				if(!OnceTrig){
					k514SoundManager.singleton.PlayAudioClip(SFX_TYPE_UI.UI1);
					StartCoroutine(StartAnimation());
					OnceTrig = true;
				}
	}
	public void OnMenuButtonClickedE(){
		Application.Quit();
	}

}
