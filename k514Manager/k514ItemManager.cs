using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM_TYPE{
	POWER,SCORE,END
}

public class k514ItemManager : MonoBehaviour {

	public static k514ItemManager singleton = null;
	private List<k514ItemController>[] itemDisablePool;
	private bool OnceTrig = false;

	// Use this for initialization
	void Awake () {
		if(singleton == null){
			singleton = this;
		}
		else if(singleton != this) Destroy(gameObject);
	}

	void Start(){
		itemDisablePool = new List<k514ItemController>[(int)ITEM_TYPE.END];
		for(int i = 0 ; i < (int)ITEM_TYPE.END ; i++){
			itemDisablePool[i] = new List<k514ItemController>();
		}
	}

	void Update(){

		if(!OnceTrig){
			OnceTrig = true;

			Transform tmp = null;
			for(int i = 0 ; i < transform.childCount ; i++){
				tmp =  transform.GetChild(i);
				switch(tmp.name){
					case "Power" :
						itemDisablePool[(int)ITEM_TYPE.POWER].Add(tmp.GetComponent<k514ItemController>());
					break;

                    case "Score" :
						itemDisablePool[(int)ITEM_TYPE.SCORE].Add(tmp.GetComponent<k514ItemController>());
					break;
				}
				tmp.gameObject.SetActive(false);
			}
		}

	}
	
	public k514ItemController GetPooledItem(ITEM_TYPE index){
		int Cnt = itemDisablePool[(int)index].Count;
		k514ItemController	tmp = (itemDisablePool[(int)index])[Cnt-1];
		tmp.gameObject.SetActive(true);
		itemDisablePool[(int)index].RemoveAt(Cnt-1);
		return tmp;
	}

	public void KillItem(k514ItemController item , ITEM_TYPE index){
		item.gameObject.SetActive(false);
		itemDisablePool[(int)index].Add(item);
	}

}
