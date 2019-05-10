using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public enum trashType { GLASS = 0, METAL, PAPER, PLASTIC, BIO, EWASTE };
    public trashType type; //set this in inspector for the prefab.
    public int value;
    private float TimeSpan;

    // Start is called before the first frame update
    void Start()
    {
        value = 500;
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan += Time.deltaTime;
        if( TimeSpan > 1 )
        {
            TimeSpan -= 1;
            value -= 1;
            if( value < 100 )
            {
                value = 100;
            }
        }
    }
}
