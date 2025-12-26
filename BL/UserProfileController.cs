using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.MemoGame.BL
{
	internal class UserProfileController
	{
		private IDataAccessObject _dao;

		private IUserProfile? currentUserProfile;

		public UserProfileController(IDataAccessObject dao) 
		{
			_dao = dao;
			currentUserProfile = dao.GetFirstUserProfile();
		}
		
		public IUserProfile GetCurrentUserProfile()
		{
			return currentUserProfile ?? throw new InvalidOperationException("No user profile is currently set.");
		}

		public IEnumerable<IUserProfile> GetAllUserProfiles()
		{
			return _dao.GetAllUserProfiles();
		}

		public void SetCurrentUserProfile(IUserProfile user)
		{
			currentUserProfile = user;
		}

		public IUserProfile CreateNewUserProfile(string userName)
		{
			var newUser = _dao.CreateNewUserProfile(userName);
			return newUser;
		}

		public void DeleteUserProfile(IUserProfile userProfile)
		{
			_dao.DeleteUserProfile(userProfile);
			if (currentUserProfile == userProfile)
			{
				currentUserProfile = _dao.GetFirstUserProfile(); // fallback to first user profile
			}
		}

		public void UpdateUserProfile(IUserProfile userProfile, string newUsername)
		{
			var updatedProfile = userProfile;
			updatedProfile.UserName = newUsername.Trim();
			_dao.UpdateUserProfile(updatedProfile);
		}

		public IEnumerable<IUserProfile> GetCurrentlyPlayingUsers()
		{
			return new List<IUserProfile>()
			{
				currentUserProfile
			};
		}
	}
}
