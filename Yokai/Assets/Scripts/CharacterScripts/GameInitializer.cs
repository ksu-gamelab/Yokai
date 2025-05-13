using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public PlayerUIController uiController;


    void Start()
    {
        uiController.SetStatus(playerStatus); // max値の受け渡し不要
    }

}
