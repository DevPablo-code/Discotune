using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restart : MonoBehaviour
{
    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
        Time.timeScale = 1f;
    }
}
