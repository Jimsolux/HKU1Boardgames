using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region oldstuffs
    //public Vector3 current;
    //public Vector3 target;
    //public float moveSpeed;
    //public Camera cam;

    //private enum cameraDir
    //{
    //    up, down, left, right
    //}


    //private void Update()
    //{
    //    current = this.gameObject.transform.position;
    //    CheckMousePosition();
    //}

    //private void CheckMousePosition()
    //{
    //    if(Input.mousePosition.x > Screen.width - (Screen.width / 10))
    //    {
    //        target = Input.mousePosition;
    //        UpdateCameraPos(cameraDir.right);
    //    }
    //    if (Input.mousePosition.z > Screen.height - (Screen.height / 10))
    //    {
    //        target = Input.mousePosition;
    //        UpdateCameraPos(cameraDir.up);
    //    }
    //    if (Input.mousePosition.x > Screen.width - Screen.width + (Screen.width / 10))
    //    {
    //        target = Input.mousePosition;
    //        UpdateCameraPos(cameraDir.left);
    //    }
    //    if (Input.mousePosition.z > Screen.height - Screen.height + (Screen.height / 10))
    //    {
    //        target = Input.mousePosition;
    //        UpdateCameraPos(cameraDir.down);
    //    }
    //}

    //private void UpdateCameraPos(cameraDir dir)
    //{

    //    switch(dir)
    //    {
    //        case cameraDir.up:
    //            cam.transform.Translate(new Vector3(0, 0, -moveSpeed), Space.World);
    //            break;
    //        case cameraDir.down:
    //            cam.transform.Translate(new Vector3(0, 0, moveSpeed), Space.World);
    //            break;
    //        case cameraDir.left:
    //            cam.transform.Translate(new Vector3(-moveSpeed, 0, 0), Space.World);
    //            break;
    //        case cameraDir.right:
    //            cam.transform.Translate(new Vector3(moveSpeed, 0, 0), Space.World);
    //            break;
    //    }
    //    //this.gameObject.transform.position = Vector3.Lerp(current, target, moveSpeed * Time.deltaTime);
    //}
    #endregion
    public float panSpeed = 1f; // Speed of camera panning
    public float borderThickness = 30f; // Thickness of the border where panning is allowed

    void Update()
    {
        //Debug.Log(Input.mousePosition.x + " X coordinate");
        //Debug.Log(Input.mousePosition.y + " y coordinate");
        // Get mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;
        Vector3 midPointScreen = new Vector3(Screen.width /2, Screen.height /2, 0 );

        // Check if mouse position is near the screen borders
        bool nearLeftBorder = mousePosition.x < borderThickness;
        bool nearRightBorder = mousePosition.x > Screen.width - borderThickness;
        bool nearTopBorder = mousePosition.y > Screen.height - borderThickness;
        bool nearBottomBorder = mousePosition.y < borderThickness;

        // Check if mouse is near any border to allow panning
        if (nearLeftBorder || nearRightBorder || nearTopBorder || nearBottomBorder)
        {
            // Get mouse input along the horizontal and vertical axes
            float panHorizontal =  Input.mousePosition.x - midPointScreen.x;
            float panVertical = Input.mousePosition.y - midPointScreen.y;

            // Calculate the translation based on mouse input and pan speed
            Vector3 panTranslation = new Vector3(panHorizontal, 0, panVertical) * panSpeed * Time.deltaTime;

            // Apply the translation to the camera's position
            transform.Translate(panTranslation, Space.World);
        }
    }
}

