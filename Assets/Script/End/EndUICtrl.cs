using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndUICtrl : MonoBehaviour {
    public GameObject end_ui_default;
    public GameObject end_ui_quit;
    public GameObject end_ui_restart;
    public UnityEvent OnHover = new UnityEvent();


    bool is_hover = false;
    float _time, exec_time = 0.01f;

    private int chooseBtn = 1;


	// Use this for initialization
	void Start () {
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

        float h = Input.GetAxis ("Horizontal");
        if (h == 1) chooseBtn = (chooseBtn + 1) % 2 + 1;
        if (h == -1) chooseBtn = ((chooseBtn - 1) < 0 ? 2 : (chooseBtn - 1)) % 2 + 1;

        if (chooseBtn == 1) {
            end_ui_default.SetActive(false);
            end_ui_restart.SetActive(true);
            end_ui_quit.SetActive(false);
        } else if (chooseBtn == 2) {
            end_ui_default.SetActive(false);
            end_ui_restart.SetActive(false);
            end_ui_quit.SetActive(true);
        }

        if (end_ui_quit.activeSelf && Input.GetButtonDown("Jump")) {
            Application.Quit();
        } else if (end_ui_restart.activeSelf && Input.GetButtonDown("Jump")) {
            SceneManager.LoadScene("start");
        }
	}

    // 當滑鼠進入時
    public void OnMouseEnter() {
        is_hover = true;
        if (gameObject.name == "Restart") {
            end_ui_default.SetActive(false);
            end_ui_restart.SetActive(true);
            end_ui_quit.SetActive(false);
        }

        if (gameObject.name == "Leave") {
            end_ui_default.SetActive(false);
            end_ui_restart.SetActive(false);
            end_ui_quit.SetActive(true);
        }
    }

    void OnMouseOver() {
    }

    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    public void OnMouseExit() {
        is_hover = false;

        end_ui_default.SetActive(true);
        end_ui_restart.SetActive(false);
        end_ui_quit.SetActive(false);
    }

    public void OnMouseClick() {
        if (gameObject.name == "Restart") {
            SceneManager.LoadScene("start");
        }
        else {
            Application.Quit();
        }
    }

    public void Do_something() {
    }
}
