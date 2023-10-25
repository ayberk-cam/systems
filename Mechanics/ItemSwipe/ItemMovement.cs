using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private float swipeAngle = 0f;
    private bool isMoving = false;
    private bool isTouched = false;
    private Item item;

    private void Awake()
    {
        item = GetComponent<Item>();
    }

    public void SetCondition(bool condition)
    {
        isMoving = condition;
    }

    public bool GetCondition()
    {
        return isMoving;
    }

    private void OnMouseDown()
    {
        isTouched = true;
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        isTouched = false;
        isMoving = false;
        GridManager.instance.SetMoveCondition(false);
    }

    private void OnMouseDrag()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(finalTouchPosition,firstTouchPosition) > 1f && isTouched && !isMoving && !GridManager.instance.GetMoveCondition() && !item.GetItemIsMatched() && !GameManager.instance.GetGameCondition())
        {
            CalculateAngle();
        }
    }

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        MovementChecker();
    }

    private void MovementChecker()
    {
        isMoving = true;

        if (swipeAngle > -45 && swipeAngle <=45)
        {
            //RightSwipe
            EventHelper.ItemMovementEvent?.Invoke(item, "Right");
        }
        else if(swipeAngle > 45 && swipeAngle <= 135)
        {
            //UpSwipe
            EventHelper.ItemMovementEvent?.Invoke(item, "Up");
        }
        else if(swipeAngle > 135 || swipeAngle <= -135)
        {
            //LeftSwipe
            EventHelper.ItemMovementEvent?.Invoke(item, "Left");
        }
        else if(swipeAngle < -45 && swipeAngle >= -135)
        {
            //DownSwipe
            EventHelper.ItemMovementEvent?.Invoke(item, "Down");
        }
        else
        {
            //NoSwipe
        }
    }

    private IEnumerator MovementRoutine(Vector3 targetPosition, float duration, bool conditionSetter = false)
    {
        if(conditionSetter)
        {
            isMoving = true;
        }

        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        if (conditionSetter)
        {
            isMoving = false;
        }
    }

    public void MakeMovement(Vector3 targetPosition)
    {
        StartCoroutine(MovementRoutine(targetPosition,0.1f));
    }

    public void MakeNoMovement(string swipePosition)
    {
        StartCoroutine(NoMovementRoutine(swipePosition));
    }

    private IEnumerator NoMovementRoutine(string swipePosition)
    {
        Vector3 startPosition = Vector3.zero;
        Vector3 targetPosition = Vector3.zero;

        switch (swipePosition)
        {
            case "Left":
                startPosition = gameObject.transform.position;
                targetPosition = gameObject.transform.position + Vector3.left * 0.1f;
                yield return MovementRoutine(targetPosition, 0.05f);
                yield return MovementRoutine(startPosition, 0.05f);
                break;
            case "Right":
                startPosition = gameObject.transform.position;
                targetPosition = gameObject.transform.position + Vector3.right * 0.1f;
                yield return MovementRoutine(targetPosition, 0.05f);
                yield return MovementRoutine(startPosition, 0.05f);
                break;
            case "Up":
                startPosition = gameObject.transform.position;
                targetPosition = gameObject.transform.position + Vector3.up * 0.1f;
                yield return MovementRoutine(targetPosition, 0.05f);
                yield return MovementRoutine(startPosition, 0.05f);
                break;
            case "Down":
                startPosition = gameObject.transform.position;
                targetPosition = gameObject.transform.position + Vector3.down * 0.1f;
                yield return MovementRoutine(targetPosition, 0.05f);
                yield return MovementRoutine(startPosition, 0.05f);
                break;
        }
    }
}
