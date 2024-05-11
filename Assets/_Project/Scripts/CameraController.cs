using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineInputAxisController CameraInput;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool shouldRotate = Input.GetMouseButton(1);
        CameraInput.enabled = shouldRotate;

    }
}
