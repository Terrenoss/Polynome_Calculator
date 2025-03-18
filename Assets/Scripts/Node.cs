using TMPro;
using UnityEngine;

public class Node
{
    public float value;
    public string power;
    public string unity;
    public Node next;

    public Node(float value, string power, string unity)
    {
        this.value = value;
        this.power = power;
        this.unity = unity;
    }
}
