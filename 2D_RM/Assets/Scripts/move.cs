using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    #region 欄位
    [Header("汽車的 cc 數")]
    [Tooltip("這是汽車的 cc 數")]
    [Range(1000, 5000)]
    public int cc = 2000;
    [Header("汽車的重量"), Tooltip("這是汽車的重量。"), Range(0.5f, 10)]
    public float weight = 1.5f;
    [Header("汽車的品牌")]
    public string brand = "BMW";
    [Header("有沒有天窗")]
    public bool hasWindow = true;


    public Color color;
    public Color red = Color.red;
    public Color mycolor = new Color(0.3f, 0, 0.6f);
    public Color mycolor2 = new Color(0, 0.5f, 0.5f, 0.5f);

    //座標二維-四維 Vector2 Vector3 Vector4
    public Vector2 v2;
    public Vector2 v2zero = Vector2.zero;
    public Vector2 v2one = Vector2.one;
    public Vector2 v2my = new Vector2(7, 9);

    public Vector3 v3 = new Vector3(1, 2, 3);
    public Vector4 v4 = new Vector4(1, 2, 3, 4);

    public KeyCode key1;
    public KeyCode key2 = KeyCode.A;      //不指定為 None (無)
    public KeyCode key3 = KeyCode.Mouse0; //左0 右1 滾輪2
    public KeyCode key4 = KeyCode.Joystick1Button0;

    //遊戲物件 與元件
    //遊戲物件 GameObject
    public GameObject obj1;
    public GameObject obj2;
    //元件 Component - 屬性面板上可摺疊的
    public Transform tra;
    public SpriteRenderer spr;
    #endregion
    #region 事件
    public int number = 1;
    public bool test = false;
    public string prop = "紅色藥水";
    //事件 :在特定時間點會被執行的方法
    //Unity 提供的事件 : 開始 更新
    //開始事件 :播放遊戲後執行一次
    //應用 :數值初始畫，例如:遊戲一開始多少金幣或生命值等等...
    private void Start()
    {
        //print(任何資料) - 輸出資料到 Console 儀表板上
        print("我是開始事件唷~");

        //欄位存取
        //取得
        //語法 : 欄位名稱
        print("取得欄位:" + number);
        //存放
        //語法 : 欄位名稱 指定 值;
        test = true;
        print("存放欄位後的結果:" + test);

        prop = "藍色藥水";
        print("存放後的道具名稱" + prop);

        // 呼叫方法
        //方法名稱();

        Test();
        //傳回方法 :
        //傳回類型 名稱=傳回方法();
        int t = Ten();
        print("傳回方法的結果 : " + t);

        Drive50();
        //呼叫方法要有相同數量的參數
        //有預設值得參數為<選填是參數>
        Drive(200, "咻咻咻");
        Drive(800, "咚咚咚");
        //有多個選填式參數
        Drive(50, "閃電");                 //錯誤 - 把特效放在音效上
        Drive(50, effect: "閃電");        //正確 - 指定特效參數

        float bmi = BMI(1.78f, 71);
        print("計算後的BMI:" + bmi);
    }

    //更新事件執行時間點與次數 : 開始事件後以每秒約六十次執行 60FPS
    //應用 : 監聽玩家輸入與物件持續行為，例如 : 玩家有沒有按按鈕或讓物件持續移動
    private void Update()
    {
        print("我是更新事件!!");
    }
    #endregion

    #region 方法
    //方法 : 保存較複雜或演算法的程式區塊
    //語法 : 
    //修飾詞 傳回類型 名稱() { 較複雜或演算法的程式區塊 }
    //void 無傳回 : 使用這個方法不會有傳回
    //方法需要被呼叫才會執行
    /// <summary>
    /// 我是一個測試方法
    /// </summary>
    private void Test()
    {
        print("我是測試方法");
    }

    //如果不是無傳回void
    //在大括號內必須使用 傳回 return 值 (必須跟傳回類型相同)
    /// <summary>
    /// 傳回整數十的方法
    /// </summary>
    /// <returns>整數十</returns>

    private int Ten()
    {
        return 10;
    }

    //舉例 :
    //三個方法 1. 以時速 50 開車 2. 時速100 3.時速300
    //加新功能 要有音效
    private void Drive50()
    {
        print("開車時速:" + 50);
        print("開車音效");
    }
    private void Drive100()
    {
        print("開車時速:" + 100);
        print("開車音效");
    }
    private void Drive300()
    {
        print("開車時速:" + 300);
        print("開車音效");
    }

    //用參數解決 Paramater
    //參數語法 : 類型 參數名稱
    /// <summary>
    /// 開車
    /// </summary>
    /// <param name="speed">開車的時速</param>
    /// <param name="sound">開車的音效</param>
    /// <param name="effect">開車的特效</param>
    //有預設值得參數只能寫在最右邊
    private void Drive(int speed, string sound = "咻~", string effect = "灰塵特效")
    {
        print("開車時速:" + speed);
        print("開車音效" + sound);
        print("開車特效" + effect);

    }

    /// <summary>
    /// BMI值計算方法
    /// </summary>
    /// <param name="height">請輸入身高(單位為公尺)</param>
    /// <param name="weight">請輸入體重(單位為公斤)</param>
    /// <returns></returns>
    private float BMI(float height, float weight)
    {
        return weight / (height * height);
    }
    #endregion

}
