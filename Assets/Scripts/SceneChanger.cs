using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // call this from any script
    public static void EnterScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
