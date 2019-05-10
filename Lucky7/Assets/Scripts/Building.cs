using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] Road TrashDumpLocation;
    private float spawnProgress;
    // Start is called before the first frame update
    void Start()
    {
        spawnProgress = 8;
    }

    // Update is called once per frame
    void Update()
    {
        spawnProgress += Time.deltaTime;
        if( spawnProgress > 10 )
        {
            DumpTrashOnRoad();
            spawnProgress -= 10;
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
