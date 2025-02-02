﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CtrlPanel : MonoBehaviour {


    public UnityEvent OnHover = new UnityEvent();
    private GameCtrl gameCtrl;

    bool is_hover = false;
    float _time, exec_time = 0.01f;
    bool restart_flag = false, continue_flag = false, leave_flag = false;
    Vector3 ori_scale;

    string[] btn_name = { "Restart", "Continue", "Leave"};

	// Use this for initialization
	void Start () {
        gameCtrl = GameObject.Find("gameCtrl").GetComponent<GameCtrl>();
        ori_scale = gameObject.transform.localScale;

		gameObject.GetComponent<Button>().onClick.AddListener(() => {
            switch (gameObject.name) {
                case "Continue":
                    gameCtrl.Continue();
                    break;
                case "Restart":
                    gameCtrl.Continue();
                    SceneManager.LoadScene("start");
                    break;
                case "Leave":
                    Application.Quit();
                    break;
            }
        });
	}

	// Update is called once per frame
	void Update () {
        // if (gameObject.name == btn_name[Mathf.Clamp(gameCtrl.ChooseBtn, 0, 2)]) {
        //     if (gameObject.transform.localScale == ori_scale)
        //         gameObject.transform.localScale *= 1.2f;
        // } else
        //     gameObject.transform.localScale = ori_scale;

        // if (gameCtrl.ChooseBtn == 0 && Input.GetButtonUp("Jump")) {
        //     gameCtrl.Continue();
        //     SceneManager.LoadScene("start");
        // }
        // if (gameCtrl.ChooseBtn == 1 && Input.GetButtonUp("Jump")) {
        //     gameCtrl.Continue();
        // }
        // if (gameCtrl.ChooseBtn == 2 && Input.GetButtonUp("Jump")) {
        //     Application.Quit();
        // }


        if (is_hover) {
            _time += Time.deltaTime;

            if (_time >= exec_time) {
                is_hover = false;
                OnHover.Invoke();
            }
        }
	}

    // 當滑鼠進入時
    public void OnMouseEnter() {
        is_hover = true;
        gameObject.transform.localScale *= 1.2f;
    }

    void OnMouseOver() {
    }

    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    public void OnMouseExit() {
        is_hover = false;

        gameObject.transform.localScale = ori_scale;
    }
}
