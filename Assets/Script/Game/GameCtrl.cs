using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour {
    private float speed_step = 0.02f; // 初始移動速度
    private float speed_add = 0.02f; // 每十秒增加的速度
    private int floor = 0; // 樓層
    private float _time = 1;
    private bool isPause = false;


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

    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void Update () {
        _time += Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (!isPause) {
                Pause();
            } else {
                Continue();
            }
        }

        if (((int)_time) % 10 == 0) {
            Debug.Log(" == time: " + _time);
            _time = 1;
            speed_step += speed_add;
            floor += 1;

            // 更新已經生成的石梯的移動速度，以避免後進的石梯追過前面的石梯
            Add_all_stair_speed();
        }
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
