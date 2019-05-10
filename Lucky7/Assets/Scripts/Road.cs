using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] List<Trash> trashPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        trashPrefabs = new List<Trash>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainTrash()
    {
        if( trashPrefabs.Count > 0 )
        {
            float result = Random.Range(0, trashPrefabs.Count - 1);
            Instantiate(trashPrefabs[Mathf.RoundToInt(result)], gameObject.transform);
        }
        
    }
}
