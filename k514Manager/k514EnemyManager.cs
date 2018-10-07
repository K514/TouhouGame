using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_TYPE{
	FAIRY,FAIRY2,RUMIA,END
}

public class k514EnemyManager : MonoBehaviour {

	public static k514EnemyManager singleton = null;
	private List<k514EnemyController>[] enemyDisablePool;
	private bool OnceTrig = false;

	[System.NonSerialized]public int ENEMY_CNT = 0;

	// Use this for initialization
	void Awake () {
		if(singleton == null){
			singleton = this;
		}
		else if(singleton != this) Destroy(gameObject);
	}

	void Start(){
		enemyDisablePool = new List<k514EnemyController>[(int)ENEMY_TYPE.END];
		for(int i = 0 ; i < (int)ENEMY_TYPE.END ; i++){
			enemyDisablePool[i] = new List<k514EnemyController>();
		}
	}

	void Update(){

		if(!OnceTrig){

			Transform tmp = null;
			for(int i = 0 ; i < transform.childCount ; i++){
				tmp =  transform.GetChild(i);
				switch(tmp.name){
					case "Fairy" :
						enemyDisablePool[(int)ENEMY_TYPE.FAIRY].Add(tmp.GetComponent<k514EnemyController>());
					break;
					case "Fairy2" :
						enemyDisablePool[(int)ENEMY_TYPE.FAIRY2].Add(tmp.GetComponent<k514EnemyController>());
					break;
					case "BossRumia" :
						enemyDisablePool[(int)ENEMY_TYPE.RUMIA].Add(tmp.GetComponent<k514EnemyController>());
					break;
				}
				tmp.gameObject.SetActive(false);
			}
			Shuffle(enemyDisablePool[(int)ENEMY_TYPE.FAIRY]);
			Shuffle(enemyDisablePool[(int)ENEMY_TYPE.FAIRY2]);
			OnceTrig = true;
		}

	}
	
	public k514EnemyController GetPooledEnemy(ENEMY_TYPE index){
		ENEMY_CNT++;
		int Cnt = enemyDisablePool[(int)index].Count;
		k514EnemyController	tmp = (enemyDisablePool[(int)index])[Cnt-1];
		tmp.gameObject.SetActive(true);
		enemyDisablePool[(int)index].RemoveAt(Cnt-1);
		return tmp;
	}

	public void KillEnemy(k514EnemyController enemy , ENEMY_TYPE index){
		ENEMY_CNT--;
		enemy.gameObject.SetActive(false);
		enemyDisablePool[(int)index].Add(enemy);
	}

	public void Shuffle(List<k514EnemyController> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = Random.Range(0,n);  
			k514EnemyController value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

}
