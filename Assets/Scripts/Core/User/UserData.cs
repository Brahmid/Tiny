namespace Core
{
	using System;
	using Sirenix.OdinInspector;
    using Sirenix.Utilities;
    using UnityEngine;

	[Serializable]
	public class UserData
	{
        [Serializable]
        public class UserStatistics
        {
            public int BetsCount;
            public int BetsWin;
			public double GameTime;		
					
            public UserStatistics()
			{
                GameTime = 0f;
                BetsCount = 0;
                BetsWin = 0;
            }

			UserStatistics(double time, int betsCount,int betsWin)
			{
                GameTime = time;
				BetsCount = betsCount;
				BetsWin = betsWin;
			}

			

            public static UserStatistics operator +(UserStatistics left, UserStatistics right)
            {
                return new UserStatistics(
                        left.GameTime + right.GameTime,
                        left.BetsCount + right.BetsCount,
                        left.BetsWin + right.BetsWin
                        );
            }
        }

		// Serialized fields
		
		[SerializeField, HideInInspector] private long m_Balance;
		[SerializeField, HideInInspector] private int m_BetIndex;

        //Statistics
        [SerializeField, HideInInspector] private int m_StartsCount = 0;
        [SerializeField, HideInInspector] private UserStatistics m_SessionStats = new UserStatistics();
		[SerializeField, HideInInspector] private UserStatistics m_AllTimeStats = new UserStatistics();


        // Public properties (getters)
        [ShowInInspector, ReadOnly]  public long Balance => m_Balance;
		[ShowInInspector, ReadOnly]  public int BetIndex => m_BetIndex;

		public bool Dirty { get; private set; }
		
		
		// Signals
		public Action SignalBalanceChanged { get; set; }
		public Action SignalBetIndexChanged { get; set; }

		
		// Data changing methods
		[Button]
		public void SetBalance(long value)
		{
			m_Balance = value;
			AfterBalanceChanged();
		}
		
		[Button]
		public void AddBalance(long value)
		{
			m_Balance += value;
			AfterBalanceChanged();
		}

		[Button]
		public void SetBetIndex(int index)
		{
			m_BetIndex = index;
			AfterBetChanged();
		}

		public void ClearDirty()
		{
			SetDirty(false);
		}
		
		
		// private methods
		private void AfterBalanceChanged()
		{
			if (m_Balance < 0)
			{
				Debug.LogError("Balance got negative - that shouldn't happen!");
				m_Balance = 0;
			}
			
			if (SignalBalanceChanged != null)
			{
				SignalBalanceChanged.Invoke();
			}

			SetDirty();
		}

		private void AfterBetChanged()
		{
			if (m_BetIndex < 0)
			{
				Debug.LogError("Bet index got negative - that shouldn't happen!");
				m_BetIndex = 0;
			}
			
			if (SignalBetIndexChanged != null)
			{
				SignalBetIndexChanged.Invoke();
			}
			
			SetDirty();
		}
		
		private void SetDirty(bool dirty = true)
		{
			Dirty = dirty;
		}

        public void LogBet(long bet, int winPercent)
        {
            if(winPercent > 0)
			{
				m_SessionStats.BetsWin++;
            }
            m_SessionStats.BetsCount++;
        }

        public void Initialize()
        {
			m_StartsCount++;
            m_AllTimeStats = m_AllTimeStats + m_SessionStats;
			m_SessionStats = new UserStatistics();
			SetDirty();
        }

        internal void LogTime()
        {
			m_SessionStats.GameTime = Time.realtimeSinceStartupAsDouble;
        }
    }
}