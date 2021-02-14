using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float Level(int score)
    {
        int defaultSpeed = -5;
        int increSpeed = (score * -1) / 100;
        if (increSpeed < -5)
        {
            increSpeed = -5;
        }

        return defaultSpeed + increSpeed;
    }
}
