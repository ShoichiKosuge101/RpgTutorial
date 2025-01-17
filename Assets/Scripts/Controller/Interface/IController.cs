﻿using Cysharp.Threading.Tasks;
using Utils;

namespace Controller.Interface
{
    public interface IController
    {
        public BaseParam CurrentParam { get; }
        public UniTask AttackAsync(ControllerBase target);
        public UniTask DefendAsync(ControllerBase target);
        public UniTask HealAsync(ControllerBase target);
        public UniTask TakeDamage(int damage);

        void SendStatus(BaseParam baseParam, bool isPlayer);
        void SendLog(in string message);
    }
}