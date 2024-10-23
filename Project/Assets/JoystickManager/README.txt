    Компонент JoystickInputManager служит для считывания всех событий джойстика.

    Также в компоненте заложена возможность передавать события нажатий на объекты UI,
которые расположены на Canvas, Render Mode которого установлен как World Space. Для этого необходимо определить
отдельный Layer для таких объектов и выбрать его в инспекторе для параметра World UI Layer компонента JoystickManager.
Также на объект нужно добавить компонент, реализующий интерфейс IWorldUIRayReceiver.
Таким образом, если под пальцем при нажатии на экран будет расположен объект на указанном слое, 
все события для данного нажатия не будут вызваны в InputManager, и будет вызван метод Receive() у компонента, 
реализующий интерфейс IWorldUIRaycasReceiver

    В инспекторе у компонента JoystickInputManager можно настроить следующие параметры:
        Joystick - ссылка на компонент, наследованный от Joystick
            Префабы с этими компонентами находятся в папке Joystick pack
            (Это модифицированная версия ассета Joystick pack. Необходимо учесть,
                что ассет из Asset store не будет корректно работать с Joystick Input)
        World UI Layer - layer элементов UI, расположенных на Canvas в мировом пространстве;

    Публичные свойства JoystickInputManager:
	    Vector2 JoystickDirection - направление ввода через джойстик;

    В компоненте JoystickInputManager определены следующие события:
        onStartInput - вызывается в момент нажатия пальца пользователя на джойстик;
        onStopInput - вызывается в момент отпускания пальца пользователя с джойстика;

    Подписаться на данные события можно через CallbackManager, все необходимые события уже определены в CallbackManager изначально
        Пример:
            DiyaFW.CallbackSystem.Subscribe("onStartInput", (EventParams p) => Debug.Log("Start input"));

    Также, на каждое событие можно подписаться напрямую через интерфейс взаимодействия JoystickInputManager
        Пример:
            DiyaFW.JoystickInputManager.SubscribeOnStartInput(() => Debug.Log("Start input"));

    