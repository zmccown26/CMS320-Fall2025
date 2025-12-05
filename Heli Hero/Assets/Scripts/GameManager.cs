using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.Cinemachine;
public class GameManager : MonoBehaviour {

public static GameManager Instance { get; private set; }


[SerializeField] private List<GameLevel> gameLevelList;
[SerializeField] private CinemachineCamera cinemachineCamera;


private static int levelNumber = 1;


public event System.EventHandler OnGamePaused;
public event System.EventHandler OnGameResumed;

private int score;
private float time;
private bool isTimerActive;

private void Awake(){
   Instance = this;
}

private void Start(){
   Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
   Lander.Instance.OnLanded += Lander_OnLanded;
   Lander.Instance.OnStateChanged += Lander_OnStateChanged;

   GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;
   LoadCurrentLevel();
}

private void GameInput_OnMenuButtonPressed(object sender, System.EventArgs e){
   PauseUnpauseGame();
}

private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e){
   isTimerActive = e.state == Lander.State.Normal;

   if(e.state == Lander.State.Normal){
      cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
      CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize();

}
}
private void Update(){
   if(isTimerActive){
      time += Time.deltaTime;
   }
}

private void LoadCurrentLevel(){
   foreach(GameLevel gameLevel in gameLevelList){
      if(gameLevel.GetLevelNumber() == levelNumber){
         
         GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
         Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
         cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
         CinemachineCameraZoom2D.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
      }
   }
}

 public void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e){
      AddScore(e.score);
}

private void Lander_OnCoinPickup(object sender, System.EventArgs e){
   AddScore(500);
   
}

 public void AddScore(int addScoreAmount)
 {
    score += addScoreAmount;
    Debug.Log("Coin picked up! Score: " + score);
 }

 public int GetScore() {
   return score;
 }

 public float GetTime() {
   return time;
 }

public void GoToNextLevel() {
   levelNumber++;
   SceneManager.LoadScene(1);
}

public void RetryLevel() {
   SceneManager.LoadScene(1);
}

public int GetLevelNumber() {
   return levelNumber;
}

public void PauseGame() {
   Time.timeScale = 0f;
   OnGamePaused?.Invoke(this, EventArgs.Empty);
}

public void ResumeGame() {
   Time.timeScale = 1f;
   OnGameResumed?.Invoke(this, EventArgs.Empty);
}

public void PauseUnpauseGame() {
   if(Time.timeScale == 1f){
      PauseGame();
   } else {
      ResumeGame();
   }
}
}
