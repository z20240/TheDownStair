using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairMoveUp : MonoBehaviour {

    private GameObject GameCtrl;
    private float stair_speed;
    GameCtrl gameCtrl;

    public float Stair_speed {
        get { return stair_speed; }
        set { stair_speed = value; }
    }

    // Use this for initialization
    void Start () {
        GameCtrl = GameObject.Find("gameCtrl");
        gameCtrl = GameCtrl.GetComponent<GameCtrl>();

        stair_speed = gameCtrl.Speed_step;
	}

	// Update is called once per frame
	void Update () {
        if (gameCtrl.IsPause)
            return;

        // Debug.Log("gameCtrl " + stair_speed + "  pos " + gameObject.transform.position);
	}

    // Update is called once per frame
    void FixedUpdate () {
        gameObject.transform.position += Vector3.up * stair_speed;
    }
}
