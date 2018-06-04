using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("施予一個 250 的力");
            other.gameObject.transform.position = other.gameObject.transform.position + new Vector3(0, -1f, 0);

            other.GetComponent<PlayerCtrl>().addHp(-40);
        }
    }
}
