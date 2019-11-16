using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float m_speed = 2;
    public float offsetY = 4f;
    public Transform m_target = default;

    private bool isFollowing;

    public void OnStartFollowing(GameObject target)
    {
        m_target = target.transform;
        isFollowing = true;

    }

    private void Update()
    {
        if(isFollowing)
        {
            float interpolation = m_speed * Time.deltaTime;

            Vector3 position = this.transform.position;
            position.y = Mathf.Lerp(transform.position.y, m_target.position.y + offsetY, interpolation);
            position.x = Mathf.Lerp(transform.position.x, m_target.position.x, interpolation);

            transform.position = position;
        }
        

    }

}
