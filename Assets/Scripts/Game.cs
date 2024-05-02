using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    

    private void OnPause()
    {

        Time.timeScale = 0;
    }

    private void OnPlay()
    {
        Time.timeScale = 1;

    }
}
