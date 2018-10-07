using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class k514UISpellCardManager : MonoBehaviour {

	public static k514UISpellCardManager singleton = null;
    public RectTransform lower;
    public RectTransform magic_circle, rune;
    public Sprite[] number;
    public Sprite[] m_a_x;
    public Image[] power_ui,zanki_ui,bomb_ui,score_ui;
	public Image blackFade,BlueFade,ReiMask,ReiUI;
    public RectTransform[] bgm_name;
    public RectTransform Score,Boss,Ecutin;
    public RectTransform[] enemySpell;


    public RectTransform upper;
    private k514PlayerController p_sComp = null;
    private bool UI_toggle = false, UI_Black = false, Bosshp_toggle = false;
    private k514EnemyController target = null;
    private IEnumerator BossHP_Progress = null;

    // Use this for initialization
	void Awake () {
		if(singleton == null){
			singleton = this;
		}
		else if(singleton != this) Destroy(gameObject);
	}

    void Start(){
        p_sComp = GameObject.Find("Player").GetComponent<k514PlayerController>();
        Boss.gameObject.SetActive(false);
        BossHP_Progress = BossHP_Progress_inner();
    }

    public void EnemySpellAnimate(int NameIndex, float moveTime){
        StartCoroutine(E_SpellCardAnimating(NameIndex,moveTime));
    }

    public IEnumerator E_SpellCardAnimating(int NameIndex, float moveTime){
        yield return null;
        RectTransform Name = this.enemySpell[NameIndex];
        RectTransform CutIn = this.Ecutin;

        CutIn.gameObject.SetActive(true);
        Name.gameObject.SetActive(true);
        bool trig1 = true;

		Vector3 CutIn_home = CutIn.anchoredPosition,Name_home = Name.anchoredPosition;

        float start = 0f, start2 = moveTime*0.2f , reversedMoveTime = 1f/moveTime , normalized = 0f;
		while(start2 < moveTime){
			start += Time.deltaTime;
			normalized = start*reversedMoveTime;
            magic_circle.Rotate(Vector3.forward,360f*start);
            rune.Rotate(Vector3.back,36f*start);
			//UI_Main_TitleImage.color = new Color(1f - 2*start,1f - 2*start,1f - 2*start,1f);
			if(start < moveTime) {
                CutIn.anchoredPosition = Vector3.Lerp(CutIn_home,CutIn_home + Vector3.left * 100f,normalized);
            }
            if(start > start2){
                if(trig1){
                    trig1 = false;
                    start2 = 0f;    
                }
                start2 += Time.deltaTime;
    			normalized = start2*reversedMoveTime;
                Name.anchoredPosition = Vector3.Lerp(Name_home,Name_home + Vector3.down * 400f,normalized);
            }
            yield return null;
		}
        yield return new WaitForSeconds(0.8f);

        CutIn.anchoredPosition = CutIn_home;
        CutIn.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        Name.anchoredPosition = Name_home;
        Name.gameObject.SetActive(false);
    }

    public IEnumerator SpellCardAnimating(RectTransform CutIn, RectTransform Name, float moveTime){
        yield return null;
        CutIn.gameObject.SetActive(true);
        Name.gameObject.SetActive(true);
        magic_circle.gameObject.SetActive(true);
        rune.gameObject.SetActive(true);
        bool trig1 = true;

		Vector3 CutIn_home = CutIn.anchoredPosition,Name_home = Name.anchoredPosition, Rune_Home = rune.anchoredPosition;
        float start = 0f, start2 = moveTime*0.2f , reversedMoveTime = 1f/moveTime , normalized = 0f;
		while(start2 < moveTime){
			start += Time.deltaTime;
			normalized = start*reversedMoveTime;
            magic_circle.Rotate(Vector3.forward,360f*start);
            rune.Rotate(Vector3.back,36f*start);
			//UI_Main_TitleImage.color = new Color(1f - 2*start,1f - 2*start,1f - 2*start,1f);
			if(start < moveTime) {
                CutIn.anchoredPosition = Vector3.Lerp(CutIn_home,CutIn_home + Vector3.right * 100f,normalized);
                rune.anchoredPosition = Vector3.Lerp(Rune_Home,Rune_Home + Vector3.left * 15f + Vector3.up * 10f,normalized);
                rune.localScale = Vector3.Lerp(Vector3.one,Vector3.one*2f,normalized);                                       
            }
            if(start > start2){
                if(trig1){
                    trig1 = false;
                    start2 = 0f;    
                }
                start2 += Time.deltaTime;
    			normalized = start2*reversedMoveTime;
                Name.anchoredPosition = Vector3.Lerp(Name_home,Name_home + Vector3.up * 600f,normalized);
            }
            yield return null;
		}
        rune.anchoredPosition = Rune_Home;
        rune.localScale = Vector3.one;
        rune.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.8f);

        CutIn.anchoredPosition = CutIn_home;
        CutIn.gameObject.SetActive(false);
        magic_circle.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        Name.anchoredPosition = Name_home;
        Name.gameObject.SetActive(false);
    }


    public IEnumerator UI_Slide_out(float moveTime){
        yield return null;
        //upper = Bosshp_toggle ? Boss : Score;
        upper.gameObject.SetActive(true);
        lower.gameObject.SetActive(true);

		Vector3 Upper_home = upper.anchoredPosition,Lower_home = lower.anchoredPosition;
        float start = 0f, reversedMoveTime = 1f/moveTime , normalized = 0f;
		while(start < moveTime){
			start += Time.deltaTime;
			normalized = start*reversedMoveTime;
			if(start < moveTime) {
                if(!UI_toggle){
                    upper.anchoredPosition = Vector3.Lerp(Upper_home,Upper_home + Vector3.up * 100f,normalized);
                    lower.anchoredPosition = Vector3.Lerp(Lower_home,Lower_home + Vector3.down * 100f,normalized);
                }else{
                    upper.anchoredPosition = Vector3.Lerp(Upper_home,Upper_home + Vector3.down * 100f,normalized);
                    lower.anchoredPosition = Vector3.Lerp(Lower_home,Lower_home + Vector3.up * 100f,normalized);
                }
            }
            yield return null;
		}

        // upper.anchoredPosition = Upper_home;
        // lower.anchoredPosition = Lower_home;
        if(!UI_toggle){
            upper.gameObject.SetActive(false);
            lower.gameObject.SetActive(false);
        }
        UI_toggle = !UI_toggle;
    }


    public IEnumerator UI_Black_out(float delay,float moveTime){
        yield return null;
        yield return new WaitForSeconds(delay);
        blackFade.gameObject.SetActive(true);        
        Color target = UI_Black ? new Color(0f,0f,0f,0f) : new Color(0f,0f,0f,1f);

        float start = 0f, reversedMoveTime = 1f/moveTime , normalized = 0f;
		while(start < moveTime){
			start += Time.deltaTime;
			normalized = start*reversedMoveTime;
			if(start < moveTime) {
                blackFade.color = Color.Lerp(blackFade.color, target, normalized);
            }
            yield return null;
		}
        if(UI_Black) blackFade.gameObject.SetActive(false);        
        UI_Black = !UI_Black;        
    }


    public void ToggleUpperBar(k514EnemyController e = null){
        target = e;
        if(!Bosshp_toggle){
            Boss.gameObject.SetActive(true);
            Score.gameObject.SetActive(false);
            StartCoroutine(BossHP_Progress);
        }else{
            Score.gameObject.SetActive(true);
            Boss.gameObject.SetActive(false);
            StopCoroutine(BossHP_Progress);            
        }
        Bosshp_toggle = !Bosshp_toggle;
    }

    IEnumerator BossHP_Progress_inner(){
        yield return null;
        Image[] image = Boss.GetComponentsInChildren<Image>();
        Debug.Log(image.Length);
        float reversedMaxHP = 1f/target.GetMaxHP();
        float nowPersent = 1f,compare = 0f;
        while(true){
            nowPersent = image[1].fillAmount;
            compare = target.GetHP() * reversedMaxHP;
            Debug.Log(target.GetHP()+" : "+compare);
            image[1].fillAmount = image[3].fillAmount = Mathf.Lerp(nowPersent,compare,Time.deltaTime);
            image[4].fillAmount = compare;
            if(compare>1f){
                image[5].fillAmount = image[3].fillAmount = image[1].fillAmount = compare - 1f;
            }else{
                image[5].fillAmount = 0f;
            }
            yield return null;
        }
    }

    public void UI_GetNumber(Image[] set, int number, int precise){

        int[] factor = new int[precise+1];
        for(int i = precise ; i >= 0 ; i--){
            factor[precise-i] = number/k514SystemManager.MathMgr.Pow(10,i);
            number -= factor[precise-i]*k514SystemManager.MathMgr.Pow(10,i);
            set[precise-i].sprite = this.number[factor[precise-i]];
        }
    }

    public void Power_UI_Update(){
        if(p_sComp.sPP>=127){
            for(int i = 0 ; i < 3 ; i++){
                power_ui[i].sprite = m_a_x[i];
            }
            return;
        }
        UI_GetNumber(power_ui,p_sComp.sPP,2);
    }
    
    public void Zanki_UI_Update(){
		if(p_sComp.ZANKI>9 || p_sComp.ZANKI<0 ) return;        
        UI_GetNumber(zanki_ui,p_sComp.ZANKI,0);
    }

    public void Rei_UI_Update(){
        float currentProgress = ReiUI.fillAmount;
        float updateProgress = p_sComp.SP;
        ReiUI.fillAmount = ReiMask.fillAmount = Mathf.Lerp(currentProgress,updateProgress,10f*Time.deltaTime);
    }

    public void Bomb_UI_Update(){
		if(p_sComp.BOMB>9) return;
        UI_GetNumber(bomb_ui,p_sComp.BOMB,0);
    }

    public void Score_UI_Update(){
		if(p_sComp.SCORE>999999999) return;
        UI_GetNumber(score_ui,p_sComp.SCORE,8);
    }

    public void InWater(){
        BlueFade.gameObject.SetActive(true);
    }

    public void OutWater(){
        BlueFade.gameObject.SetActive(false);
    }

    public void PumpBgmName(int index){
        StartCoroutine(UI_bgm_Slide_out(index,1f));
    }

    public IEnumerator UI_bgm_Slide_out(int index,float moveTime){
        yield return null;
        bgm_name[index].gameObject.SetActive(true);

		Vector3 Upper_home = bgm_name[index].anchoredPosition;
        float start = 0f, reversedMoveTime = 1f/moveTime , normalized = 0f;
		while(start < moveTime){
			start += Time.deltaTime;
			normalized = start*reversedMoveTime;
			if(start < moveTime) {
                     bgm_name[index].anchoredPosition = Vector3.Lerp(Upper_home,Upper_home + Vector3.right * 50f,normalized);
            }
            yield return null;
		}
        yield return new WaitForSeconds(4f);

        start = 0f;
        normalized = 0f;
		while(start < moveTime){
			start += Time.deltaTime;
			normalized = start*reversedMoveTime;
			if(start < moveTime) {
                     bgm_name[index].anchoredPosition = Vector3.Lerp(Upper_home + Vector3.right * 50f,Upper_home + Vector3.right * 200f,normalized);
            }
            yield return null;
		}
    
        bgm_name[index].anchoredPosition = Upper_home;
        bgm_name[index].gameObject.SetActive(false);
        

    }

}
