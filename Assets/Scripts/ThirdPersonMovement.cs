using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private Transform debugTransform;

    public CharacterController controller;
    public Transform cam;

    //bool monsterCam = false;

    public float speed = 4f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;

    //public GameObject thirdCamera;

    //public Transform enemy;

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //creating movement
        if (direction.magnitude >= 0.1f)
        {
            //direction control
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //smoothing rotation
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //applying rotation
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        /*
        //focus on enemy camera
        if (Input.GetKeyDown(KeyCode.Q))
        {
            monsterCam = true;
            //Debug.Log(monsterCam);
        } else if (Input.GetKeyUp(KeyCode.Q))
        {
            monsterCam = false;
            //Debug.Log(monsterCam);
        }

        if (monsterCam == true)
        {
            thirdCamera.SetActive(true);
            //Debug.Log("Zoom in");
            transform.LookAt(enemy);
        } else
        {
            thirdCamera.SetActive(false);
        }*/

        //aim
        //Get center of the screen value
        /*
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            transform.position = raycastHit.point;
            debugTransform.position = raycastHit.point;
        }*/
    }
}
