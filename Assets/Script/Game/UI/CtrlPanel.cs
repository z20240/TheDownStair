using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CtrlPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
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

	}
}
