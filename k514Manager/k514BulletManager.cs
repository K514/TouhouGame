using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BULLET_TYPE{
	AMULLET,TEST,TEST2,SHADOW,SMALL_CB,SMALL_SP,THUNDER,RED,RUMIA,RUMIA2,END
}

public class k514BulletManager : MonoBehaviour {

	public bool trig = false;
	public Transform particle;
	public static k514BulletManager singleton = null;
	public Transform[] pfab;
	public List<k514BulletBridge> bulletPool;
	public List<k514BulletBridge>[] bulletDisablePool;

	// Use this for initialization
	void Awake () {
		if(singleton == null){
			singleton = this;
		}
		else if(singleton != this) Destroy(gameObject);
	
		bulletPool = new List<k514BulletBridge>();
		bulletDisablePool = new List<k514BulletBridge>[(int)BULLET_TYPE.END];
		for(int i = 0 ; i < (int)BULLET_TYPE.END ; i++){
			bulletDisablePool[i] = new List<k514BulletBridge>();
		}
	}

	public void KillAllBullet(){
		Transform tmp = null;
		for(int i = 0 ; i < transform.childCount ; i++){
			
			if (transform.GetChild(i).gameObject.active){
				tmp = Instantiate(particle);
				tmp.position = transform.GetChild(i).position;
				transform.GetChild(i).GetComponent<k514BulletBridge>().Interactive_Destroy();
			}
		}
	}


	public void KillAllBulletNoEf(){
		for(int i = 0 ; i < transform.childCount ; i++){
			
			if (transform.GetChild(i).gameObject.active){
				transform.GetChild(i).GetComponent<k514BulletBridge>().Interactive_Destroy();
			}
		}
	}
	
	public Transform CreateBullet<T>(BULLET_TYPE type, Vector3 dir, float moveTime, float speed, Transform TargetPlace = null, int lifeSpan = 100, float damage = 5f, bool whos_shot = true) where T : k514BulletBridge{
		int index = (int)type;
		if(index >= pfab.Length) index = pfab.Length;
		Transform tmp = null;
		T tmp2 = null;
		k514BulletBridge tmp3 = null;

		int Cnt = bulletDisablePool[index].Count;
		if(Cnt<1){
			tmp = Instantiate(pfab[index]);
			tmp2 = tmp.gameObject.AddComponent<T>();
			tmp2.SetID(index);
		}else{

			tmp3 = bulletDisablePool[index][Cnt-1];
			if(tmp3.GetType() == typeof(T)){
				tmp2 = (T)tmp3;
			}else{
				tmp = tmp3.transform;
				Destroy(tmp.GetComponent<k514BulletBridge>());
				tmp2 = tmp.gameObject.AddComponent<T>();
			}
			tmp = tmp2.transform;
			tmp.gameObject.SetActive(true);
			bulletDisablePool[index].RemoveAt(Cnt-1);
		}

		tmp2.Init(dir, moveTime, speed, lifeSpan, damage, whos_shot);

		if(TargetPlace!=null){
			tmp2.SetTargetTransform(TargetPlace);
		}
		return tmp;
	}

		
	public void KillBullet(int index){
		bulletDisablePool[bulletPool[index].GetID()].Add(bulletPool[index]);
		Transform tmp;
		
		// 패턴 제거
		for(int i = 0 ; i < bulletPool[index].transform.childCount ; i++){
			tmp = bulletPool[index].transform.GetChild(i);
			if(tmp.name == "Pattern") Destroy(tmp.gameObject);
		}

		bulletPool[index].gameObject.SetActive(false);
		bulletPool.RemoveAt(index);
	}

	void Update(){

		if(trig){
			trig = false;
			KillAllBullet();
		}

		for(int i = 0 ; i < bulletPool.Count ; i++){
			if(bulletPool[i].isDead()){
				bulletPool[i].onceTrig = false;
				KillBullet(i);
				i--;
			}else{
				if(Time.timeScale != 0f)
					bulletPool[i].Progress();
			}
		}
	}

}
