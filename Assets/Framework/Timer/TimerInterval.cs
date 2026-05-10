using System;
using UnityEngine;

namespace Framework.Timer
{
    public class TimerInterval : TimerBase
    {
        public static TimerInterval Create(float interval, int times, Action<int> onExecute){
            var t = new TimerInterval(interval, times, onExecute);
            GlobalUpdate.Instance.Register(t);
            return t;
        }

        private TimerInterval(float interval, int times, Action<int> onExecute){
            INTERVAL   = interval;
            TIMES      = times;
            ON_EXECUTE = onExecute;
        }


        private readonly float       INTERVAL;
        private readonly int         TIMES;
        private readonly Action<int> ON_EXECUTE;

        private float _time;
        private int   _count;


        public override void OnUpdate(){
            _time += Time.deltaTime;
            if (_time < INTERVAL) return;
            _time -= INTERVAL;

            _count++;
            if (TIMES == -1 || _count < TIMES)
                ON_EXECUTE.Invoke(_count);
            else{
                onComplete?.Invoke();
                IsDone = true;
            }
        }

        public TimerInterval OnComplete(Action action){
            onComplete = action;
            return this;
        }
    }
}