using System.Collections;    //引用 系統.集合 API - 集合 與協同程序
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(0, 1000)]
    public float Speed = 10.5f;
    [Header("跳越高度"), Range(0, 3000)]
    public int Jumph = 100;
    [Range(0, 200)]
    public float HP = 100;
    [Header("是否在地板上"), Tooltip("這是否在地板上")]
    public bool isGrounded = false;
    [Header("子彈"), Tooltip("這是子彈")]
    public GameObject Bullet;
    [Header("子彈速度"),Range(0, 5000)]
    public int BulletSpeed = 800;
    [Header("開槍音效"), Tooltip("這是開槍音效")]
    [Header("判斷地板碰撞的位移與半徑")]
    public Vector3 groundOffset;
    public float groundRafius = 0.2f;
    [Header("子彈生成位置")]
    public Vector3 posBullet;

    public AudioClip BiuAudio;
    private AudioSource Aud;
    private Rigidbody2D rig;
    private Animator Ani;
    private ParticleSystem ps;

    /// <summary>
    /// 圖片 : 血條
    /// </summary>
    private Image imgHP;
    /// <summary>
    /// 文字 :生命
    /// </summary>
    private Text textHP;
    private float hpMax;
    #endregion

    #region 事件

    private CanvasGroup groupFinal;
    /// <summary>
    /// 結束標題
    /// </summary>
    private Text textFinalTitle;


    private void Start()
    {
        //利用程式取的元件
        //傳回元件 取得元件<元件名稱>() - <泛型>
        //取得跟此腳本同一層的原件
        rig = GetComponent<Rigidbody2D>();
        Ani = GetComponent<Animator>();
        Aud = GetComponent<AudioSource>();

        //粒子系統 = 變形元件.搜尋子物件("物件特效")
       ps = transform.Find("集氣特效").GetComponent<ParticleSystem>();

        //2D物理.忽略圖層碰撞 (圖層1 .圖層2 .是否要忽略)
        Physics2D.IgnoreLayerCollision(9, 10, true);

        //遊戲物件 .尋找("物件名稱 - 絕對不要有相同的名稱")
       imgHP = GameObject.Find("血條").GetComponent<Image>();
        textHP = GameObject.Find("生命").GetComponent<Text>();
        textHP.text = life.ToString();
        hpMax = HP;

        groupFinal = GameObject.Find("結束畫面").GetComponent<CanvasGroup>();
        textFinalTitle = GameObject.Find("結束標題").GetComponent<Text>();
    }

    //一秒約執行60次
    private void Update()
    {
        Move();
        Jump();
        Fire();
    }

    private void FixedUpdate()
    {
        MoveFixed();
    }


    //繪製圖式 - 輔助編輯時的圖形線條
    private void OnDrawGizmos()
    {
        // 1. 指定顏色
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        // 2. 繪製圖形
        //transform 可以抓到此腳本同一層的變形元件
        //繪製球體(中心點.半徑)
        //物件的右方 X 軸 : transform.right
        //物件的右方 Y 軸 : transform.up
        //物件的右方 Z 軸 : transform.forward
        Gizmos.DrawSphere(transform.position + transform.right * groundOffset.x + transform.up * groundOffset.y , groundRafius);

        //先指定顏色在畫圓型
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawSphere(transform.position + transform.right * posBullet.x + transform.up * posBullet.y, 0.1f);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EatProp(collision.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "死亡區域") HP = 0;
    }
   
    //與粒子碰撞會執行的事件
    //1.要有碰撞器
    //2.粒子設定
    //2-1 Collision 勾選
    //2-2 類型 Type :World
    //2-3 模式 Mode : 2D
    //2-4. 勾選傳送訊息 Send Collision Messages
    private void OnParticleCollision(GameObject other)
    {
        Hit(other.GetComponent<ParticleSystemDate>().attack);
    }
    #endregion

    #region 方法
    
    /// <summary>
    /// 物理移動
    /// </summary>
    private void MoveFixed()
    {
        //print("水平的值: " + h);
        //2. 使用左右鍵的資訊控制腳色
        //剛體.加速度 = 二維向量(水平 * 速度 * 一幀的時間, 指定回原本的 Y 軸加速度 )
        //一幀的時間 - 解決不同效能的裝置速度差問題
        rig.velocity = new Vector2(h * Speed * Time.deltaTime, rig.velocity.y);

    }

    private float h;
    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        //1. 要抓到玩家按下左右鍵的資訊
        h = Input.GetAxis("Horizontal");
        
        //如果按下 D 面向右邊 0 0 0
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.eulerAngles = Vector3.zero;
        }    
        //否則 如果按下A面向左邊 0 180 0
        else if (Input.GetKeyDown(KeyCode.A))
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        //設定動畫
        //水平值 不等於零 布林值 打勾
        //水平值 等於零 布林值 取消

        Ani.SetBool("走路開關", h != 0);



    }
    /// <summary>
    /// 跳躍
    /// </summary>
    private void Jump()
    {
        //如果 玩家 按下 空白鍵 就 往上跳躍
        //判斷式C#
        //傳回值為布林值的方法可以當成布林值使用
        //@ 判斷布林值是否等於true寫法
        //1. isGrounded == true(原本寫法)
        //2. isGrounded (簡寫)
        if (Input.GetKeyDown(KeyCode.Space)&& isGrounded)
        {
            //剛體.添加推力(二維向量)
            rig.AddForce(new Vector2(0,Jumph));
        }
        //碰到的物件= 2D物理.覆蓋圓形(中心點.半徑)
        //圖層語法 : 1<< 圖層邊號(LayerMask int)
        Collider2D hit = Physics2D.OverlapCircle(transform.position + transform.right * groundOffset.x + transform.up * groundOffset.y, groundRafius, 1 << 8);

       // print("碰到的物件:" + hit.name);

        //如果碰到的物件 存在 並且 碰到的物件名撐 等於 地板 就代表在地板上
        //並且&& (SHIFT+7)
        //等於 ==
        //或者 || (SHIFT + \)
        //或者 名稱 等於 跳台

        if (hit && (hit.name =="地板" || hit.name == "跳台"))
        {
            isGrounded = true;
        }
        //否則不在地板上
        //否則 else
        //語法: else{ 程式區塊 } - 僅能寫在 if 下方
        else
        {
            isGrounded = false;
        }
    }
    /// <summary>
    /// 紀錄按下左件的計時器
    /// </smmary>
    private float timer;

    /// <summary>
    /// 攻擊力
    /// </summary>
    private float attack = 10;

    /// <summary>
    /// 開槍
    /// </summary>
    private void Fire()
    {
        //如果玩家按下左鍵 就開槍 - 動畫與音效 發射子彈
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ani.SetTrigger("攻擊觸發");
            ps.Play();              //播放集氣
           
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            //累加+=
            timer += Time.deltaTime;
            //print("按住左鍵的時間" + timer);
        }
        // 放開左鍵
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            ps.Stop();       //停止集氣
            Aud.PlayOneShot(BiuAudio, 0.5f);

            // Object.Instantiate(Bullet);//原始寫法
            //Quaternion 四位元 -角度
            //Quaternion.identity 零角度
            GameObject temp = Instantiate(Bullet, transform.position + transform.right * posBullet.x + transform.up * posBullet.y, Quaternion.identity);      //簡寫

            //暫存物件 . 取得原件 <2D鋼體>().添加推力 (腳色的前方 * 子彈速度)
            temp.GetComponent<Rigidbody2D>().AddForce(transform.right * BulletSpeed);
            //暫存物件.添加元件<子彈>();
            temp.AddComponent<Bullet>();
            //刪除 (物件,延遲秒數)
            Destroy(temp, 2f);
            //讓子彈的角度跟玩家目前的角度相同 - 子彈角度問題
            //取得例子的渲染元件
            ParticleSystemRenderer render = temp.GetComponent<ParticleSystemRenderer>();
            //渲染的翻面 = 腳色的高度 - ? : 三元運算
            render.flip = new Vector3(transform.eulerAngles.y == 0 ? 0 : 1, 0, 0);

            //計時器 = 數學 .夾住 (計時器 .最小 .最大)
            timer = Mathf.Clamp(timer, 0, 5);

            //子彈的攻擊力 = 攻擊力 加上 四捨五入(計時器)*2

            temp.GetComponent<Bullet>().attack = attack + (Mathf.Round(timer) + 5) * 4;
            //集氣:調整子彈尺寸
            //temp.transform.lossyScale = Vector3.one; //lossyScale 為唯讀 Read Only - 不能指定值
            temp.transform.localScale = Vector3.one + Vector3.one * timer;

            //計時器歸零
            timer = 0;
        }
                }
    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="damage">造成的傷害</param>
    public void Hit(float damage)
    {
        HP -= damage;
        imgHP.fillAmount = HP / hpMax;
        if (HP <= 0) Dead();
    }

    //靜態 static
    //1. 靜態欄位重新仔入後不會還原為預設值
    //2. 靜態欄位不會顯示在屬性面板上
    [Header("生命數量")]
    public int life = 3;

    /// <summary>
    /// 死亡
    /// </summary>
    /// <returns>是否死亡</returns>
    private bool Dead()
    {
        //如果 尚未死亡 並且 血量 低於等於 零 才可以執行 死亡
        if (!Ani.GetBool("死亡開關") && HP <= 0)
        {
            Ani.SetBool("死亡開關", HP <= 0);
            life--;                            //生命遞減
            textHP.text = life.ToString();     //更新生命數量

            if (life >= 0) Invoke("Replay", 2f);    //如果生命大於等於0就重新遊戲
            else StartCoroutine(GameOver());        // 否則就啟動協同程序 遊戲結束

  
        }
            return HP <= 0;    
    }

    //IEnumerator 允許傳回時間 必須有 yield 讓步
   public IEnumerator GameOver(string finalTitle = "GameOver")
    {
        textFinalTitle.text = finalTitle;
        while (groupFinal.alpha < 1)                    //當透明度 < 1 時執行
        {
            groupFinal.alpha += 0.5f;                     //遞增透明度0.05
            yield return new WaitForSeconds(0.02f);      // 間隔0.02秒
        }

        groupFinal.interactable = true;             //允許互動
        groupFinal.blocksRaycasts = true;             //允許滑鼠遮擋
    
    
    }

    /// <summary>
    /// 重新遊戲
    /// </summary>
    private  void Replay()
    {
        SceneManager.LoadScene("遊戲畫面");
    }

    /// <summary>
    /// 吃道具
    /// </summary>
    /// <param name="pro">道具名稱</param>
    private void EatProp (GameObject prop)
        {
         if(prop.tag == "道具")
        {
            //字串 API Remove(編號) 刪除包含只訂編號後面的字串
            switch (prop.name.Remove(2))
            {
                case "補血":
                    HP += 30;
                    HP = Mathf.Clamp(HP, 0, hpMax);
                    imgHP.fillAmount = HP / hpMax;
                    break;
            }

            Destroy(prop);
        }
        }


#endregion
}
