using Octokit;
using System.Diagnostics;
using System.Net;
using System.IO.Compression;

UpdateManager um = new UpdateManager("v3b");
um.UpdateEmulator();
/*GitHubClient client = new GitHubClient(new ProductHeaderValue("app"));

Task<IReadOnlyList<Release>> versionTask = client.Repository.Release.GetAll("LunaticShiN3", "Luna-Project64");
Task<IReadOnlyList<ReleaseAsset>> assetTask = client.Repository.Release.GetAllAssets("LunaticShiN3", "Luna-Project64", versionTask.Result[0].Id);

try
{
	//Step 2: Kill Project64 Processes if such exist
	if(Process.GetProcessesByName("Project64").Length > 0)
	{
		foreach (Process emulator  in Process.GetProcessesByName("Project64"))
		{
			emulator.Kill();
			Console.WriteLine("Killed Project64");
		}
	}

	//Step 5: Delete Project64.exe
	string extractPath = Directory.GetCurrentDirectory();
	File.Delete(Path.Combine(extractPath, "Project64.exe"));

	//Step 3: Download the most recent Version from Github
	using (var webClient =  new WebClient())
	{
		webClient.DownloadFile(assetTask.Result[0].BrowserDownloadUrl, assetTask.Result[0].Name);
		Console.WriteLine("Done Downloading!");
	}

	//Step 4: Ensure that the path name is properly formatted
	if (!extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
		extractPath += Path.DirectorySeparatorChar;



	//Step 6: Extract the zip-File
	using (ZipArchive archive = ZipFile.OpenRead(assetTask.Result[0].Name))
	{
		foreach(ZipArchiveEntry entry in archive.Entries) 
		{
			if(entry.Name == "Project64.exe")
			{
				string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.Name));
                Console.WriteLine(entry.Name);

                if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal)) Console.WriteLine();
                entry.ExtractToFile(destinationPath);
            }
		}
	}
	Console.WriteLine("Done Extracting!");

	//Step 7: Delete the downloaded zip-File
	File.Delete(assetTask.Result[0].Name);
	Console.WriteLine("Done Deleting!");

	//Step 8: Start Project64 again
	Process.Start(extractPath + "Project64.exe");
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}
*/


Environment.Exit(0);