namespace Core
{
    using NUnit.Framework;
    using Sirenix.OdinInspector;
	using UnityEngine;

	public class UserService : IInitializable, IUpdatable
	{
		[ShowInInspector]
		public UserData UserData { get; private set; } = new UserData();

		private UserSettings m_UserSettings;

		void IInitializable.Initialize(ServiceLocator locator)
		{
			m_UserSettings = locator.Get<MasterSettings>().UserSettings;
			if(!LoadUser())
				SetStartingUserData();
			
		}

		bool LoadUser()
		{
			if(System.IO.File.Exists(Application.persistentDataPath + "/userData.json"))
			{
				string saveData = System.IO.File.ReadAllText(Application.persistentDataPath + "/userData.json");
                UserData = JsonUtility.FromJson<UserData>(saveData);
				UserData.Initialize();
                return true;
			}

			return false;
		}

		bool SaveUser()
		{
            if (m_UserSettings != null)
            {
				UserData.LogTime();
                string saveData = JsonUtility.ToJson(UserData);
				System.IO.File.WriteAllText(Application.persistentDataPath+"/userData.json", saveData);
                return true;
            }

            return false;
        }

		void IInitializable.Deinitialize()
		{
					
		}

		void IUpdatable.Update(float deltaTime)
		{
			if (UserData.Dirty)
			{
				if(SaveUser())
					UserData.ClearDirty();
			}
		}

		public void ResetUser()
		{
			SetStartingUserData();
		}

		private void SetStartingUserData()
		{
			UserData = new UserData();
			UserData.SetBalance(m_UserSettings.StartBalance);
			
		}
	}
}