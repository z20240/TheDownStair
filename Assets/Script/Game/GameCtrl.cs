using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCtrl : MonoBehaviour {
    private float speed_step = 0.01f; // 初始移動速度
    private float speed_add = 0.01f; // 每十秒增加的速度
    private int floor = 0; // 樓層
    private float _time = 1;
    private float _game_time = 0;
    private bool isPause = false;
    private int chooseBtn = 2;

    private stairSpawner stairSpawner;
    private PlayerCtrl playerCtrl;

    public GameObject CtrlPannel;

    public float Speed_step {
        get { return speed_step; }
        set { speed_step = value; }
    }

    public bool IsPause {
        get { return isPause; }

        set { isPause = value; }
    }

    public int Floor {
        get { return floor; }
        set { floor = value; }
    }

    public int ChooseBtn {
        get { return chooseBtn; }
        set { chooseBtn = value; }
    }

    // Use this for initialization
    void Start () {
        stairSpawner = GameObject.Find("stairSpawner").GetComponent<stairSpawner>();
        playerCtrl = GameObject.Find("player").GetComponent<PlayerCtrl>();
        Screen.SetResolution(1920, 1080, true);
	}

	// Update is called once per frame
	void Update () {
        _time += Time.deltaTime;
        _game_time += Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("Cancel")) {
            if (!isPause) {
                Pause();
            } else {
                Continue();
            }
        }

        if (isPause) {
            float h = Input.GetAxis ("Horizontal");
            if (h == 1) chooseBtn = (chooseBtn + 1) % 3 + 1;
            if (h == -1) chooseBtn = ((chooseBtn - 1) < 0 ? 3 : (chooseBtn - 1)) % 3 + 1;
        }

        if (((int)_time) % 6 == 0) {
            Debug.Log(" == time: " + _time);
            _time = 1;
            speed_step += speed_add;
            playerCtrl.jumpForce += 10; // 提升玩家起跳的力道

            // 更新已經生成的石梯的移動速度，以避免後進的石梯追過前面的石梯
            Add_all_stair_speed();
        }

        // 死亡後 1.5 秒 切換畫面
        if (playerCtrl.Die_time > 0 && _game_time >= playerCtrl.Die_time + 1.5) {
            SceneManager.LoadScene("end");
        }

        // 計算下樓的樓層 (每 1 個階梯 1 樓)
        floor = stairSpawner.Stair_count / 1;
	}

    public void Pause() {
        //時間暫停
        Time.timeScale = 0f;
        isPause = true;
        CtrlPannel.SetActive(true);
    }

    public void Continue() {
        isPause = false;
        Time.timeScale = 1f;
        CtrlPannel.SetActive(false);
    }

    void Add_all_stair_speed() {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("stair");

        for(var i = 0 ; i < gameObjects.Length ; i ++) {
            gameObjects[i].GetComponent<StairMoveUp>().Stair_speed = speed_step;
        }
    }
}
