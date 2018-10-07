using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class k514EnemyController : IPatternable {

	// object status relatives
	protected float firstAttackInterval;
	protected float AttackInterval;
	public float HP = 10f;
	protected float MAX_HP;

	protected bool OnceTrig = false, isBoss = false;
	protected WaitForSeconds coolTime = null;
	protected Transform bullet = null;
	protected Transform target = null;
	protected Vector3 firstView;
	protected IEnumerator attack_patterns = null;
	protected Animator anim;
	protected Rigidbody rigid;

	// Enemy Behaviour
	[System.NonSerialized]public List<k514EnemyBehaviour> patterns;
	protected k514EnemyBehaviour currentPatterns = null;
	protected int patternsIndex = 0;


	// bomb Desc
	protected Dictionary<int,k514MusouFuuin> bombs_check = null;


	// Use this for initialization

	protected virtual IEnumerator GetAttckPatterns(){
		return AttackPattern();
	}


	void Start(){
		anim = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		firstView = transform.forward;
		patterns = new List<k514EnemyBehaviour>();
		bombs_check = new Dictionary<int,k514MusouFuuin>();
		MAX_HP = HP;
		//InitPatterns();
	}

	public float GetMaxHP(){
		return MAX_HP;
	}

	public float GetHP(){
		return HP;
	}

	protected void SetAttackInterval(){
		firstAttackInterval = Random.Range(3f,5f);
		AttackInterval = Random.Range(2f,4f);
		coolTime = new WaitForSeconds(AttackInterval);
	}

	protected void OnEnable(){
		SetAttackInterval();
		if(MAX_HP!=0)HP = MAX_HP;
		if(attack_patterns==null){
			attack_patterns = GetAttckPatterns();
			OnceTrig = false;
		}
	}

	protected void OnDisable(){
		if(attack_patterns!=null){
			StopCoroutine(attack_patterns);
			attack_patterns = null;
		}
	}

	void InitPatterns(){
		k514RotateOn tmp0 = (k514RotateOn)k514EnemyPatternFactory.CreatePattern<k514RotateOn>(this);
		tmp0.Init(this,Vector3.up);
		patterns.Add(tmp0);

		k514MoveDirect tmp = (k514MoveDirect)k514EnemyPatternFactory.CreatePattern<k514MoveDirect>(this);
		tmp.Init(this,Vector3.forward*20);
		patterns.Add(tmp);

		tmp = (k514MoveDirect)k514EnemyPatternFactory.CreatePattern<k514MoveDirect>(this);
		tmp.Init(this,new Vector3(40,0,20));
		patterns.Add(tmp);

		k514MoveBezier tmp2 = (k514MoveBezier)k514EnemyPatternFactory.CreatePattern<k514MoveBezier>(this);
		tmp2.Init(this,new Vector3(20,40,20),new Vector3(40,60,40),new Vector3(20,0,20),new Vector3(0,0,40));
		patterns.Add(tmp2);
	
		
	}

	protected virtual void Update(){

		// late start
		if(!OnceTrig){
			OnceTrig = true;
			StartCoroutine(attack_patterns);
		}
		
		// bombs
		if(bombs_check.Count > 0){
			int[] key = bombs_check.Keys.ToArray();
			for(int i = 0 ; i < key.Length ; i++){
				try{
					if(!bombs_check[key[i]].isAlive()){
						DoInteract_HP(bombs_check[key[i]].GetDamage());
						bombs_check.Remove(key[i]);
						k514SystemManager.InteractMgr.GetPlayerTransform().GetComponent<k514PlayerController>().SP += Random.Range(0.008f,0.012f);
					}
				}catch(System.Exception e){

				}
			}
		}



		if(currentPatterns == null && patternsIndex == patterns.Count) return;

		if(currentPatterns == null && patterns.Count > 0){
			currentPatterns = patterns[patternsIndex];
			patternsIndex++;
			if(LoopTrig) patternsIndex = patternsIndex % patterns.Count;
		}
		if(currentPatterns != null && currentPatterns.Progress()){
			// Progress 진행됨.
		}else{
			// Progress 종료됨.
			if(currentPatterns != null) currentPatterns.EndProcess();
			currentPatterns = null;
		}
		if(RotateTrig) CheckRotation();
		else SetTarget();



	}

	protected virtual IEnumerator AttackPattern(){
		yield return null;
		yield return new WaitForSeconds(firstAttackInterval);
		while(true){
                k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT1,false);
				bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.TEST,k514SystemManager.InteractMgr.GetPlayerHitVectorFrom(transform.position),0.05f,10f);
				bullet.position = transform.position;
				bullet.gameObject.name = "Test";
				bullet.parent = k514SystemManager.BulletMgr.transform;
				k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
				yield return coolTime;
		}
	}

	protected virtual void CheckRotation(){
		transform.Rotate(RotationAxis,Time.time,Space.Self);
	}

	protected virtual void SetTarget(){
		target = k514SystemManager.InteractMgr.GetPlayerHitTransform();
		if(target!=null){
			transform.forward = firstView = target.position - transform.position ;
		}
		else transform.forward = firstView;
	}

	protected virtual void OnTriggerEnter(Collider hit) {
		if(hit.gameObject.CompareTag("Bullet")){
			k514BulletBridge tmp = hit.GetComponent<k514BulletBridge>();
			if(!tmp.isEnemyShot() && !tmp.GetOnceTrig()){
				tmp.DoInteract();
				if(!isBoss) k514SystemManager.EffectMgr.CastEffect(EFFECT_TYPE.HIT,transform.position+Vector3.up*0.4f);
				k514SystemManager.InteractMgr.GetPlayerTransform().GetComponent<k514PlayerController>().SCORE_STORAGE += Random.Range(30,60);
				k514SystemManager.InteractMgr.GetPlayerTransform().GetComponent<k514PlayerController>().SP += Random.Range(0.003f,0.005f);
				this.DoInteract_HP(tmp.GetDamage());
			}
		}

		if(hit.gameObject.CompareTag("Bomb")){
			Debug.Log("in!");
			k514MusouFuuin tmp2 = hit.GetComponent<k514MusouFuuin>();
			bombs_check.Add(tmp2.GetID(),tmp2);
		}
	}

	protected virtual void OnTriggerExit(Collider hit) {

		if(hit.gameObject.CompareTag("Bomb")){
			Debug.Log("out!");
			k514MusouFuuin tmp2 = hit.GetComponent<k514MusouFuuin>();
			bombs_check.Remove(tmp2.GetID());
		}
	}


	protected virtual void DoInteract_HP(float damage){
		this.HP -= damage;
		if(this.HP<=0){
			k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_ENEMY.ENEMY_DEAD);
			k514SystemManager.InteractMgr.GetPlayerTransform().GetComponent<k514PlayerController>().SP += Random.Range(0.012f,0.015f);			
			DoInteract_Dead();
			PopItem();
		}else{
			k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_ENEMY.ENEMY_HIT);

		}
	}
	public virtual void DoInteract_Dead(){
		bombs_check.Clear();
	}
	public abstract void PopItem();
}
