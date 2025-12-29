public class SettingsMenu : MenuNavigation
{
	private bool isSave;

	private void OnEnable()
	{
		isSave = false;
	}
	private void OnDisable() 
	{
		if (isSave || SettingsGame.Instance == null || SoundSingleton.Instance == null || MusicSingleton.Instance == null)
			return;

		SettingsGame.Instance.Cancel();
	}

	public void OnOk()
	{
		isSave = true;

		Message.Saving("GoodSave", SettingsGame.Instance.Save());
	}
}
