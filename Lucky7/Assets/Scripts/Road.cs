using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public List<Trash> trashPrefabs;
    private Trash trash;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GainTrash()
    {
        if (trashPrefabs.Count > 0 && trash == null)
        {
            int spawnPlaceX = Random.Range(1, 3);
            int spawnPlaceZ = Random.Range(1, 3);
            float extraX = 0;
            float extraZ = 0;
            switch (spawnPlaceX)
            {
                case 1:
                    extraX = 0.25f;
                    break;
                case 2:
                    extraX = 0f;
                    break;
                case 3:
                    extraX = -0.25f;
                    break;
                default:
                    break;
            }
            switch (spawnPlaceZ)
            {
                case 1:
                    extraZ = 0.25f;
                    break;
                case 2:
                    extraZ = 0f;
                    break;
                case 3:
                    extraZ = -0.25f;
                    break;
                default:
                    break;
            }
            float result = Random.Range(0, trashPrefabs.Count);
            trash = Instantiate(trashPrefabs[Mathf.RoundToInt(result)], new Vector3(transform.position.x + extraX, transform.position.y, transform.position.z + extraZ), Quaternion.identity);
        }

    }
}
