using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {
    // == setting

    // [HideInInspector]
    private bool facingRight = true; // For determining which way the player is currently facing.
    // [HideInInspector]
    [Header ("垂直向上起跳的力")]
    public float jumpForce = 300f; // Amount of force added when the player jumps.
    [Header ("玩家圖")]
    public GameObject skin;
    public bool is_faint = false;
    [Header ("確認玩家是否在地上")]
    public GameObject groundCheck;
    public LayerMask groundLayer;
    [Header ("是否踩在地板")]
    public bool grounded = false; // Whether or not the player is grounded.
    [Header ("玩家血量")]

    [Header("音效 跳躍")]
    public AudioSource audio_jump;
    [Header("音效 頭暈")]
    public AudioSource audio_faint;

    private int maxhp = 100;
    private int hp;

    private bool jump = false; // Condition for whether the player should jump.
    private float moveForce = 365f; // Amount of force added to move the player left and right.
    private float maxSpeed = 2f;
    public float moveSpeed = 0.05f; // 移動速度
    private float effect_time = 2f; // 1 秒
    private int die_time = 0;
    private bool is_die = false;

    private Animator animator;
    GameObject pool;
    private ObjectPool objPool;
    private bool isClone = false;
    private float _timer, _next, _ori_move_speed, _fixed_timer;
    private float _ketamine_next_time;
    private float life_time;

    class Effect {
        private string name;
        private float defaultTime;
        private float nextTime;
        private bool isTrigger;
        private bool triggered;

        public Effect (string _name, float _defaultTime, float _nextTime) {
            name = _name;
            defaultTime = _defaultTime;
            nextTime = _nextTime;
            isTrigger = false;
            triggered = false;
        }

        public float DefaultTime {
            get { return defaultTime; }
            set { defaultTime = value; }
        }

        public float NextTime {
            get { return nextTime; }
            set { nextTime = value; }
        }

        public bool IsTrigger {
            get { return isTrigger; }
            set { isTrigger = value; }
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public bool Triggered {
            get { return triggered; }
            set { triggered = value; }
        }
    }

    private IDictionary<string, Effect> effHash = new Dictionary<string, Effect> ();
    List<string> effHashkeyList;

    public bool IsClone {
        get { return isClone; }
        set { isClone = value; }
    }

    public float Life_time {
        get { return life_time; }
        set { life_time = value; }
    }

    public int Hp {
        get { return hp; }
        set { hp = value; }
    }

    public int Maxhp {
        get { return maxhp; }
        set { maxhp = value; }
    }

    public int Die_time {
        get { return die_time; }
        set { die_time = value; }
    }

    // Use this for initialization
    void Start () {
        animator = skin.GetComponent<Animator> ();

        pool = GameObject.Find ("pool");
        objPool = pool.GetComponent<ObjectPool> ();

        effHash["Bath_salts"] = new Effect ("Bath_salts", 1f, 0f); // 浴鹽：加速
        effHash["Heroin"] = new Effect ("Heroin", 1f, 0f); // 海洛因：減速
        effHash["Ketamine"] = new Effect ("Ketamine", 1f, 0f); // K 他命：損血
        effHash["MDMA"] = new Effect ("MDMA", 1f, 0f); // 搖頭丸：頭暈
        effHash["NewDrugs"] = new Effect ("NewDrugs", 1f, 0f); // 新藥物：相反方向
        effHash["Amphetamines"] = new Effect ("Amphetamines", 2f, 0f); // 安非他命：分身
        effHash["Health_bag"] = new Effect ("Health_bag", 0f, 0f);

        effHashkeyList = new List<string> (effHash.Keys);
        hp = maxhp = 100;
        _next = _timer = 0;
        _ori_move_speed = moveSpeed;
    }

    void Update () {
        if (isClone && _timer >= life_time) {
            // 如果是 克隆的人，時間到要消失
            objPool.Recovery ("Player", gameObject);
        }

        foreach (string key in effHashkeyList) {
            if (_timer > effHash[key].NextTime && effHash[key].IsTrigger) {
                effHash[key].IsTrigger = effHash[key].Triggered = false;

                if (key == "MDMA") {
                    animator.SetBool ("Faint", false);
                }
                if (key == "Bath_salts" || key == "Heroin") {
                    // 回復基礎速度
                    moveSpeed = _ori_move_speed;
                }
                if (key == "NewDrugs") {
                    Debug.Log("回覆方向 " + facingRight);
                }
            }
        }

        // 如果按下跳並且是在地上，才給他跳
        if (Input.GetButtonDown("Jump") && grounded) {
            audio_jump.PlayOneShot(audio_jump.clip);
            jump = true;
        }

        // 用碰撞是否有開啟當作是否設定過了
        if (hp <= 0 && !isClone && !is_die) {
            // 死亡，遊戲結束，進入endgame
            die_time = (int)_timer;
            is_die = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        _fixed_timer += Time.deltaTime;
        _timer += Time.deltaTime;

        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        // 設置偵測是否為地板，會從 gameObject.transform.position 發射一個設限到 groundCheck.transform.position, 第三個參數是設定能碰撞的 layer 層
        grounded = Physics2D.Linecast(transform.position, groundCheck.transform.position, 1 << LayerMask.NameToLayer ("Ground"));

        if (hp <= 0 && !isClone && !is_die)
            gameObject.GetComponent<Collider2D>().enabled = false;

        if (effHash["Ketamine"].IsTrigger) {
            // Ｋ他命 (血條逐漸變少)
            if (_fixed_timer >= _ketamine_next_time + 0.5f) {
                addHp(-10);
                _ketamine_next_time = _fixed_timer;
            }
        }
        if (!isClone && (effHash["MDMA"].IsTrigger || die_time > 0)) {
            // 搖頭丸 (走一下停一下) || 已經死掉了
            animator.SetBool ("Faint", true);
            audio_faint.PlayOneShot(audio_faint.clip, 0.5f);
            return;
        }

        // Cache the horizontal input.
        float h = Input.GetAxis ("Horizontal");

        if (effHash["NewDrugs"].IsTrigger)
            h = -h;

        // The Speed animator parameter is set to the absolute value of the horizontal input.
        animator.SetFloat ("Speed", Mathf.Abs (h));

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h * GetComponent<Rigidbody2D> ().velocity.x < maxSpeed) {
            // ... add a force to the player.
            // GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
            if (h > 0) gameObject.transform.position += new Vector3 (moveSpeed, 0, 0);
            if (h < 0) gameObject.transform.position += new Vector3 (-moveSpeed, 0, 0);
        }

        // If the input is moving the player right and the player is facing left...
        if (h > 0 /*&& !facingRight*/)
            Flip (1); // ... flip the player.
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (h < 0 /*&& facingRight*/)
            Flip (-1); // ... flip the player.

        if (jump) {
            // Add a vertical force to the player.
            GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0f, jumpForce));

            // Make sure the player can't jump again until the jump conditions from Update are satisfied.
            jump = false;
        }
    }

    /// <summary>
    /// OnCollisionEnter2D is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter2D (Collision2D other) {
        string collider_name = UtilTool.getOriName (other.gameObject.name);

        if (other.gameObject.tag == "stair") {
            addHp(2); // 踩一個階梯 +2 hp
        }

        if (other.gameObject.tag == "Drug") {
            effHash[collider_name].NextTime = System.Math.Max(effHash[collider_name].NextTime, _timer) + effHash[collider_name].DefaultTime;
            effHash[collider_name].IsTrigger = true;

            switch (collider_name) {
                case "Bath_salts":
                    // 浴鹽 (突然暴衝 0.5f)
                    moveSpeed *= 2;
                    effHash["Bath_salts"].Triggered = true;
                    break;
                case "Heroin":
                    // 海洛因 (行動緩慢)
                    moveSpeed /= 2;
                    effHash["Heroin"].Triggered = true;
                    break;
                case "Ketamine":
                    // Ｋ他命 (血條逐漸變少)
                    effHash["Ketamine"].Triggered = true;
                    break;
                case "MDMA":
                    // 搖頭丸 (走一下停一下)
                    effHash["MDMA"].Triggered = true;
                    break;
                case "NewDrugs":
                    // 新興藥物 (相反方向)
                    if (effHash["NewDrugs"].Triggered) {
                        // effHash["NewDrugs"].NextTime += effHash["NewDrugs"].DefaultTime;
                    } else {
                        effHash["NewDrugs"].Triggered = true;
                        facingRight = false;
                    }
                    break;
                case "Amphetamines":
                    // 安非他命 (出現分身，且分身不受控)
                    GameObject playerClone = objPool.ReUse ("Player", transform.position + new Vector3 (Random.Range (-1, 1), 0, 0), transform.rotation);
                    Debug.Log ("playerClone:" + playerClone.name);
                    playerClone.GetComponent<PlayerCtrl> ().IsClone = true;
                    playerClone.GetComponent<PlayerCtrl> ().moveSpeed *= -1;
                    playerClone.GetComponent<PlayerCtrl> ().facingRight = !playerClone.GetComponent<PlayerCtrl> ().facingRight;
                    playerClone.GetComponent<PlayerCtrl> ().Life_time = effHash["Amphetamines"].DefaultTime;
                    playerClone.name = "NonPlayer";
                    break;

                case "Health_bag":
                    addHp(40);
                    break;
            }
        }
    }

    public void Flip (int dist) {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        // theScale.x *= -1;
        if (dist > 0) theScale.x = Mathf.Abs(theScale.x);
        else theScale.x = -Mathf.Abs(theScale.x);

        transform.localScale = theScale;
    }

    public void addHp(int patch) {
        hp = Mathf.Clamp(hp + patch, 0, 100);
    }
}