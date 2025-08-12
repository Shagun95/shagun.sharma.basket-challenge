using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionData : Singleton<SessionData>
{
    /// <summary>
    /// The shoot type currently achieved
    /// </summary>
    public ShootType currentShootType;
    public int scoreToAdd;
    public int currentTemporaryBonus = 0;
    public bool gameIsOn = false;
    /// <summary>
    /// Is the ball launching? used to disable the input (and the bar) while launching
    /// </summary>
    public bool ballIsLaunching = false;

    public int scoreForThisRound;
}
