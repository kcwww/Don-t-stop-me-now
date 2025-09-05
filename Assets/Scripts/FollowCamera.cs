using UnityEngine;

enum CameraMode
{
    TopView,
    SideView
}

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] CameraMode cameraMode = CameraMode.TopView;



    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 탑뷰 에서는 타겟의 x 및 z 좌표를 따라가고, y 좌표는 고정
        if (cameraMode == CameraMode.TopView)
        {
            Vector3 newPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.position = newPosition;
        }
        else if (cameraMode == CameraMode.SideView)
        {
            // 사이드뷰 에서는 타겟의 x 및 y 좌표를 따라가고, z 좌표는 고정
            Vector3 newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }
}
