using UnityEngine;

public class HammerTool : MonoBehaviour, IToolAction
{
    public AudioClip toolSound;
    public void PerformAction()
    {
        //int _index = RoomManager.instance.currentRoomNumber;
        //if (_index >= 0)
        //{
        //    RoomManager.instance.rooms[_index].CheckIfTrashIsCleaned();

        //}

        gameObject.GetComponent<Animator>().SetTrigger("PlayHammer");
        SoundController.instance.OnPlayInteractionSound(toolSound);
        // Add specific hammer action logic here
    }
    public void AutoPerformAction()
    {

    }
}
