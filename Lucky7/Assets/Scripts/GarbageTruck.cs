using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarbageTruck : MonoBehaviour
{
    [SerializeField] Trash.trashType type;


    // Start is called before the first frame update
    void Start()
    {

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
            MapHandler m = FindObjectOfType<MapHandler>();
            if( t.type == type )
            {
                m.AddScore(t.value);

                Destroy(t.gameObject);
            }
            else
            {
                m.RemoveScore();
                Destroy(t.gameObject);
            }
        }

        TruckDepot depot = other.gameObject.GetComponent<TruckDepot>();
        if(depot != null)
        {
            Canvas[] canvasses = FindObjectsOfType<Canvas>();
            if (canvasses != null)
            {
                for (int i = 0; i < canvasses.Length; i++)
                {
                    if (canvasses[i].name == "TruckSpawner")
                    {
                        canvasses[i].enabled = true;
                    }
                }
            }
        }
    }
}
