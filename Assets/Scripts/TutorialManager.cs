using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public Sprite[] planeSprites;

    public GameObject[] panels;
    public Animator transition;
    int ind = 0;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = planeSprites[PlayerPrefs.GetInt("currentPlane")];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && ind < panels.Length-1)
        {
            ind++;
            panels[ind].SetActive(true);
            panels[ind - 1].SetActive(false);
        }

        transform.Translate(Vector3.left * 7f * Time.deltaTime);
    }

    public void OnClick()
    {
        StartCoroutine(LoadLevel("MainGame"));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}