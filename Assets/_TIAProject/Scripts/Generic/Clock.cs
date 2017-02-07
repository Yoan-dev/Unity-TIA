using UnityEngine;

public class Clock : IClock
{

    private float startTime;

	public void StartTime ()
    {
        startTime = Time.time;
    }

    public string ToStringTime()
    {
        float time = Time.time;
        int min = (int)((time - startTime) / 60);
        int sec = (int)((time - startTime) % 60);
        string textMin;
        string textSec;

        textMin = (min < 10) ? "0" + min : min.ToString();
        textSec = (sec < 10) ? "0" + sec : sec.ToString();
 
        return textMin + " : " + textSec;
    }
}
