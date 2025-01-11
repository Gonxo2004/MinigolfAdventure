using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayButton : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        Debug.Log("Cambiando a escena: " + name);
        SceneManager.LoadScene(name);
    }

}


