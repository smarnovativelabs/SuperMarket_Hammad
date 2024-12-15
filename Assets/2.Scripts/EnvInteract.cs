using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnvInteract : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static EnvInteract instance;
    Vector3 m_StartPos;
    float tapTimer = 0f;
    int tapPointerId = -1;
    float tapDistance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //  Debug.Log("EnvInteract instance set.");
        }
        else
        {
            Destroy(gameObject);
            //    Debug.Log("Duplicate EnvInteract instance destroyed.");
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (tapPointerId < 0)
        {
            tapPointerId = data.pointerId;
            m_StartPos = data.position;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (data.pointerId == tapPointerId)
        {
            tapPointerId = -1;

            if (tapTimer < 0.5f)
            {
                float distance = Vector3.Distance(m_StartPos, data.position);

                if (distance < 0.25f)
                {
                    UIController.instance.OnInteractBtnPressed();
                }
            }
            tapTimer = 0f;
        }
    }

    public void OnInGameAd()
    {
        tapPointerId = -1;
        tapTimer = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        //  Debug.Log("EnvInteract Start method called.");
    }

    // Update is called once per frame
    void Update()
    {
        if (tapPointerId >= 0)
        {
            tapTimer += Time.deltaTime;
        }
    }
}