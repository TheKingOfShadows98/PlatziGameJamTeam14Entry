using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float timeEnd;
    public float Cd;

    public float TimeLeft() => timeEnd - Time.time;

    public bool IsEnd() => Time.time >= timeEnd;

    public bool IsAndRestart()
    {
        bool valid = IsEnd();
        if (valid) Start();
        return valid;
    }

    /// <summary>
    /// Start The Timer. <para></para>
    /// cd To defaultCd [Cd]
    /// </summary>
    /// <param name="cd"> cd = -1 To defaultCd [Cd]</param>
    public void Start(float cd = -1)
    {
        cd = cd < 0 ? Cd : cd;
        timeEnd = Time.time + cd;
    }
    public Timer(float defaultCd = 1)
    {
        Cd = defaultCd;
    }

}
