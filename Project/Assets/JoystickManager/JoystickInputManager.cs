using UnityEngine;
using System;

public class JoystickInputManager : MonoBehaviour
{
    public Vector2 JoystickDirection {
        get {
            return joystick.Direction;
        }
    }

    [SerializeField] Joystick joystick;
    [SerializeField] LayerMask worldUILayer;

    Action onStartInput;
    Action onStopInput;

    private bool _onInput;
    public bool OnInput => _onInput;
    
    public void Initialize()
    {
        if (!joystick) {
            throw new Exception("Field \"Joystick\" is empty");
        }

        joystick.SetWorldUILayer(worldUILayer);
        joystick.onStartInput += InvokeOnStartInput;
        joystick.onStopInput += InvokeOnStopInput;
    }

    #region ACTIONS
    public void SubscribeOnStartInput(Action action)
    {
        onStartInput += action;
    }
    public void SubscribeOnStopInput(Action action)
    {
        onStopInput += action;
    }

    public void UnsubscribeOnStartInput(Action action)
    {
        onStartInput -= action;
    }
    public void UnsubscribeOnStopInput(Action action)
    {
        onStopInput -= action;
    }

    void InvokeOnStartInput()
    {
        onStartInput?.Invoke();
        _onInput = true;
    }
    void InvokeOnStopInput()
    {
        onStopInput?.Invoke();
        _onInput = false;
    }
    #endregion

    public Vector2 GetJoystickDirectionRelativeObject(Transform transform)
    {
        Vector2 joystickInput = JoystickDirection;

        float joystickHorizontalSign = Mathf.Sign(joystickInput.x);
        float joystickVerticalSign = Mathf.Sign(joystickInput.y);
        Vector2 cameraRightDirection = new Vector2(transform.right.x, transform.right.z).normalized * joystickHorizontalSign;
        Vector2 xDirectionAdapted = Vector2.Lerp(Vector2.zero, cameraRightDirection, Mathf.Abs(joystickInput.x));
        Vector2 cameraForwardDirection = new Vector2(transform.forward.x, transform.forward.z).normalized * joystickVerticalSign;
        Vector2 zDirectionAdapted = Vector2.Lerp(Vector2.zero, cameraForwardDirection, Mathf.Abs(joystickInput.y));
        return xDirectionAdapted + zDirectionAdapted;
    }

}
