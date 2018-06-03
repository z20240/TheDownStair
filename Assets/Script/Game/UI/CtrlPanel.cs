using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CtrlPanel : MonoBehaviour {


    public UnityEvent OnHover = new UnityEvent();

    bool is_hover = false;
    float _time, exec_time = 0.01f;
    Vector3 ori_scale;

	// Use this for initialization
	void Start () {
        ori_scale = gameObject.transform.localScale;
		gameObject.GetComponent<Button>().onClick.AddListener(() => {
            switch (gameObject.name) {
                case "Continue":
                    GameObject.Find("gameCtrl").GetComponent<GameCtrl>().Continue();
                    break;
                case "Restart":
                    GameObject.Find("gameCtrl").GetComponent<GameCtrl>().Continue();
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
