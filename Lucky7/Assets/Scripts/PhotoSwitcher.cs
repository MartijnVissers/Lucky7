using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoSwitcher : MonoBehaviour
{
    [SerializeField]
    private List<Texture> Truckpictures = new List<Texture>();

    [SerializeField]
    private List<GameObject> Trucks = new List<GameObject>();

    [SerializeField]
    private RawImage image;

    [SerializeField]
    private Canvas canvas;

    private int listitem = 0;
    
    public void ButtonUp()
    {
        if(listitem < Truckpictures.Count - 1)
        {
            listitem++;
        }
        else
        {
            listitem = 0;
        }
        image.texture = Truckpictures[listitem];
    }

    public void ButtonDown()
    {
        if (listitem >= 1)
        {
            listitem--;
        }
        else
        {
            listitem = Truckpictures.Count - 1;
        }
        image.texture = Truckpictures[listitem];
    }
    
    public void PlaceTruck()
    {
        canvas.enabled = false;
        Destroy(FindObjectOfType<GarbageTruck>().gameObject);
        GameObject temp = Instantiate(Trucks[listitem], new Vector3(4, 0, 1.8f), Quaternion.identity);
        temp.transform.Rotate(new Vector3(0, 180, 0));
    }
}
