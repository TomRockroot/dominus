using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class X_MainMenu : MonoBehaviour
{

	public void StartSandbox()
    {
        SceneManager.LoadScene("Sandbox");
    }

    public void StartAStar()
    {
        SceneManager.LoadScene("AStar");
    }

    public void StartMapEditor()
    {
        SceneManager.LoadScene("MapEditor");
    }
}
