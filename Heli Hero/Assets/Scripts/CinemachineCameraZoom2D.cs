using UnityEngine;
using Unity.Cinemachine;

public class CinemachineCameraZoom2D : MonoBehaviour
{

   private const float NORMAL_ORTHOGRAPHIC_SIZE = 25f; // Change this value to adjust zoom when camera locks onto helicopter

   public static CinemachineCameraZoom2D Instance { get; private set; }

   private void Awake(){
      Instance = this;
   }

    [SerializeField] private CinemachineCamera cinemachineCamera;
    private float targetOrthographicSize = 10f;

    private void Update(){
        float zoomSpeed = 2f;
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }

    public void SetTargetOrthographicSize(float targetOrthographicSize){
        this.targetOrthographicSize = targetOrthographicSize;
    }

    public void SetNormalOrthographicSize(){
        SetTargetOrthographicSize (NORMAL_ORTHOGRAPHIC_SIZE);
    }
}
