namespace Tests.FunctionalUi;

public class AppSettings
{
	public bool IsHeadless { get; set; } = false;
	public int ActionDelayMs { get; set; } = 200;
	public string BaseUrl { get; set; } = null!;
	public string Username { get; set; } = null!;
	public string Password { get; set; } = null!;
}