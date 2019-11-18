using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Prototype : MonoBehaviour
{
    // Starts Prototype (Changes Scene when Player Presses Buton)
    public void StartPrototypeScene ()
    {
        SceneManager.LoadScene("Prototype");
    }
}
