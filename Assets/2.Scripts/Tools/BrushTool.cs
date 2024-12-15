//using UnityEngine;

//public class BrushTool : MonoBehaviour, IToolAction
//{
//    public AudioClip brushToolSound;
//    public void PerformAction()
//    {
//        var _index = RoomManager.instance.currentRoomNumber;
//        if (_index > -1)
//        {
//            RoomManager.instance.rooms[_index].CheckIfTrashIsCleaned();
//            RoomManager.instance.rooms[_index].CheckRoomProgress();
//        }
//        SoundController.instance.OnPlayInteractionSound(brushToolSound);
//        gameObject.GetComponent<Animator>().SetTrigger("PlayBrush");
//        // Add specific brush action logic here
//    }
//    public void AutoPerformAction()
//    {

//    }
//}
