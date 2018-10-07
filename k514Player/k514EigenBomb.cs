using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514EigenBomb : MonoBehaviour {

    public Transform[] ommyo;
    public RectTransform CutIn, SpellCard;
    private Transform player = null, Lamullet = null, Ramullet = null;
    private Transform[] effects;
    private k514PlayerController p_sComp = null;
    private k514MusouFuuin[] ch_sComp = null;
    private bool isActive = false,checker = false,trogig = false;

    void Start(){
        this.gameObject.SetActive(false);

        player = GameObject.Find("Player").transform;
        p_sComp = player.GetComponent<k514PlayerController>();
        Lamullet = GameObject.Find("ommyodama Left").transform;
        Ramullet = GameObject.Find("ommyodama Right").transform;        
    
        effects = new Transform[ommyo.Length];
        for(int i = 0 ; i < ommyo.Length ; i++){
            effects[i] = ommyo[i].GetChild(2);
        }
        ch_sComp = new k514MusouFuuin[8];
        for(int i = 0 ; i < ommyo.Length ; i++){
            ch_sComp[i] = ommyo[i].GetComponent<k514MusouFuuin>();
            ommyo[i].GetComponent<Collider>().enabled = false;
        }
    }

    void Update(){
        if(p_sComp.BombTrig && trogig){
            k514SystemManager.BulletMgr.KillAllBulletNoEf();
        }
        if(isActive){
            checker = false;
            for(int i = 0 ; i < 8 ; i ++){
                if(!ch_sComp[i].isAlive()){
                    effects[i].gameObject.SetActive(false);
                    ommyo[i].gameObject.SetActive(false);
                    ommyo[i].localScale = Vector3.one*2f;
                    ommyo[i].GetComponent<Collider>().enabled = false;
                }else{
                    checker = true;
                }
            }
            if(!checker){
                EndProcess();
            }
        }
    }

    public IEnumerator Musou_Fuuin(){
        yield return null;
        p_sComp.animator.SetBool("Bomb",true);
        this.gameObject.SetActive(true);
        StartCoroutine(k514SystemManager.UI_SpecaMgr.SpellCardAnimating(CutIn,SpellCard,2f));
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.SPELL_ACTIVE);
        k514SystemManager.BulletMgr.KillAllBullet();
        trogig = true;
        // 샷 금지
        p_sComp.ShotableTrig = false;
    
        // 가지고 다니던 양쪽 음양옥이 각각 4개씩 분할됨.
        for(int i = 0 ; i < ommyo.Length ; i++){
            ommyo[i].gameObject.SetActive(true);
            ommyo[i].position = (i >= ommyo.Length/2) ? Lamullet.position : Ramullet.position;
        }

        Lamullet.gameObject.SetActive(false);
        Ramullet.gameObject.SetActive(false);

        float step = 0.15f;
        bool accelTrig = false;
        int j = 0;
        int expand_bound = 30, end_Bound = 60 , break_bound = 90;
        float startRadius = 1f, endRadius = 6f, expand_step = (endRadius - startRadius)/(end_Bound - expand_bound);
        float startScale = 2f, endScale = 6f, scaleStep = (endScale - startScale)/(end_Bound - expand_bound);
        int[] perm = k514SystemManager.MathMgr.Permutation(8);
        Vector3 normal;

        while(j<break_bound){
            if(accelTrig && step < 0.3f){
                p_sComp.animator.SetBool("Bomb",false);
                step += 0.02f;
            }

            // expand
            if(j > expand_bound){
                for(int i = 0 ; i < ommyo.Length ; i++){
                    ommyo[i].position = k514SystemManager.InteractMgr.GetPlayerHitVector() + (ommyo[i].position-(k514SystemManager.InteractMgr.GetPlayerHitVector())).normalized * (startRadius + expand_step*(j-expand_bound));
                    ommyo[i].localScale = Vector3.one*(startScale + scaleStep*(j-expand_bound));
                }
            }

            if(j == expand_bound + 10){
                for(int i = 0 ; i < ommyo.Length ; i++){
                    k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.KIRA);
                    effects[i].gameObject.SetActive(true);
                }
            }

            // rotate
            for(int i = 0 ; i < ommyo.Length/2 ; i++){
                if (j*step >= i) {
                    ommyo[i].RotateAround(transform.position, Vector3.up, 45*step);
                    ommyo[ommyo.Length - 1 - i].RotateAround(transform.position, Vector3.up, 45*step);                
                    if(i == ommyo.Length/2 - 1) accelTrig = true;
                }
            }
            j++;
            yield return null;
        }


        // animate stop , make random rotation transform
        // activate patterns
        
        for(int i = 0 ; i < 8 ; i++){
            ommyo[perm[i]].Rotate(new Vector3(Random.Range(0f,180f),Random.Range(0f,180f),Random.Range(0f,180f)),Random.Range(0f,180f));
            ommyo[perm[i]].GetComponent<Collider>().enabled = true;
            ch_sComp[perm[i]].InitPattern(i);
            k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.MUSOU_START,true);
            yield return new WaitForSeconds(Random.Range(0.05f,0.2f));
        }
        isActive = true;

    }

    void EndProcess(){
        isActive = false;
        trogig = false;
        p_sComp.BombTrig = false;
        p_sComp.ShotableTrig = true;
        Lamullet.gameObject.SetActive(true);
        Ramullet.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

}
