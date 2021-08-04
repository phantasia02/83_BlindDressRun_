using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CChildCollision : MonoBehaviour
{
    [SerializeField]  CMovableBase m_MainCMovable = null;

    private void Awake()
    {
        m_MainCMovable = gameObject.GetComponentInParent<CMovableBase>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_MainCMovable)
        {
            m_MainCMovable.OnCollisionEnter(collision);
        }
    }
}
