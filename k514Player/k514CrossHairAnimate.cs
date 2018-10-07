using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514CrossHairAnimate : MonoBehaviour {

	public Transform leftUp , leftDown , rightUp, rightDown, center, leftUpBG, leftDownBG, RightUpBG, RightDownBG;
	private SpriteRenderer[] renderer;
	private int sprite_num = 69, nowIndex = 0;
	private Object[] sprites;

	public Color ColorDetected, ColorNormal;
	public float AlphaMin, AlphaMax; 
	private Color tmp_col;
	private float alpha;


	public bool targetDetected = false;
	
	// animate
	private IEnumerator anime;
	float AnimateInterval = 0.01f;
	bool AnimateTrig = true , OnceTrig = false;
	float offset = 0f;
	// hide
	float hideLerp = 0.75f;

	// transform
	float RotateZ = 30f;
	float x = 5, y = 3;


	// Use this for initialization
	void Start () {
		alpha = AlphaMin;
		tmp_col = ColorDetected;

		sprites = Resources.LoadAll("UI/Image/Rune",typeof(Sprite));
		renderer = new SpriteRenderer[9];
			renderer[0] = leftUp.GetComponent<SpriteRenderer>();
			renderer[1] = leftDown.GetComponent<SpriteRenderer>();
			renderer[2] = rightUp.GetComponent<SpriteRenderer>();
			renderer[3] = rightDown.GetComponent<SpriteRenderer>();
			renderer[4] = center.GetComponent<SpriteRenderer>();
			renderer[5] = leftUpBG.GetComponent<SpriteRenderer>();
			renderer[6] = leftDownBG.GetComponent<SpriteRenderer>();
			renderer[7] = RightUpBG.GetComponent<SpriteRenderer>();
			renderer[8] = RightDownBG.GetComponent<SpriteRenderer>();
			
		center.gameObject.SetActive(false);
		anime = AnimateSprite();
		StartCoroutine(anime);
		OnceTrig = true;
	}

	void OnEnable(){
		if(OnceTrig){
			StartCoroutine(anime);
		}
	}

	void OnDisable(){
		StopCoroutine(anime);
	}


	IEnumerator AnimateSprite(){
		yield return null;
		bool Direct = true;
		WaitForSeconds interval = new WaitForSeconds(AnimateInterval);
		while(AnimateTrig){
			
			for(int i = 0 ; i < 4 ; i++){
				renderer[i].sprite = sprites[nowIndex] as Sprite;		
			}

			nowIndex++;
			if(nowIndex>=sprites.Length){
				nowIndex = 0;
			}

			alpha = (Direct) ? Mathf.Lerp(alpha,AlphaMax,Time.deltaTime) : Mathf.Lerp(alpha,AlphaMin,3f*Time.deltaTime) ;
			if(alpha > AlphaMax - 0.1f){
				Direct = false;
			}else if(alpha < AlphaMin + 0.1f){
				Direct = true;
			}

			yield return interval;
		}
	}
	
	// Update is called once per frame
	void Update () {
		offset = 30f*alpha*(alpha-0.5f);

		if(!targetDetected){
			center.gameObject.SetActive(false);
			tmp_col = Color.Lerp(tmp_col,ColorNormal,Time.deltaTime);
			AnimateInterval = 0.1f;
			leftUp.position = Vector3.Lerp(leftUp.position,transform.position + new Vector3(-(5+offset),3,0),Time.deltaTime);
			leftUp.eulerAngles = Vector3.forward*Mathf.Lerp(leftUp.eulerAngles.z,30f,Time.deltaTime);
			leftDown.position = Vector3.Lerp(leftDown.position,transform.position + new Vector3(-(5+offset),-3,0),Time.deltaTime);
			leftDown.eulerAngles = Vector3.forward*Mathf.Lerp(leftDown.eulerAngles.z,150f,Time.deltaTime);
			rightUp.position = Vector3.Lerp(rightUp.position,transform.position + new Vector3((5+offset),3,0),Time.deltaTime);
			rightUp.eulerAngles = Vector3.forward*Mathf.Lerp(rightUp.eulerAngles.z,150f,Time.deltaTime);
			rightDown.position = Vector3.Lerp(rightDown.position,transform.position + new Vector3((5+offset),-3,0),Time.deltaTime);
			rightDown.eulerAngles = Vector3.forward*Mathf.Lerp(rightDown.eulerAngles.z,30f,Time.deltaTime);
		}else{
			center.gameObject.SetActive(true);
			center.Rotate(Vector3.forward,60f*Time.deltaTime,Space.Self);
			center.position = alpha*k514SystemManager.InteractMgr.GetPlayerVector() + (1f-alpha)*transform.position;
			tmp_col = Color.Lerp(tmp_col,ColorDetected,Time.deltaTime);
			AnimateInterval = 0.02f;	
			leftUp.position = Vector3.Lerp(leftUp.position,transform.position + new Vector3(-(4+offset),2,0),Time.deltaTime);
			leftUp.eulerAngles = Vector3.forward*Mathf.Lerp(leftUp.eulerAngles.z,45f,Time.deltaTime);
			leftDown.position = Vector3.Lerp(leftDown.position,transform.position + new Vector3(-(4+offset),-2,0),Time.deltaTime);
			leftDown.eulerAngles = Vector3.forward*Mathf.Lerp(leftDown.eulerAngles.z,135f,Time.deltaTime);
			rightUp.position = Vector3.Lerp(rightUp.position,transform.position + new Vector3((4+offset),2,0),Time.deltaTime);
			rightUp.eulerAngles = Vector3.forward*Mathf.Lerp(rightUp.eulerAngles.z,135f,Time.deltaTime);
			rightDown.position = Vector3.Lerp(rightDown.position,transform.position + new Vector3((4+offset),-2,0),Time.deltaTime);
			rightDown.eulerAngles = Vector3.forward*Mathf.Lerp(rightDown.eulerAngles.z,45f,Time.deltaTime);
		}
			
		tmp_col.a = alpha;
		renderer[5].color = renderer[6].color = renderer[7].color = renderer[8].color = tmp_col;

	}
}
