using System;
using Cinemachine;
using UnityEngine;

public class CoolAnimationClass
{
    private Transform _cameraMan;
    private Transform _dooorCameraMan;
    private CinemachineVirtualCamera _vcam;
    private CinemachineFramingTransposer _framingTransposer;
    private CinemachineHardLookAt _hardLookAt;

    public CoolAnimationClass()
    {
        _cameraMan = GameObject.FindGameObjectWithTag("cameraman").transform;
        _vcam = _cameraMan.GetComponent<CinemachineVirtualCamera>();
        _framingTransposer = _vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void CoolRoomTransition(Door door)
    {
        _hardLookAt = _vcam.AddCinemachineComponent<CinemachineHardLookAt>();
        _vcam.Follow = door.transform;
        _vcam.LookAt = door.transform;
        _framingTransposer.m_TrackedObjectOffset = new Vector3(door.transform.forward.x, 0.5f, 1f);
        _framingTransposer.m_CameraDistance = 8f;
    }

    public void CoolRoomTransitionExit()
    {
        _vcam.DestroyCinemachineComponent<CinemachineHardLookAt>();
        _vcam.Follow = MainManager.Instance.myPlayer.transform;
        _vcam.LookAt = MainManager.Instance.myPlayer.transform;
        _framingTransposer.m_TrackedObjectOffset = new Vector3(0f, 0f, -0.4f);
        _framingTransposer.m_CameraDistance = 12f;

        _vcam.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }
}