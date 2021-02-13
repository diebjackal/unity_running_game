using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int statePoint;
    public int stageIndex;
    public int health;

    public PlayerMove player;
    public GameObject[] Stages;

    public Image[] UIhealth;
    public Text UIpoint;
    public Text UIStage;
    public GameObject RestatBtn;
    public void NextStage()
    {
        //Change State
        if(stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerPosition();
            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else
        { //Game Clear
            Time.timeScale = 0;
            Debug.Log("게임 클리어");
            RestatBtn.SetActive(true);
            UIStage.text = "Clear";
        }
        //calculate Point
        totalPoint += statePoint;
        statePoint = 0;
    }
    private void Update()
    {
        UIpoint.text = (totalPoint + statePoint).ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(health > 1)
                PlayerPosition();

            HealthDown();
        }
    }
    public void HealthDown()
    {
        if (health > 1){
            health--;
            UIhealth[health].color = new Color(1, 1, 1, 0.2f);
        }else{
            //All HEalth UI Off
            UIhealth[0].color = new Color(1, 1, 1, 0.2f);
            //player Die Effect
            player.OnDie();
            //Result UI
            Debug.Log("Player Die");
            //Retry Button UI
            RestatBtn.SetActive(true);
        }
    }
    void PlayerPosition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
