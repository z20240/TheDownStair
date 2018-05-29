using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stairSpawner : MonoBehaviour {
    GameObject pool;
    private ObjectPool objPool;
    private float _timer;
    private float _next_spawntime;
    private float spawnTime = 1.2f; // 產生階梯的時間
    private int playerHeight = 2;
    private GameObject last_stair;

    private string[] stair = {
        "stone_stair_1",
        "stone_stair_2",
    };

    // Use this for initialization
    void Start () {
        pool = GameObject.Find("pool");
        objPool = pool.GetComponent<ObjectPool>();

        _next_spawntime = spawnTime;
    }

    // Update is called once per frame
    void Update () {
        _timer += Time.deltaTime; //時間增加

        if ( _timer <= _next_spawntime)
            return;

        if (last_stair && last_stair.transform.position.y < transform.position.y + playerHeight)
            // 確認每個生成的階梯間隔都可以塞人
            return;

        Debug.Log("[Spawn ]timer:" + _timer + " next_spawntimer:" + _next_spawntime);
        int rnd = Random.Range(0, 2);
        last_stair = objPool.ReUse( stair[rnd], transform.position + new Vector3(Random.Range(-6f, 6f), 0, 0), transform.rotation);
        _timer = 0;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            other.GetComponent<PlayerCtrl>().Hp = 0;
        }
    }
}
