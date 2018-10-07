using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514UI_Loading_Animation : MonoBehaviour {

	public int sprite_num;

	private int nowIndex = 0;
	private Object[] sprites;
	private Image Loading_Image_Panel = null;
	private float nowTime = 0f;

	void Start(){
		Loading_Image_Panel = GetComponent<Image>();
		sprites = Resources.LoadAll("UI/Image/Th11_Load",typeof(Sprite));
		nowTime = Time.time;
	}


	// Update is called once per frame
	void Update () {
		if(Time.time - nowTime > 0.05f){
			
			nowTime = Time.time;

			Loading_Image_Panel.sprite = sprites[nowIndex] as Sprite;		
			nowIndex++;
			if(nowIndex>=sprites.Length){
				nowIndex = 0;
			}
		}
	}
}
