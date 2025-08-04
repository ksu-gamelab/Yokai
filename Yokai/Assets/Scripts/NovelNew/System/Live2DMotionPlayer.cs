using Live2D.Cubism.Framework.Motion;
using Unity.VisualScripting;
using UnityEngine;
public class Live2DMotionPlayer : MonoBehaviour
{
    CubismMotionController _motionController;
    public AnimationClip animations;
    public GameObject model;
    private void Start()
    {
        _motionController = model.GetComponent<CubismMotionController>();
    }
    private void Update()
    {
        //マウスを押したら
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("マウスが押されました");
            PlayMotion(animations);
        }
    }
    public void PlayMotion(AnimationClip animation)
    {
        if ((_motionController == null) || (animation == null))
        {
            return;
        }
        _motionController.PlayAnimation(animation, isLoop: false);
    }
}