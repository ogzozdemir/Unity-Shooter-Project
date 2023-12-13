using UnityEngine;

public class HideSelf : MonoBehaviour
{
    private float timer;

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            timer += Time.deltaTime;

            if (timer > 2f)
                Hide();   
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        timer = 0;
    }
}
