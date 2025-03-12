using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprRnd : MonoBehaviour
{
    [SerializeField] List<Sprite> sprList;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprList[Random.Range(0, sprList.Count)];
    }
}
