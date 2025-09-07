using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using static SheepHerding.SheepHerdingWidget;

public class SheepHerdingTrack : MonoBehaviour
{
    [SerializeField] private Image m_Dog;
    [SerializeField] private Image[] m_Sheeps;
    [SerializeField] private Image m_Track;
    [SerializeField] private TMP_Text m_Text;

    [SerializeField] private Sprite m_StandingSheep;
    [SerializeField] private Sprite m_RunningSheep;

    private int m_StepsCount = 0;
    private List<float> m_Positions = new List<float>() ;

    float m_Distance;

    public void CalculateTrack(int targetFails, int stepsCount)
    {
        m_StepsCount = stepsCount;
        m_Positions.Clear();
        List<int> steps = new List<int>() ;
        for (int i = 0; i < stepsCount; i++)
        {
            if (i < targetFails)
            {
                steps.Add(1);
                continue;
            }
            if (i + 1 < stepsCount)
            {
                if (Random.Range(0f, 1f) < 0.3f)
                {
                    steps.Add(1);
                    steps.Add(3);
                    i++;
                    continue;
                }
                
            }            
            steps.Add(2);
        }
        steps = steps.OrderBy(_ => System.Guid.NewGuid()).ToList();        
        int sum = 0;
        foreach (int step in steps)
        {                      
            sum += step;
            m_Positions.Add(sum / (2f * stepsCount));
        }
    }

    public bool Move(float deltaMove)
    {
        RectTransform dogTrans = m_Dog.rectTransform;
        if (deltaMove < 1)
        {
            float currentStepF = deltaMove * 20;
            int currentStep = Mathf.FloorToInt(currentStepF);
            float currentPosition = Mathf.Lerp(currentStep > 0 ?  m_Positions[currentStep - 1] : 0, m_Positions[currentStep], currentStepF - currentStep);
            Debug.Log(currentPosition);
            
            dogTrans.localPosition =  new Vector3(LerpOnTrack(currentPosition), dogTrans.localPosition.y, dogTrans.localPosition.z);
            dogTrans.rotation = Quaternion.Euler(0, 0, Mathf.Sin(currentPosition * 30) * 15);
            MoveSheeps();

            return true;
        }
        dogTrans.localPosition = new Vector3(LerpOnTrack(m_Positions[m_Positions.Count - 1]), dogTrans.localPosition.y, dogTrans.localPosition.z);
        MoveSheeps();
        return false;
    }

    private void MoveSheeps()
    {
        RectTransform dogTrans = m_Dog.rectTransform;
        foreach (var sheep in m_Sheeps)
        {
            RectTransform sheepTrans = sheep.rectTransform;
            if (sheepTrans.position.x <= dogTrans.position.x + dogTrans.rect.width / 2)
            {
                sheepTrans.position = new Vector3(dogTrans.position.x + dogTrans.rect.width, sheepTrans.position.y, sheepTrans.position.z);
                sheep.sprite = m_RunningSheep;
            }
        }
    }

    public void Reset()
    {

        RectTransform dogTrans = m_Dog.rectTransform;       

        dogTrans.localPosition = new Vector3(LerpOnTrack(0), dogTrans.localPosition.y, dogTrans.localPosition.z);
        dogTrans.rotation = Quaternion.Euler(0, 0, 0);
        foreach (var sheep in m_Sheeps)
        {
            RectTransform sheepTrans = sheep.rectTransform;
            sheepTrans.localPosition = new Vector3(LerpOnTrack(Random.Range(0.3f, 0.8f)), sheepTrans.localPosition.y, sheepTrans.localPosition.z);
            sheep.sprite = m_StandingSheep;

        }
    }

    private float LerpOnTrack(float delta)
    {
        return Mathf.Lerp(-1 * m_Distance / 2, m_Distance / 2, delta);
    }

    internal void Setup(HerdingTrackConfig config)
    {
        m_Dog.sprite = config.Sprite;
        m_Text.text = config.Text;
        m_Distance = m_Track.rectTransform.rect.width;

        foreach (var sheep in m_Sheeps)
        {
            RectTransform sheepTrans = sheep.rectTransform;
            sheepTrans.localPosition = new Vector3(LerpOnTrack(Random.Range(0.3f, 0.8f)), sheepTrans.localPosition.y, sheepTrans.localPosition.z);

        }
    }
}
