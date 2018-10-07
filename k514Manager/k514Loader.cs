using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class k514Loader : MonoBehaviour {

    // 로드할 씬 이름
    public static string sceneName = "";
    private AsyncOperation  async_operation;
    private float           nowTime;
    private bool            IsLoad = false;

	// Use this for initialization
	void Start () {
        nowTime = Time.time;
	}

    // 비동기식 로딩 쓰레드
    public IEnumerator LoadScene(string name)
    {
        bool IsDone = false;

        if(!IsDone)
        {
            IsDone = true;
            async_operation = SceneManager.LoadSceneAsync(name);
            async_operation.allowSceneActivation = false;
        }
        yield return null;
    }
	
	// Update is called once per frame
	void Update () {
        Time.timeScale = 1f;
        
        if(!IsLoad && Time.time - nowTime > 1f)
        {
            // 비동기식 로딩 쓰레드 호출
            IsLoad = true;
            StartCoroutine(LoadScene(sceneName));
			nowTime = Time.time;
        }

        if(Time.time - nowTime > 1f)
        {
            // 다음 씬으로의 전환 활성화
            async_operation.allowSceneActivation = true;
        }
	}
}
