using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementModel
{
    Camera map_cam_;
    int camera_offset_;

    // Start is called before the first frame update
    public CameraMovementModel(int offset)
    {
        camera_offset_ = offset;
    }
    void Start()
    {
        map_cam_ = Camera.main;
        camera_offset_ = 100;
    }

    //to bin func
    public void callback_move_camera()
    {
        

        

    }

    public void init_callback()
    {
        GameModel.callbackFct += callback_move_camera;
    }

    public void remove_callback()
    {
        GameModel.callbackFct -= callback_move_camera;
    }

}
