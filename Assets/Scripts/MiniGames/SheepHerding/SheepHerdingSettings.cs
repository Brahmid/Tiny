using Core;
using GUI.Widgets;
using UnityEngine;
using WheelOfFortune;

namespace SheepHerding
{
    [CreateAssetMenu(fileName = "SheepHerdingGame", menuName = "Settings/Games/SheepHerdingGame")]
    public class SheepHerdingSettings : MiniGameSettingsBase<SheepHerdingSettings, SheepHerdingGame, SheepHerdingScreen, long, SheepHerdingWidget.HerdingTrackConfig>
    {
        public SheepHerdingWidget.SheepHerdingConfig RaceConfigs;

        protected override SheepHerdingGame CreateTypedGame(UserService userService)
        {
            return new SheepHerdingGame(this, userService);
        }
    }
}
