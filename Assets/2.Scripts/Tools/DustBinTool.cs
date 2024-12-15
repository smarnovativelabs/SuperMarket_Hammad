using UnityEngine;

public class DustBinTool : MonoBehaviour, IToolAction
{
    public AudioClip toolSound;

    public void PerformAction()
    {
        //int _index = RoomManager.instance.currentRoomNumber;
        

        //if (_index > -1)
        //{
        //    RoomManager.instance.rooms[_index].CheckIfTrashIsCleaned();
        //    RoomManager.instance.rooms[_index].CheckRoomProgress();            
        //}
        gameObject.GetComponent<Animator>().SetTrigger("DustBinHit");
        SoundController.instance.OnPlayInteractionSound(toolSound);
        // Add specific brush action logic here
    }
    public void AutoPerformAction()
    {

    }
}
