using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageTruck : MonoBehaviour
{
    [SerializeField] Trash.trashType type;
    public int Score;
    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Trash t = other.gameObject.GetComponent<Trash>();
        if ( t != null )
        {
            if( t.type == type )
            {
                AddScore();
                Destroy(t.gameObject);
            }
            else
            {
                RemoveScore();
                Destroy(t.gameObject);
            }
        }
    }

    public void AddScore()
    {
        Score += 100;
    }

    public void RemoveScore()
    {
        Score -= 100;
    }
}
