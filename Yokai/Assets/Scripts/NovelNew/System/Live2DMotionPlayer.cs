using UnityEngine;

public class Live2DAnimatorPlayer : MonoBehaviour
{
    public Animator animator;
    public string[] stateNames; // Animator に登録したステートの名前
    private int previousIndex = -1; // 前回再生したインデックスを記録

    public float fadeDuration;// ブレンド時間（秒）

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (stateNames.Length == 0) return;

            int newIndex;
            do
            {
                newIndex = Random.Range(0, stateNames.Length); // ランダムにインデックスを選択
            } while (newIndex == previousIndex); // 前回と同じインデックスは避ける

            // "nagisa_initial" にクロスフェード（Layer 0）
            animator.CrossFade("nagisa_initial", fadeDuration, 0);

            // 新しいモーションにクロスフェード（Layer 0）
            animator.CrossFade(stateNames[newIndex], fadeDuration, 0);

            previousIndex = newIndex;
        }
    }
}
