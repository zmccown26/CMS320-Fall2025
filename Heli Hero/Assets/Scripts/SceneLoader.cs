using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
public enum Scene{
    MainMenu,
    Level_01,
}

        public static void LoadScene(Scene scene){
            SceneManager.LoadScene(scene.ToString());
        }
}