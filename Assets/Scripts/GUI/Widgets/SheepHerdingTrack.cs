using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SheepHerdingTrack : MonoBehaviour
{
    [SerializeField] private Image m_Dog;
    [SerializeField] private Image[] m_Sheeps;
    [SerializeField] private Image m_Track;

    float m_Distance;
    float m_Moved = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Distance = m_Track.rectTransform.rect.width;
    }

    public bool Move(float deltaMove)
    {
        m_Moved += deltaMove;
        if (m_Moved < 1)
        {
            m_Dog.rectTransform.localPosition =  new Vector3(LerpOnTrack(m_Moved), m_Dog.rectTransform.localPosition.y, m_Dog.rectTransform.localPosition.z);
            foreach (var sheep in m_Sheeps)
            {
                if (sheep.rectTransform.position.x <= m_Dog.rectTransform.position.x + m_Dog.rectTransform.rect.width / 2)
                {
                    //sheep.rectTransform.SetParent(m_Dog.rectTransform);
                    sheep.rectTransform.position = new Vector3(m_Dog.rectTransform.position.x + m_Dog.rectTransform.rect.width, sheep.rectTransform.position.y, sheep.rectTransform.position.z);

                }

            }
            m_Dog.rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.realtimeSinceStartup * 5) * 15);
            return true;
        }
        m_Dog.rectTransform.localPosition = new Vector3(m_Distance / 2, m_Dog.rectTransform.localPosition.y, m_Dog.rectTransform.localPosition.z);
        return false;
    }

    public void Reset()
    {
        m_Moved = 0f;
        Move(0);
        foreach (var sheep in m_Sheeps)
        {
            sheep.rectTransform.localPosition = new Vector3(LerpOnTrack(Random.Range(0.3f, 0.8f)), sheep.rectTransform.localPosition.y, sheep.rectTransform.localPosition.z);

        }
    }

    private float LerpOnTrack(float delta)
    {
        return Mathf.Lerp(-1 * m_Distance / 2, m_Distance / 2, delta);
    }
}
