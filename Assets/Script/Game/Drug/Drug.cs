using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drug : MonoBehaviour {

    GameObject pool;
    private ObjectPool objPool;

	// Use this for initialization
	void Start () {
        pool = GameObject.Find("pool");
        objPool = pool.GetComponent<ObjectPool>();
	}

	// Update is called once per frame
	void Update () {

	}

    /// <summary>
    /// OnCollisionEnter2D is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
            objPool.Recovery(UtilTool.getOriName(gameObject.name), gameObject);
    }
}
