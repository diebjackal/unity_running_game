using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int statePoint;
    public int stageIndex;
    public int health;

    public PlayerMove player;
    public GameObject[] Stages;
    public void NextStage()
    {
        //Change State
        if(stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerPosition();
        }
        else
        { //Game Clear
            Time.timeScale = 0;

            Debug.Log("게임 클리어");
        }
        //calculate Point
        totalPoint += statePoint;
        statePoint = 0;
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
        if (health > 1)
            health--;
        else
        {
            //player Die Effect
            player.OnDie();
            //Result UI
            Debug.Log("Player Die");
            //Retry Button UI
        }
    }

    void PlayerPosition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }
}
