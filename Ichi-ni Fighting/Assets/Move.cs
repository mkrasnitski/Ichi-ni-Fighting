using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Move : MonoBehaviour
{
    private int startup;
    private int active;
    private int recovery;
    private int landingLag;
    private int damage;
    private int total;

    public Move()
    {
        startup = 0;
        active = 0;
        recovery = 0;
        landingLag = 0;
        damage = 0;
        total = 0;
    }

    public Move(int s, int a, int r, int d)
    {
        startup = s;
        active = a;
        recovery = r;
        total = startup + active + recovery;
        damage = d;
    }

    public int Startup
    {
        get { return startup; }
    }

    public int Active
    {
        get { return active; }
    }

    public int Recovery
    {
        get { return recovery; }
    }

    public int LandingLag
    {
        get { return landingLag; }
    }

    public int Damage
    {
        get { return damage; }
    }

    public int Total
    {
        get { return total; }
    }
}
