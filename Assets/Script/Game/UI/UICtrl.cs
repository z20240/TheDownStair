using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour {
    public Image blood;
    public Text Hp;
    public Text Score;
    GameCtrl gameCtrl;
    PlayerCtrl playerCtrl;
	// Use this for initialization
	void Start () {
        gameCtrl = GameObject.Find("gameCtrl").GetComponent<GameCtrl>();
        playerCtrl = GameObject.Find("player").GetComponent<PlayerCtrl>();
	}

	// Update is called once per frame
	void Update () {
        blood.transform.localPosition = new Vector3(2 * (playerCtrl.Hp - playerCtrl.Maxhp), 0, 0);
        Hp.text = "Hp : " + playerCtrl.Hp + "%";
        Score.text = gameCtrl.Floor + " FLOOR";
	}
}
