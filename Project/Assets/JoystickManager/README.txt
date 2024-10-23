    ��������� JoystickInputManager ������ ��� ���������� ���� ������� ���������.

    ����� � ���������� �������� ����������� ���������� ������� ������� �� ������� UI,
������� ����������� �� Canvas, Render Mode �������� ���������� ��� World Space. ��� ����� ���������� ����������
��������� Layer ��� ����� �������� � ������� ��� � ���������� ��� ��������� World UI Layer ���������� JoystickManager.
����� �� ������ ����� �������� ���������, ����������� ��������� IWorldUIRayReceiver.
����� �������, ���� ��� ������� ��� ������� �� ����� ����� ���������� ������ �� ��������� ����, 
��� ������� ��� ������� ������� �� ����� ������� � InputManager, � ����� ������ ����� Receive() � ����������, 
����������� ��������� IWorldUIRaycasReceiver

    � ���������� � ���������� JoystickInputManager ����� ��������� ��������� ���������:
        Joystick - ������ �� ���������, ������������� �� Joystick
            ������� � ����� ������������ ��������� � ����� Joystick pack
            (��� ���������������� ������ ������ Joystick pack. ���������� ������,
                ��� ����� �� Asset store �� ����� ��������� �������� � Joystick Input)
        World UI Layer - layer ��������� UI, ������������� �� Canvas � ������� ������������;

    ��������� �������� JoystickInputManager:
	    Vector2 JoystickDirection - ����������� ����� ����� ��������;

    � ���������� JoystickInputManager ���������� ��������� �������:
        onStartInput - ���������� � ������ ������� ������ ������������ �� ��������;
        onStopInput - ���������� � ������ ���������� ������ ������������ � ���������;

    ����������� �� ������ ������� ����� ����� CallbackManager, ��� ����������� ������� ��� ���������� � CallbackManager ����������
        ������:
            DiyaFW.CallbackSystem.Subscribe("onStartInput", (EventParams p) => Debug.Log("Start input"));

    �����, �� ������ ������� ����� ����������� �������� ����� ��������� �������������� JoystickInputManager
        ������:
            DiyaFW.JoystickInputManager.SubscribeOnStartInput(() => Debug.Log("Start input"));

    