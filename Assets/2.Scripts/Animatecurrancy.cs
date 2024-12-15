using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animatecurrancy : MonoBehaviour
{
    public Vector3 moneyTryPos;
    public Vector3 moneyBoxPos;
    public CurrancyMove moveState;
    public float speed;
    public Vector3 destination;
    void Awake()
    {
     //   print("")
        moneyBoxPos = transform.transform.position;

        print(moneyBoxPos);
    }
  

    // Update is called once per frame
    void Update()
    {
        if(moveState== CurrancyMove.MovetoTry)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destination) <= 0.1f)
            {
              //  transform.position=destination+new Vector3(0,0.1f,0);
            }
        }
        if (moveState == CurrancyMove.MovetoBox)
        {
            print(moneyBoxPos);

            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);

            if(Vector3.Distance(transform.position, destination) <= 0.01f)
            {
                moveState = CurrancyMove.Destroy;

                transform.rotation=Quaternion.Euler(0f, 0f, 0f);

                StartCoroutine(destroyCash());
               
                //also remove this obejct from the list
            }
        }
    }

    public void setCurrenDestination(CurrancyMove move,Vector3 destination)
    {
        this.destination= destination;
        moveState = move;
    }

    IEnumerator destroyCash()
    {

        yield return new WaitForSeconds(0.2f);

      
        Destroy(gameObject);
    }
}

public enum CurrancyMove
{
    MovetoTry,
    MovetoBox,
    Destroy
}
