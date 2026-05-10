using System;
using UnityEngine;


namespace Framework.Timer
{
    public class TimerPerFrame : TimerBase
    {
        public static TimerPerFrame Create(float duration, Action<float> onFrame = null){
            var t = new TimerPerFrame(duration, onFrame);
            GlobalUpdate.Instance.Register(t);
            return t;
        }

        private TimerPerFrame(float duration, Action<float> onFrame){
            DURATION = duration;
            ON_FRAME = onFrame;
        }

        private readonly Action<float> ON_FRAME;
        private readonly float         DURATION;

        public override void OnUpdate(){
            ON_FRAME?.Invoke(time);
            time += Time.deltaTime;
            if (time < DURATION) return;
            onComplete?.Invoke();
            IsDone = true;
        }
        
        
        public TimerPerFrame OnComplete(Action action){
            onComplete = action;
            return this;
        }
    }
}