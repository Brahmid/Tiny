using Core;
using Core.Utility;
using GUI.Widgets;
using UnityEngine;
using WheelOfFortune;

namespace SheepHerding
{

    public class SheepHerdingGame : MiniGameBase<SheepHerdingSettings, long, SheepHerdingWidget.HerdingTrackConfig>
    {
        private UserService userService;        

        public SheepHerdingGame(SheepHerdingSettings settings, UserService userService) : base(settings, userService)
        {
        }

        public override SheepHerdingWidget.HerdingTrackConfig GenerateWinResult()
        {
           return Settings.RaceConfigs.Tracks.SelectWeightedRandom(m_Random);
        }

        public override void ProcessWinResult(long bet, SheepHerdingWidget.HerdingTrackConfig winResult)
        {
            m_UserData.AddBalance(bet * winResult.WinPercent / 100);
            m_UserData.LogBet(bet, winResult.WinPercent);
        }

        public override bool TryToPlaceBet(long bet)
        {
            if(m_UserData.Balance < bet)
            {                
                return false;
            }

            m_UserData.AddBalance(-bet);
            return true;
        }        
    }
}
