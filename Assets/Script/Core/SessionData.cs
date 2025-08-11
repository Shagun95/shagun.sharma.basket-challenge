using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionData : Singleton<SessionData>
{
    public ShootType currentShootType;
    public int scoreToAdd;

    public int currentTemporaryBonus = 0;
}
