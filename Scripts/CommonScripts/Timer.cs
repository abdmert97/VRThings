using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CommonScripts
{
    public enum TIMERSTARTTIME {AWAKE,START,CONDITION};
    public enum TIMESHOWTYPE {TYPE1,TYPE2,TYPE3};
    public class Timer : MonoBehaviour
    {
        #region static events
        public static event Action StartTimer;
        public static event Action StopTimer;
        public static event Action<int> EndTimer;
        public static event Action<int> RestartTimer;
        #endregion
        public TIMERSTARTTIME timerStartTime;
        [Tooltip("When you set this condition timer starts.")]
        public  bool StartCondition;
        private bool _isTimerStarted;
        public List<TimerElement> timers = new List<TimerElement>();

        #region External Usage Functions

        public void AddTime(int index, int addedTime)
        {
            if (timers.Count > index)
            {
                timers[index].timeLeft += addedTime;

            }
        }
        public void RestarTimerIndex(int index)
        {

            if (timers.Count > index)
            {
                timers[index].timeLeft = timers[index].startTime;
            }
        }
        public void PauseTimer(int index)
        {
            if (timers.Count > index)
            {
                timers[index].isTimerEnabled = false;
            }
        }
        public void ContinueTimer(int index)
        {
            if (timers.Count > index)
            {
                timers[index].isTimerEnabled = true;
            }
        }
        public void TurnOffDisplay(int index)
        {
            if (timers.Count > index)
            {
                timers[index].text.enabled = false;
                timers[index].isTextActive = false;
            }
        }
        public void TurnOnDisplay(int index)
        {
            if (timers.Count > index)
            {
                timers[index].text.enabled = true;
                timers[index].isTextActive = true;
            }
        }
        #endregion
        #region Internal Usage Functions
        private void Awake()
        {
            StartTimer += BeginTimer;
            StartTimer += CreateTimers;
            StopTimer += TimerStopped;
            if (timerStartTime == TIMERSTARTTIME.AWAKE)
            {
                StartTimer.Invoke();
            }
        }
        private void Start()
        {
            if (timerStartTime == TIMERSTARTTIME.START)
            {
                StartTimer.Invoke();
            }
        }
        private void Update()
        {
            if (!_isTimerStarted && timerStartTime == TIMERSTARTTIME.CONDITION && StartCondition)
            {
                StartTimer.Invoke();
            }
            if (_isTimerStarted)
            {
                RunTimers();
            }
        }
        private void TimerStopped()
        {
            Debug.Log("timerStopped");
        }
        private void RunTimers()
        {
            float counterCount = timers.Count;

            for (int i = 0; i < counterCount; i++)
            {
                TimerElement timerElement = timers[i];

                if (timerElement.timeLeft > 0 && timerElement.isTimerEnabled)
                    timerElement.timeLeft -= Time.deltaTime;
                if (timerElement.timeLeft <= 0 && timerElement.isTimerEnabled)
                {
                    timerElement.isTimerEnabled = false;
                    StopTimer.Invoke();
                    EndTimer.Invoke(i);
                }
            }
            DisplayTimes();
        }

        private void DisplayTimes()
        {
            float counterCount = timers.Count;

            //TYPE1 : MM:SS , TYPE2 : MM, TYPE3 : SS
            for (int i = 0; i < counterCount; i++)
            {
                TimerElement timerElement = timers[i];

                string displayFormat = "";
                float time = timerElement.timeLeft;
                if (timerElement.timeShowType == TIMESHOWTYPE.TYPE1)
                {
                    displayFormat += ((int)(time / 60)).ToString();
                    displayFormat += " : ";
                    displayFormat += ((int)(time % 60)).ToString();
                }
                else if (timerElement.timeShowType == TIMESHOWTYPE.TYPE2)
                {
                    displayFormat += ((int)(time / 60)).ToString();
                }
                else
                {
                    displayFormat += ((int)time).ToString();
                }
                timerElement.text.text = displayFormat;
            }
        }

        private void BeginTimer()
        {
            _isTimerStarted = true;
        }
        private void CreateTimers()
        {
            float counterCount = timers.Count;
            for (int i = 0; i < counterCount; i++)
            {
                TimerElement timerElement = timers[i];
                timerElement.text.text = timerElement.startTime.ToString();
                timerElement.timeLeft = timerElement.startTime;
            }
        }
        #endregion
    }

    #region Timer Element Storage
    [System.Serializable]
    public class TimerElement
    {
        public TextMeshProUGUI text;
        public bool isTimerEnabled;
        public bool isTextActive;
        [HideInInspector]
        public float timeLeft;
        public float startTime;
        [Tooltip("TYPE1 : MM:SS , TYPE2 : MM, TYPE3 : SS")]
        public TIMESHOWTYPE timeShowType;
        public TimerElement(TextMeshProUGUI _text,
                            bool _isTimerEnabled,
                            bool _isTextActive,
                            float _timeLeft,
                            float _startTime,
                            TIMESHOWTYPE _timeShowType)
        {
            text = _text;
            isTimerEnabled = _isTimerEnabled;
            isTextActive = _isTextActive;
            timeLeft = _timeLeft;
            startTime = _startTime;
            timeShowType = _timeShowType;
        }
    }
    #endregion
}

