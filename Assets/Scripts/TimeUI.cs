using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Transform hour;
    [SerializeField] private Transform minute;
    [SerializeField] private Transform second;
    [SerializeField] private TimeFetcher fetcher;

    private DateTime _dateTime;
    private DateTime _lastCheckedTime;
    private float _startHourOffset;
    private float _startMinuteOffset;
    
    private void Start()
    {
        _startHourOffset = hour.localEulerAngles.z;
        _startMinuteOffset = minute.localEulerAngles.z;
        fetcher.GetTime(time =>
        {
            _dateTime = time;
            _lastCheckedTime = time;
        });
    }

    private void Update()
    {
        _dateTime = _dateTime.Add(TimeSpan.FromSeconds(Time.deltaTime));
        
        text.SetText(_dateTime.ToString("hh:mm tt", CultureInfo.InvariantCulture));

        float hourDegrees = 360f / 12f;
        float minuteDegrees = 360f / 60f;
        
        hour.localEulerAngles = new Vector3(0,0,_startHourOffset - hourDegrees * _dateTime.Hour);
        minute.localEulerAngles = new Vector3(0,0,_startMinuteOffset - minuteDegrees * _dateTime.Minute);
        second.localEulerAngles = new Vector3(0,0,_startMinuteOffset - minuteDegrees * _dateTime.Second);

        if (_dateTime.Hour == _lastCheckedTime.Hour)
            return;
        
        fetcher.GetTime(time =>
        {
            _dateTime = time;
            _lastCheckedTime = time;
        });
    }
}
