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
        float angle = transform.eulerAngles.y;
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        GameManager.spikes.Add(this);
    }

    // Update is called once per frame
    public void ActionTick()
    {
        for (int i = 0; i < GameManager.entities.Count; i++)
        {
            Entity entity = GameManager.entities[i];
            if (entity.currentCell == cell)
            {
                entity.GetHit(direction, 1);
                print("Spikes hitting " + entity.gameObject.name);
            }
        }
    }
}
