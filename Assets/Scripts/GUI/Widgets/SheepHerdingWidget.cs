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
            public Color Color;
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

        List<HerdingTrackConfig> Tracks;
        [SerializeField] SheepHerdingTrack[] m_tracks;

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
                for (int i = 0; i < m_tracks.Length; i++)
                {
                    keepMoving &=  m_tracks[i].Move(speeds[i]*Time.deltaTime);
                }
                await Task.Yield();
            }
        }
    }
}
