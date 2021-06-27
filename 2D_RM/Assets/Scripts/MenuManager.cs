using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //引用場景管理器API

public class MenuManager : MonoBehaviour
{
    //使用靜態方法處理1. 開始遊戲 2. 離開遊戲
    //如何讓按鈕跟程式溝通
    //需要一個公開的方法
    public void StartGame()
    {
        //延遲呼叫 ("方法名稱",延遲時間)
        Invoke("DelyStartGame",1.1f);
    }

    private void DelayStartGame()
    {
        //Application.LoadLevel("遊戲畫面");   //綠底: 過時的API 建議換新的
        //場景管理.載入場景("場景名稱")
        SceneManager.LoadScene("遊戲畫面"); //使用字串載入
        // SceneManager.LoadScene(1);       //使用編號載入
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void QuitGame()
    {
        Invoke("DelyQuittGame", 1.1f);
    }
    private void DelayQuitGame()
    {
        //應用程式.離開()
        Application.Quit();
    }
}
