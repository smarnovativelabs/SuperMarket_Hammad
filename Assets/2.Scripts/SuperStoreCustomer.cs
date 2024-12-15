using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuperStoreCustomer : MonoBehaviour
{
    public string customerName = "Customer";
    public float customerSpeed;
    public AudioClip satisfactionSoundSound;
    public AudioClip wrongCheckoutSound;
    public GameObject paymentCard;
    public GameObject paymentCash;
    public GameObject customerShoppingBag;
 //   public GameObject customerVehicle;

    public List<SuperStoreItems> customerWishList;
    public Transform customerFinishPoint;
    NavMeshAgent agent;
    Animator anim;
    public Transform targetPoint;
    int rackTraverseCount;
    CustomerStatus customerStatus;
    int queueIndex;
    bool customerReachedToQueue = false;
    int checkOutXP = 3;
    int counterIndex;
    enum CustomerStatus
    {
        Idle,
        MovingToCounter,
        InQueue,
        AtCounter,
        Leaving,
        MovingIn,
    }

    //public void OnHoverItems()
    //{
    //    UIController.instance.DisplayHoverObjectName(customerName, true);
    //    UIController.instance.OnChangeInteraction(0, true);

    //    if (gameObject.GetComponent<Outline>())
    //    {
    //        gameObject.GetComponent<Outline>().enabled = true;
    //    }
    //}

    //public void OnInteract()
    //{
    //    var _toolPicked = GameController.instance.currentPickedTool;
    //    var _itemPick = GameController.instance.currentPicketItem;

    //    if (_itemPick == null && _toolPicked == null)
    //    {
    //        if (customerStatus == CustomerStatus.AtCounter)
    //        {
    //            UIController.instance.EnableCustomerServingPanel(AssignRoom,OnRejectRoomRequest);
    //        }
    //    }
    //    else
    //    {
    //        UIController.instance.DisplayInstructions("Throw Item");
    //    }
    //}
    //public void TurnOffOutline()
    //{
    //    if (gameObject.GetComponent<Outline>())
    //    {
    //        gameObject.GetComponent<Outline>().enabled = false;
    //    }
    //}

    //IEnumerator LeaveArea()
    //{
    //    yield return new WaitForSeconds(1f);
    //    customerVehicle.GetComponent<SuperStoreVehicle>().OnVehicleLeavingArea();
    //    Destroy(gameObject);
    //}
    IEnumerator MoveToRack()
    {
        yield return new WaitForSeconds(1.5f);
        rackTraverseCount++;
        if (rackTraverseCount >= customerWishList.Count)
        {
            customerStatus = CustomerStatus.MovingToCounter;
            targetPoint = SuperStoreManager.instance.posCounters[counterIndex].CounterQueue[queueIndex].queuePoint;
        }
        else
        {
            customerStatus = CustomerStatus.MovingIn;
            GameObject _rack = SuperStoreManager.instance.GetRack(customerWishList[rackTraverseCount].rackId);
            if (_rack != null)
            {
                targetPoint = _rack.GetComponent<StoreRack>().rackCustomerPoint;

            }
        }
        
        anim.SetInteger("move", 1);
        agent.enabled = true;
        agent.SetDestination(targetPoint.position);
    }
    private void Update()
    {
        if (customerStatus == CustomerStatus.MovingIn )
        {
            if (agent.enabled)
            {
                agent.SetDestination(targetPoint.position);
            }
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.25f)
            {
                customerStatus = CustomerStatus.Idle;
                anim.SetInteger("move", 0);
                agent.enabled = false;
                StartCoroutine(MoveToRack());

            }
        }
        else if (customerStatus == CustomerStatus.Leaving)
        {
            if (agent.enabled)
            {
                agent.SetDestination(targetPoint.position);
            }
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
            {
                customerStatus = CustomerStatus.Idle;
                anim.SetInteger("move", 0);
                agent.enabled = false;
                //StartCoroutine(LeaveArea());
                Destroy(gameObject);
            }
        }
        else if (customerStatus == CustomerStatus.MovingToCounter)
        {
            if (agent.enabled)
            {
                agent.SetDestination(targetPoint.position);
            }
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.25f)
            {
                agent.enabled = false;
                anim.SetInteger("move", 0);
                customerStatus = queueIndex <= 0 ? CustomerStatus.AtCounter : CustomerStatus.InQueue;
                transform.position = targetPoint.position;
                transform.rotation = targetPoint.rotation;
                if (customerStatus == CustomerStatus.AtCounter)
                {
                    //Call To Spawn Items
                    // CashCounterManager.instance.AcceptWishList(gameObject, customerWishList);
                    SuperStoreManager.instance.AcceptCustomerWishList(counterIndex, gameObject, customerWishList);
                    SuperStoreManager.instance.UpdateGameProgressBar(true);
                    SuperStoreManager.instance.OnCustomerReachedToCounter();
                }
                if (!customerReachedToQueue)
                {
                    customerReachedToQueue = true;
                    SuperStoreManager.instance.UpdateCustomersInWait(1);
                    UIController.instance.DisplayInstructions("New Customer At Super Store Counter");
                }
            }
        }
    }

    public void SpawnNewCustomer(int _counterIndex,int _queueIndex, Transform _spawnPoint,List<SuperStoreItems> _itemsList)
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = customerSpeed;
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        customerFinishPoint = _spawnPoint;
        counterIndex = _counterIndex;
        //customerVehicle = _vehicle;
        queueIndex = _queueIndex;
        customerWishList = _itemsList;
        customerStatus = CustomerStatus.Idle;
        agent.agentTypeID = GetAgentId(0);
        agent.enabled = false;
        rackTraverseCount = -1;
        StartCoroutine(MoveToRack());
    }
    public void UpdateEnqueueCustomerTarget(int _queueIndex,Transform _target)
    {
        if(customerStatus==CustomerStatus.Idle || customerStatus==CustomerStatus.MovingIn ||
            customerStatus == CustomerStatus.InQueue ||customerStatus==CustomerStatus.MovingToCounter)
        {
            queueIndex = _queueIndex;
            if (customerStatus == CustomerStatus.InQueue || customerStatus == CustomerStatus.MovingToCounter)
            {
                anim.SetInteger("move", 1);
                targetPoint = _target;
                customerStatus = CustomerStatus.MovingToCounter;
                agent.enabled = true;
                agent.SetDestination(targetPoint.position);
            }
            
        }
    }
    int GetAgentId(int _indexId)
    {
        if(_indexId<0 || _indexId >= NavMesh.GetSettingsCount())
        {
            Debug.LogError("Invalid Agent Index!");
            return -1;
        }
        return NavMesh.GetSettingsByIndex(_indexId).agentTypeID;
    }
    public void OnLeavingArea(bool _isCashier=false)
    {
        customerShoppingBag.SetActive(true);
        customerStatus = CustomerStatus.Leaving;
        targetPoint = customerFinishPoint;
        anim.SetInteger("move", 1);
        agent.enabled = true;
        agent.SetDestination(targetPoint.position);
        float _totalPrice = 0f;
        for(int i = 0; i < customerWishList.Count; i++)
        {
            _totalPrice += customerWishList[i].sellingPrice;
        }
        PlayerDataManager.instance.playerData.playerCash += (Mathf.CeilToInt(_totalPrice));
        SoundController.instance.OnPlayInteractionSound(satisfactionSoundSound);
        UIController.instance.DisplayInstructions((int)_totalPrice + "$ Collected");
        //  UIController.instance.CashText.text = PlayerDataManager.instance.playerData.playerCash.ToString();
        UIController.instance.UpdateCurrency((int)_totalPrice);

        SuperStoreManager.instance.RemoveItemsFromStore(customerWishList);
        SuperStoreManager.instance.OnRemoveCustomerFromQueue(counterIndex);

        //grant xp reward on checkout
        if (!_isCashier)
        {
            PlayerDataManager.instance.UpdateXP(checkOutXP);
            UIController.instance.UpdateXP(checkOutXP);
        }
    }
    public void OnWrongCheckout()
    {
        SoundController.instance.OnPlayInteractionSound(wrongCheckoutSound);
    }
    public void UpdateAnimation(bool _enable)
    {
        if (_enable)
        {
            int _val = Random.Range(0, 2);
            paymentCard.SetActive(_val == 0);
            paymentCash.SetActive(_val == 1);
            StartCoroutine(EnableCashOutline(_val == 0));
        }
        else
        {
            paymentCard.SetActive(false);
            paymentCash.SetActive(false);
        }
        anim.SetBool("pay", _enable);
    }
    IEnumerator EnableCashOutline(bool _isCard)
    {
        yield return new WaitForSeconds(0.1f);
        if (_isCard)
        {
            if (paymentCard.GetComponent<Outline>())
            {
                paymentCard.GetComponent<Outline>().enabled = true;
            }
        }
        else
        {
            for(int i = 0; i < paymentCash.transform.childCount; i++)
            {
                if (paymentCash.transform.GetChild(i).GetComponent<Outline>())
                {
                    paymentCash.transform.GetChild(i).GetComponent<Outline>().enabled = true;

                }
            }
        }
    }
}
