using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    [SerializeField] float modifiedOffsetCoeff, modifierOffsetLerpCoeff;

    private bool isFollowing;
    private GameObject targetObj;
    private Vector3 offset;
    private Vector3 modifiedOffsetDirection, modifiedOffset;

    void Awake()
    {
        isFollowing = false;
    }

    void FixedUpdate()
    {
        if (isFollowing)
        {
            Following();
        }
    }

    public Camera GetMainCam()
    {
        return mainCam;
    }

    #region Camera Follow

    private void Following()
    {
        modifiedOffset = Vector3.Lerp(modifiedOffset, modifiedOffsetDirection * modifiedOffsetCoeff, modifierOffsetLerpCoeff * Time.fixedDeltaTime);

        Vector3 desiredPosition = targetObj.transform.position + offset + modifiedOffset;

        transform.position = desiredPosition;
    }

    public void SetModifiedOffsetDirection(Vector3 direction)
    {
        modifiedOffsetDirection = direction.normalized;
    }

    public void StartFollow(GameObject obj)
    {
        isFollowing = true;
        targetObj = obj;
        offset = mainCam.transform.position - obj.transform.position;
    }

    public void StartFollow(GameObject obj, Vector3 offset)
    {
        isFollowing = true;
        targetObj = obj;
        this.offset = offset;
    }

    public void StopFollow()
    {
        isFollowing = false;
    }

    #endregion

    public void SetCameraPosition(Vector2 position)
    {
        transform.position = position;
    }
}
