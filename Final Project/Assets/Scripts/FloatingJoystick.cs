using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    Vector2 joystickCenter = Vector2.zero;
    //public Rigidbody2D Player;
    //int xDirection;
    public int speed;

    void Start()
    {
        background.gameObject.SetActive(false);
    }

    //public int getDirection()
    //{
    //    return xDirection;
    //}
    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickCenter;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
        //if (direction.x > 0)
        //    xDirection = 1;
        //else if (direction.x < 0)
        //    xDirection = -1;
        //Player.AddForce(new Vector2(xDirection * speed, 0));
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.gameObject.SetActive(true);
        background.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
        joystickCenter = eventData.position;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        inputVector = Vector2.zero;
        //xDirection = 0;
    }
}