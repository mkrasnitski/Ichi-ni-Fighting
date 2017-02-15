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
    private int total;

    public Move(int s, int a, int r)
    {
        startup = s;
        active = a;
        recovery = r;
        total = s + a + r;
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

    public int Total
    {
        get { return recovery; }
    }
}
