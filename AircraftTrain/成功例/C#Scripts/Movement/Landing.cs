using UnityEngine;
using UnityEngine.SceneManagement;

public class Landing : MonoBehaviour
{   
    // 変数の宣言--------------------------------------------------
    // 航空機の3°降下時の速度(移動ベクトル)を指定
    Vector3 speed = new Vector3(0f, -5.24f, 100f);

    // Rwアプローチ01か19を選択
    public int Rw_num;

    public int num = 0; // このシーンでの航空機の飛行回数

    public static int Scene_Num = 2; // シーンナンバーを指定

    public int Now_Scene_Num = 2; // インスペクターに現在のシーンナンバーを表示

    public int light_num = 0; // Directional Lightのオンを0, オフを1に設定   

    
    public void StartPos()
    {
        Vector3 start_position = Vector3.zero;
        
        if(Rw_num == 1)
        {
            // Rw01の初期位置
            start_position = new Vector3(0f, 75.99f, -150f);
        }
        else if(Rw_num == 19)
        {
            // Rw19の初期位置
            start_position = new Vector3(0f, 56.60f, -150f);
        }
        
        // 初期位置に航空機を設置
        this.transform.position = start_position;
    }

    void Start()
    {
        StartPos();
    }

    public void Update()
    {  
        // 1秒毎の航空機の座標を変更
        this.transform.position += speed * 2 * Time.deltaTime;

        if(this.transform.position.z >= 150)
        {
            // シーンを切り替え
            if(num == 9)
            {
                if(light_num == 0)
                {
                    light_num++;// ライトをオフに
                    num = 0; // 飛行回数を0にリセット
                }
                else if(light_num == 1)
                {
                    Scene_Num++; // シーンナンバーを1追加

                    if(Scene_Num == 1)
                    {
                        SceneManager.LoadScene("Landing_Rw19");
                    }
                    else if(Scene_Num == 2)
                    {
                        SceneManager.LoadScene("Takeoff_Rw01");
                    }
                    else if(Scene_Num == 3)
                    {
                        SceneManager.LoadScene("Takeoff_Rw19");
                    }
                    else if(Scene_Num == 4)
                    {
                        UnityEditor.EditorApplication.isPlaying = false;
                    }
                }
            }
            
            num ++;
            Now_Scene_Num = Scene_Num;
            
            StartPos();
        }
    }
}
