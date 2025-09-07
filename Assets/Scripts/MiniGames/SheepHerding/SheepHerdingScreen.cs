using Core;
using GUI.Widgets;
using System;
using System.Threading.Tasks;
using UnityEngine;
using WheelOfFortune;

namespace SheepHerding
{
    [CreateAssetMenu(fileName = "HerdingGameSettings", menuName = "Settings/Games/HerdingGameSettings")]
    public class SheepHerdingScreen : MiniGameScreenView<SheepHerdingSettings, SheepHerdingGame, long, SheepHerdingWidget.HerdingTrackConfig>
    {

        [SerializeField] private BetAndPlayWidget m_BetAndPlayWidget;
        [SerializeField] private SheepHerdingWidget m_field;


        internal override void Initialize(ScreenStack stack, ServiceLocator locator)
        {
            base.Initialize(stack, locator);
            if (m_BetAndPlayWidget == null)
                throw new Exception("Game needs Bet and Play Widget!");

            // Connect game starting event
            m_BetAndPlayWidget.SignalPlayClicked += PlayGameRound;
        }

        internal override void Deinitialize()
        {
            base.Deinitialize();

            m_BetAndPlayWidget.SignalPlayClicked -= PlayGameRound;
        }

        public override void BindScreenData(SheepHerdingGame game)
        {
            base.BindScreenData(game);

            m_field.Setup(game.Settings.RaceConfigs);
        }

        protected override async Task AnimateGameRound(SheepHerdingWidget.HerdingTrackConfig winResult)
        {
            int winIndex = m_Game.Settings.RaceConfigs.Tracks.IndexOf(winResult);
            await m_field.AnimateRace(winIndex);
            Debug.Log("Race finished");
        }

        protected override long GetRoundBet()
        {
            return m_BetAndPlayWidget.BetValue;
        }

    }
}
