using Octokit;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection.Metadata.Ecma335;

public class UpdateManager
{
	GitHubClient client;
	Task<IReadOnlyList<Release>> _releases;
	Task<IReadOnlyList<ReleaseAsset>> _releaseAssets;
	string extractPath;
	string versionString;

	readonly string EMULATOR = "Project64";
	readonly string REPO_OWNER = "LunaticShiN3";
	readonly string REPO_NAME = "Luna-Project64";

	readonly string[] REPLACEABLE_FILE_NAMES = ["dgVoodoo for Jabo.zip", "parasettings.ini", "Project64.exe", "Project64.rdb"];
	readonly string[] REPLACEABLE_FOLDER_NAMES = [Path.Combine("Config", "Cheats"), Path.Combine("Config", "Enhancements"), "Lang", "Plugin"];

	public UpdateManager(string versionString)
	{
		this.client = new GitHubClient(new ProductHeaderValue("PJ64-Auto-Updater"));
		this._releases = client.Repository.Release.GetAll(REPO_OWNER, REPO_NAME);
		this._releaseAssets = client.Repository.Release.GetAllAssets(REPO_OWNER, REPO_NAME, _releases.Result[0].Id);
		this.extractPath = Directory.GetCurrentDirectory();
		this.versionString = versionString;
	}

	private GitHubClient GetGitHubClient() { return client; }
	private Task<IReadOnlyList<Release>> GetReleases() { return _releases; }
	private Task<IReadOnlyList<ReleaseAsset>> GetReleaseAssets() { return _releaseAssets; }
	private string getExtractPath() { return extractPath; }
	private string getVersionString() {  return versionString; }

	public bool UpdateEmulator()
	{
		if(_releases.Result[0].TagName != getVersionString())
		{
			extractPath = NormalizePathName(extractPath);

			try
			{
				KillEmulatorProcess();
				DeleteEmulator();
				DownloadMostRecentVersion();
				ExtractZipFile();
				DeleteZipFile();
				StartUpEmulator();
				return true;

			}
			catch (Exception e)
			{
				Console.WriteLine("Could Not Update");
				Console.WriteLine(e.Message);
                return false;
			}
		}
		return false;
	}

	private void KillEmulatorProcess()
	{
		if (Process.GetProcessesByName(EMULATOR).Length > 0)
		{
			foreach (Process emulator in Process.GetProcessesByName(EMULATOR))
			{
				emulator.Kill();
				Console.WriteLine($"Killed {EMULATOR}");
			}
		}
	}

	private void DeleteEmulator()
	{
		foreach (var fileName in REPLACEABLE_FILE_NAMES)
		{
			if(File.Exists(fileName))
				File.Delete(Path.Combine(getExtractPath(), fileName));
		}
		foreach (var dirName in REPLACEABLE_FOLDER_NAMES)
		{
			if(Directory.Exists(dirName))
				Directory.Delete(Path.Combine(getExtractPath(), dirName), true);
		}
	}

	private void DownloadMostRecentVersion()
	{
		using (var webClient = new WebClient())
		{
			webClient.DownloadFile(GetReleaseAssets().Result[0].BrowserDownloadUrl, GetReleaseAssets().Result[0].Name);
		}
	}

	private string NormalizePathName(string extractPath)
	{
		if (!extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
			extractPath += Path.DirectorySeparatorChar;
		return extractPath;
	}

	private void ExtractZipFile()
	{
		using (ZipArchive archive = ZipFile.OpenRead(GetReleaseAssets().Result[0].Name))
		{

			foreach (ZipArchiveEntry entry in archive.Entries)
			{

				foreach (var item in REPLACEABLE_FILE_NAMES)
				{
					if(entry.FullName.Contains(item))
					{
						string destinationPath = Path.GetFullPath(Path.Combine(getExtractPath(), entry.Name));

						entry.ExtractToFile(destinationPath);
					}
				}

				foreach (var item in REPLACEABLE_FOLDER_NAMES)
				{
					string destinationPath = Path.GetFullPath(Path.Combine(getExtractPath(), item));

					if (!Directory.Exists(destinationPath))
					{
						Directory.CreateDirectory(destinationPath);
					}

					if (entry.FullName.Contains(item) && !entry.FullName.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
					{
						string[] names = entry.FullName.Split('/');
						string name = names[names.Length - 2];
						destinationPath = Path.GetFullPath(Path.Combine(getExtractPath(), name));

						//Throws Error???
						entry.ExtractToFile(@destinationPath);
					}
				}
			}
        }
	}

	private void DeleteZipFile()
	{
		File.Delete(GetReleaseAssets().Result[0].Name);
	}

	private void StartUpEmulator()
	{
		Process.Start(getExtractPath() + $"{EMULATOR}.exe");
	}
}
