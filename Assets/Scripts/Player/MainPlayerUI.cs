using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ODUY
{
    public class MainPlayerUI : MonoBehaviour
    {
        [SerializeField] private Text m_playerName = default;

        MainPlayer m_target;

        Transform targetTransform;
        Vector3 targetPosition;

        private void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }

        void LateUpdate()
        {
            // Follow the Target GameObject on screen.
            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;

                this.transform.position = targetPosition + new Vector3(0f, 1f, 0f);
            }

        }

        public void SetTarget(MainPlayer _target)
        {
            if (_target == null)
                return;

            this.m_target = _target;
            targetTransform = this.m_target.GetComponent<Transform>();


            m_playerName.text = _target.photonView.Owner.NickName;

        }

    }

}
