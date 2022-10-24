using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryUI : MonoBehaviour
{
    public GameObject[] Story;

    public int curStory;

    private void Start()
    {
        curStory = 0;
        Story[0].SetActive(true);

        StartCoroutine(NextStory());
        
    }
    private IEnumerator NextStory() // 컨트롤러 버튼 누르면 넘길 수 있었으면 좋겠음
    {
        yield return new WaitForSeconds(8f);
 
        while(curStory < Story.Length-1) 
        { 
            Story[curStory].SetActive(false); 
            ++curStory;
            Story[curStory].SetActive(true);

            yield return new WaitForSeconds(5f);
        }

        Story[curStory].SetActive(false);
        yield break;
    }
}
