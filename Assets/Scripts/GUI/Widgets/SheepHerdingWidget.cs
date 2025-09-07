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
        }

        public async Task AnimateRace(int winIndex)
        {
            List<float> speeds = new List<float>();
            foreach (var dog in m_tracks)
            {
                dog.Reset();
                float speed = UnityEngine.Random.Range(0.03f, 0.04f);                
                speeds.Add(speed);                
            }
            
            speeds[winIndex] = 0.05f;


            bool keepMoving = true;

            while (keepMoving)
            {
                for (int i = 0; i < m_tracks.Count; i++)
                {
                    keepMoving &=  m_tracks[i].Move(speeds[i]*Time.deltaTime);
                }
                await Task.Yield();
            }
        }
    }
}
