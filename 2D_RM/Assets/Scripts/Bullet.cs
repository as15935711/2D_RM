using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour

{
    /// <summary>
    /// 攻擊力
    /// </summary>
    public float attack;

    private void Start()
    {
        //讓子彈不要互相碰撞
        Physics2D.IgnoreLayerCollision(10, 10, true);
        //讓子彈與道具不要互相碰撞
        Physics2D.IgnoreLayerCollision(10, 11, true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
      //如果 碰到物件的標籤 等於 敵人
      if(collision.gameObject.tag == "敵人")
        {
            //取得 敵人 腳本 並呼叫 受傷方法
            collision.gameObject.GetComponent<Enemy>().Hit(attack);
        }
      //碰撞到任何物件 都要消除
        Destroy(gameObject);
    }
}
