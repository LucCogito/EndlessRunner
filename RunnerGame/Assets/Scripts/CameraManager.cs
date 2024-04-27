using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField]
    private Camera _camera;

    public float HalfWidth { get; private set; }
    public float HalfHeight { get; private set;}

    protected override void Awake()
    {
        base.Awake();
        HalfWidth = _camera.orthographicSize * Screen.width / Screen.height;
        HalfHeight = HalfWidth * Screen.height / Screen.width;
    }
}
