using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScencCtrl : MonoBehaviour {
    GameObject start_ui_1;
    public UnityEvent OnHover = new UnityEvent();

    bool is_hover = false;
    float _time, exec_time = 0.01f;

	// Use this for initialization
	void Start () {
        start_ui_1 = GameObject.Find("image_1");
        Screen.SetResolution(1920, 1080, true);
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

        // 開始遊戲
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Jump"))
            SceneManager.LoadScene("game");

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("Cancel")
        )
            Application.Quit();
	}

    // 當滑鼠進入時
    public void OnMouseEnter() {
        is_hover = true;
        start_ui_1.SetActive(false);
    }

    void OnMouseOver() {
    }

    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    public void OnMouseExit() {
        is_hover = false;
        start_ui_1.SetActive(true);
    }

    public void OnMouseClick() {
        SceneManager.LoadScene("game");
    }

    public void Do_something() {
    }
}
