using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] Road TrashDumpLocation;
    // Start is called before the first frame update
    void Start()
    {
        TrashDumpLocation = null;
    }

    // Update is called once per frame
    void Update()
    {
        float result = Random.Range(0, 10);
        if( result < Time.deltaTime )
        {
            DumpTrashOnRoad();
        }
    }

    public void SetDumpLocation(Road r)
    {
        TrashDumpLocation = r;
    }

    void DumpTrashOnRoad()
    {
        TrashDumpLocation.GainTrash();
    }
}
