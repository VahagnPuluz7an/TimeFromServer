using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TimeFetcher : MonoBehaviour
{
    private const string URL = "https://yandex.com/time/sync.json";
    
    public void GetTime(Action<DateTime> completed)
    {
        StartCoroutine(GetTimeFromServer(x => completed?.Invoke(x)));
    }

    private static IEnumerator GetTimeFromServer(Action<DateTime> completed)
    {
        var request = UnityWebRequest.Get(URL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            var syncData = JsonUtility.FromJson<SyncTimeData>(json);
            
            var dateTime = UnixTimeStampToDateTime(syncData.time);
            
            completed?.Invoke(dateTime);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp);
        return dateTimeOffset.LocalDateTime;
    }

    [Serializable]
    public class SyncTimeData
    {
        public long time;
    }
}
