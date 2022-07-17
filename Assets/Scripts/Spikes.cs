using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private Vector2 direction;
    public Cell cell;
    // Start is called before the first frame update
    void Start()
    {
        float angle = transform.eulerAngles.y * Mathf.Deg2Rad ;
        direction = new Vector2(-(int)Mathf.Sin(angle), -(int)Mathf.Cos(angle));
        GameManager.spikes.Add(this);
    }

    // Update is called once per frame
    public bool FinalTick()
    {
        for (int i = 0; i < GameManager.entities.Count; i++)
        {
            Entity entity = GameManager.entities[i];
            if (entity.currentCell == cell)
            {
                print("Spikes hitting " + entity.gameObject.name);
                entity.GetHit(direction, 1);
                return true;
            }
        }
        return false;
    }
}
