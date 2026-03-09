using UnityEngine;

public class MagnetCollect : MonoBehaviour
{
    public bool isAttracted = false;
    private Transform playertransform;
    public float speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playertransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttracted & playertransform != null)
        {
            transform.position = 
            Vector3.MoveTowards(transform.position, playertransform.position, speed * Time.deltaTime);
        }
    }
}
