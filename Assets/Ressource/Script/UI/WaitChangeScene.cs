using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitChangeScene : MonoBehaviour
{
    [SerializeField] private Image filledImg;

    private void OnEnable()
    {
        StartCoroutine(ChanceSceneWait());
    }

    private IEnumerator ChanceSceneWait()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerMove player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        player.SetStopMove(true);
        CanvasManager.instance.transform.GetComponent<OpenPanelWithKey>().CloseAllPanel();

        float duration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
            filledImg.fillAmount = normalizedTime;
            yield return null; 
        }

        player.SetStopMove(false);
        gameObject.SetActive(false);
    }
}
