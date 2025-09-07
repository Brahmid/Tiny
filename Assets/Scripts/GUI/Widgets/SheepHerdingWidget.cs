using Core;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Core.Utility;
using System.Threading.Tasks;

namespace SheepHerding
{
    public class SheepHerdingWidget : ScreenWidget
    {
        [Serializable]
        public class HerdingTrackConfig : IWinResult, IWeightRandomItem
        {
            public int WinPercent;            
            public Sprite Sprite;
            public string Text;
            public float RandomWeight = 1f;

            int IWinResult.WinPercent => WinPercent;
            float IWeightRandomItem.Weight => RandomWeight;
        }

        [Serializable]
        public class SheepHerdingConfig
        {
            [TableList]
            public List<HerdingTrackConfig> Tracks;           

        }


        [SerializeField] private GameObject m_TrackPrefab;
        [SerializeField] private RectTransform m_Field;
        [SerializeField] private float m_RoundDuration = 10;
        [SerializeField] private float m_RoundTime = 0;

        private bool m_FirstRun = true;

        //private List<HerdingTrackConfig> m_Tracks;
        private List<SheepHerdingTrack> m_tracks = new List<SheepHerdingTrack>();

        public void Setup(SheepHerdingConfig config)
        {
            float spaceBetweenTracks = m_Field.rect.height/ config.Tracks.Count;
            float offsetTracks = -m_Field.rect.height / 2f;

            for (int index = 0; index < config.Tracks.Count; index++)
            {
                GameObject track = GameObject.Instantiate(m_TrackPrefab, m_Field);                
                track.transform.localPosition = new Vector3(0, (index * spaceBetweenTracks + spaceBetweenTracks / 2f) + offsetTracks, 0);
                SheepHerdingTrack script = track.GetComponent<SheepHerdingTrack>();
                if (script != null)
                {
                    m_tracks.Add(script);
                    script.Setup(config.Tracks[index]);
                }
               
            }
            m_TrackPrefab.SetActive(false);
            m_FirstRun = true;
        }

        public async Task AnimateRace(int winIndex)
        {            
            m_RoundTime = 0;
            for(int index = 0; index < m_tracks.Count; index++)
            {
                if(!m_FirstRun)
                    m_tracks[index].Reset();
                if (index != winIndex)
                {
                    m_tracks[index].CalculateTrack(UnityEngine.Random.Range(4, 10), 20);
                }
                else
                {
                    m_tracks[index].CalculateTrack(0, 20);
                }
            }          
            
            bool keepMoving = true;

            while (keepMoving)
            {
                m_RoundTime += Time.deltaTime;
                for (int i = 0; i < m_tracks.Count; i++)
                {
                    keepMoving &=  m_tracks[i].Move(m_RoundTime/m_RoundDuration);
                }
                await Task.Yield();
            }
            m_FirstRun = false;
        }
    }
}
