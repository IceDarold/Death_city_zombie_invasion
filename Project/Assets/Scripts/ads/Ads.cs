using System;
using DG.Tweening;

namespace ads
{
    public class Ads
    {
        private const float InterDelay = 120;
        private static bool _interInCooldown;
        public static void ShowReward(Action onComplete)
        {

        }

        public static void ShowInter()
        {
            if (_interInCooldown) return;

            _interInCooldown = true;
            DOVirtual.DelayedCall(InterDelay, () => _interInCooldown = false);
        }
    }
}
