using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Bobbing : MonoBehaviour {
  [SerializeField] private RectTransform hand;
  [SerializeField] private float amplitudeY = 10f;   // up/down bob distance
  [SerializeField] private float amplitudeX = 5f;    // side sway distance
  [SerializeField] private float duration = 0.5f;    // how long one bob lasts

  private Vector2 lastPos;
  private bool isBobbing;
  private int direction = 1; // used to flip X axis each time

  void Start() {
    lastPos = hand.anchoredPosition;
  }

  public void Bob() {
    if (!isBobbing)
      StartCoroutine(BobRoutine());
  }

  IEnumerator BobRoutine() {
    isBobbing = true;
    float timer = 0f;
    Vector2 startPos = lastPos;

    while (timer < duration) {
      timer += Time.deltaTime;
      float progress = timer / duration;

      // Y bob
      float offsetY = Mathf.Sin(progress * Mathf.PI) * amplitudeY;
      // X sway alternates direction
      float offsetX = Mathf.Cos(progress * Mathf.PI) * amplitudeX * direction;

      hand.anchoredPosition = startPos + new Vector2(offsetX, offsetY);

      yield return null;
    }

    // --- FIX: snap to exact intended final position ---
    float finalOffsetY = Mathf.Sin(Mathf.PI) * amplitudeY; // = 0
    float finalOffsetX = Mathf.Cos(Mathf.PI) * amplitudeX * direction; // = -amplitudeX * direction
    hand.anchoredPosition = startPos + new Vector2(finalOffsetX, finalOffsetY);

    // Update lastPos and flip direction for next bob
    lastPos = hand.anchoredPosition;
    direction *= -1;

    isBobbing = false;
  }
}
