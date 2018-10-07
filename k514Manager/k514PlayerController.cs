using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public enum PlayerState { GROUND , AIR};

public class k514PlayerController : MonoBehaviour {

	// test preperty
	// public Transform lookAt;

	public GameObject Effect1,Effect2,Effect3,Effect4,CrossHair;
	public k514EasterEgg portait;
	// game status //

		// max bullet
		private int MAX_BULLET = 14;
		public static int CURRENT_BULLET = 0;
		// max altitude
		float MAX_ALTITUDE = 40f;
		// current Score
		[System.NonSerialized]public int SCORE = 0, SCORE_STORAGE = 0;

		// for SP //
		private float INNER_SP = 0.5f;
		private float MAX_SP = 1f;
		public float SP{
			get{
				return INNER_SP;
			}
			set{
				if(value > MAX_SP) INNER_SP = MAX_SP;
				else if(value < 0) INNER_SP = 0;
				else INNER_SP = value;
			}
		}

		
		// for PP //
		private int INNER_PP = 0;
		private int MAX_PP = 127;
		public int sPP{
			get{
				return INNER_PP;
			}
			set{
				if(value > MAX_PP) INNER_PP = MAX_PP;
				else if(value < 0) INNER_PP = 0;
				else INNER_PP = value;
			}
		}


		// for BOMB, zanki //
		public int BOMB = 2;
		public int ZANKI = 2;

	// game stataus end //


    public float Speed = 5f;
	private float inner_Speed = 0f;
    public float JumpHeight = 2f;
	public float Gravity = 2f;
    public float DashDistance = 5f;
	public float DirectionDampTime = .25f;
	public bool GravityTrig = true, MovableTrig = true, ShotableTrig = true, BombTrig = false, OnceTrig = false, EventTrig = false, InvisibleTrig = false;
	public k514EigenAbillity CharacterAbillity;
	public k514EigenBomb Reimu_Bomb;

	[System.NonSerialized]public Animator animator;
	protected CharacterController controller;

	[System.NonSerialized]public static PlayerState CURRENT_STATE = PlayerState.GROUND;
	private Vector3 MoveDir = Vector3.zero;
	private float y_coord = 0f;
	private k514AudioSource loop_sound_Flying = null,Dash = null;

	// Use this for initialization
	void Start () 
	{

		animator = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
		inner_Speed = Speed;
		InitStatus();
		//if(animator.layerCount >= 2)
		//	animator.SetLayerWeight(2, 1);
		//팔에 대해서는 무조건 wave anim의 제어를 받도록..
	}

	void InitStatus(){
		CharacterAbillity.gameObject.SetActive(false);
		CharacterAbillity.Init(transform);
	}
		
	void ScoreCheck(){
		if(!Input.GetKey(KeyCode.V) && SP < 0.1f){
			SP += 0.0008f;
		}else if(!Input.GetKey(KeyCode.V)){
			SP += 0.00001f;
		}
		k514SystemManager.UI_SpecaMgr.Rei_UI_Update();
		if(SCORE_STORAGE > 100){
			SCORE_STORAGE-=10;
			SCORE+=10;
			k514SystemManager.UI_SpecaMgr.Score_UI_Update();
			return;
		}
		if(SCORE_STORAGE > 10){
			SCORE_STORAGE-=3;
			SCORE+=3;
			k514SystemManager.UI_SpecaMgr.Score_UI_Update();
			return;
		}
		if(SCORE_STORAGE > 0){
			SCORE_STORAGE--;
			SCORE++;
			k514SystemManager.UI_SpecaMgr.Score_UI_Update();
			return;
		}
		
		

	}

	public void EventEnd(){
		EventTrig = false;
		CrossHair.SetActive(true);
	}

	// Update is called once per frame
	void FixedUpdate () 
	{


		if(!OnceTrig){
			OnceTrig = true;
			k514SystemManager.UI_SpecaMgr.Power_UI_Update();
			k514SystemManager.UI_SpecaMgr.Zanki_UI_Update();
			k514SystemManager.UI_SpecaMgr.Bomb_UI_Update();
			k514SystemManager.UI_SpecaMgr.Score_UI_Update();
		}

		if(EventTrig){
			CrossHair.SetActive(false);
		}

		ScoreCheck();

		//test code//
		// transform.LookAt(new Vector3(lookAt.position.x,0,lookAt.position.z));
		ApplyBomb();
		ApplySlowMode();
		if(CURRENT_STATE == PlayerState.AIR) ApplyFastMode();
		ApplyMove();
		ApplyLeftRightBound();

		if (animator)
		{
			switch(CURRENT_STATE){
				case PlayerState.GROUND :
					GroundMotion();
					break;
				case PlayerState.AIR :
					AirMotion();
					break;				
			}
		} 
		ApplyMotionBugCheck();
		ApplyGravity();
		ApplyAbillityKey();
		ApplyDontBack();
		if(MovableTrig) controller.Move(MoveDir * Time.fixedDeltaTime);  
	}

	void ApplyBomb(){
		if(Input.GetKey(KeyCode.Z) && !EventTrig && this.BOMB > 0 && !BombTrig){
			BombTrig = true;
			this.BOMB -- ;
			k514SystemManager.UI_SpecaMgr.Bomb_UI_Update();
			StartCoroutine(Reimu_Bomb.Musou_Fuuin());
		}
	}



	void ApplySlowMode(){
		if(Input.GetKey(KeyCode.LeftShift) && !EventTrig ){
			inner_Speed = Speed*0.5f;
		}else{
			inner_Speed = Speed;
		}
	}


	void ApplyFastMode(){
		if(Input.GetKeyDown(KeyCode.V) && !BombTrig && !EventTrig && SP > 0.005f){
			k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.DASH,false);
			if(Dash==null) Dash = k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.WIND,false);
			Debug.Log(333333333333);
		}
		if(Input.GetKey(KeyCode.V) && !BombTrig && !EventTrig && SP > 0.005f){
			inner_Speed = Speed*4f;
			EffectSetter2(true);
			SP -= 0.004f;
		}else{
			inner_Speed = Speed;
			EffectSetter2(false);
			if(Dash!=null){
				if(Dash.gameObject.active){
					Dash.StopClip();
				}
				Dash = null;
			}
		}
	}

	void ApplyLeftRightBound(){
		if( (transform.position.x >= 79f && MoveDir.x > 0 ) || transform.position.x <= 1f && MoveDir.x < 0 ){
			MoveDir.x = 0f;
		}
	}


	void ApplyMove(){
		MoveDir = EventTrig ? Vector3.zero : new Vector3(Input.GetAxis("Horizontal"), y_coord, Input.GetAxis("Vertical"));
		MoveDir *= inner_Speed;
		MoveDir.z = BombTrig ? 0.25f*MoveDir.z : 0.5f*MoveDir.z;
		
	}

	IEnumerator ApplyJump(int Cnt){
		WaitForSeconds wait = new WaitForSeconds(0.01f);
		CURRENT_STATE = PlayerState.AIR;
		GravityTrig = false;
		for(int i = 0 ; i < Cnt ; i++){
			y_coord = JumpHeight;
			yield return wait;
		}
		y_coord = 0f;
		yield return null;
	}

	void ApplyMotionBugCheck(){
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			
		if(stateInfo.IsName("Base Layer.Rolling")){
			if(controller.isGrounded) animator.SetBool("Dive", true); 				
			GravityTrig = true;
		}

		if (stateInfo.IsName("Base Layer.Idle"))
		{			
			animator.SetBool("Jump", false); 
			animator.SetBool("Dive", false); 				
			CURRENT_STATE = PlayerState.GROUND;
			GravityTrig = true;
		}

	}

	void ApplyGravity(){
		if(GravityTrig && !controller.isGrounded){
			MoveDir.y -= Gravity * Time.deltaTime;
		}
	}

	void ApplyAbillityKey(){
		if(MoveDir.z < 0 && CURRENT_STATE == PlayerState.GROUND){
			CharacterAbillity.gameObject.SetActive(true);
			CharacterAbillity.Activate();
		}else{
			CharacterAbillity.Deactivate();
		}
	}

	void ApplyDontBack(){
		if(MoveDir.z < 0) MoveDir.z = 0;
	}


	void GroundMotion(){
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			
			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (Input.GetKey(KeyCode.C) && !EventTrig && MoveDir.z >= 0){
					animator.SetBool("Jump", true); 
					StartCoroutine(ApplyJump(10));
					k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.JUMP,false);
				}               
			}

			float h = EventTrig ? 0f : Input.GetAxis("Horizontal");
        	float v = EventTrig ? 0f : Input.GetAxis("Vertical");
			animator.SetFloat("Speed", h*h+v*v);
            animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);	
	}

	void AirMotion(){
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			
			
			if (stateInfo.IsName("Base Layer.Flying"))
			{

				EffectSetter(true);

				GravityTrig = false;
				if(Input.GetKey(KeyCode.C) && !EventTrig && transform.position.y < MAX_ALTITUDE){
					MoveDir.y += inner_Speed;
				}

				if(loop_sound_Flying == null) loop_sound_Flying = k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.FLYING,false,true);
			}else{
				EffectSetter(false);
			}
			
			if (stateInfo.IsName("Base Layer.Rolling"))
			{
				GravityTrig = true;
				if(controller.isGrounded){
					animator.SetBool("Dive", true); 				
				}

				if(loop_sound_Flying != null) loop_sound_Flying.StopClip();
				loop_sound_Flying = null;
			}

			if (stateInfo.IsName("Base Layer.Dive"))
			{			
				animator.SetBool("Jump", false); 
				animator.SetBool("Dive", false); 				
			}


			if (stateInfo.IsName("Base Layer.Idle"))
			{			
				CURRENT_STATE = PlayerState.GROUND;
			}

			float h =  EventTrig ? 0f :  Input.GetAxis("Horizontal");
        	float v =  EventTrig ? 0f :  Input.GetAxis("Vertical");
			animator.SetFloat("Speed", h*h+v*v);
            animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);	
	}

	void EffectSetter(bool trig){
		Effect1.SetActive(trig);
		Effect2.SetActive(trig);
	}

	void EffectSetter2(bool trig){
		Effect3.SetActive(trig);
		Effect4.SetActive(trig);
	}

	public bool isShottable(){
		// Debug.Log("M"+MAX_BULLET);
		// Debug.Log("C"+CURRENT_BULLET);
		return MAX_BULLET > CURRENT_BULLET;
	}

	public void SetBombTrig(bool b){
		BombTrig = b;
	}


	protected void OnTriggerEnter(Collider hit) {
		if(hit.gameObject.CompareTag("Bullet") && !InvisibleTrig){
			k514BulletBridge tmp = hit.gameObject.GetComponent<k514BulletBridge>();
			if(tmp.isEnemyShot() && !tmp.GetOnceTrig()){
				tmp.DoInteract();
				ZANKI--;
				InvisibleTrig = true;
				if(ZANKI<0){
					StartCoroutine(GameOver());					
				}else{
					StartCoroutine(Invisible());
				}
				k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.PICHU);
				k514SystemManager.UI_SpecaMgr.Zanki_UI_Update();
				k514SystemManager.EffectMgr.CastEffect(EFFECT_TYPE.PHIT,transform.position+Vector3.up*0.4f);
			}
		}
	}

	IEnumerator GameOver(){
		yield return null;
		EventTrig = true;
		LayerMask layerMask = ~(1 << LayerMask.NameToLayer("Player"));
		layerMask = ~(1 << LayerMask.NameToLayer("Player"));
		Camera.main.cullingMask = layerMask;
		portait.SetIcon(Random.Range(1,4));
		k514SystemManager.OptionMgr.StopBGM();
		StartCoroutine(k514SystemManager.UI_SpecaMgr.UI_Slide_out(1f));
		yield return new WaitForSeconds(1.5f);
		k514SystemManager.SerifuMgr.Act4(false);
		while(!k514SystemManager.SerifuMgr.isEnd()){
            yield return null;
        }
        k514SystemManager.SerifuMgr.EndProcess();
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("k514GameOver");
	}	

	IEnumerator Invisible(){
		yield return null;
		portait.SetIcon(Random.Range(1,4));
		LayerMask layerMask = ~(1 << LayerMask.NameToLayer("Player"));
		for(int i = 0 ; i < 60 ; i++){
			if(i%2 == 0){
				layerMask = ~(1 << LayerMask.NameToLayer("Player"));
				Camera.main.cullingMask = layerMask;
			}else{
				layerMask = ~0;
				Camera.main.cullingMask = layerMask;
			}
			yield return new WaitForSeconds(0.07f);
		}
		yield return new WaitForSeconds(1f);
		portait.SetIcon(0);		
		InvisibleTrig = false;
	}
}
