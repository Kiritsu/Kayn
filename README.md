# Kayn

> A Discord/Twitch bot made with DSharpPlus and Qmmands running on ASP.NET Core (.NET 5)

[![Build Status](https://dev.azure.com/allanmercou/Kayn/_apis/build/status/Kiritsu.Kayn?branchName=master)](https://dev.azure.com/allanmercou/Kayn/_build/latest?definitionId=12&branchName=master)
[![Discord Server](https://img.shields.io/discord/223501629008248832?style=flat)](https://discord.gg/UugbeH8)

## Requirements

I do not provide any artifact of the program. You will have to build it by yourself. The following tools are needed:
- [Git](https://git-scm.com/)
- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
- [Visual Studio 2019](https://visualstudio.microsoft.com/vs/), [Rider](https://www.jetbrains.com/rider/) or any good IDE.
- [Discord Bot Application](https://discord.com/developers/applications)

### Self-Compilation

1. Clone the project: `git clone https://github.com/Kiritsu/Kayn`
2. Open your IDE to compile or use `dotnet build`

You can publish the app with: `dotnet publish --framework net5.0 --configuration Release --runtime linux-x64`. For a list of runtime IDs, please see the [.NET Core RID Catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog).

3. Create a json file and fill the following values:
```
{
	"Discord" : {
		"Game": "you.",
		"Token": "",
		"Prefix": "!",
		"ClientId": "client_id",
		"ClientSecret": "client_secret"
	},
	"Database" : {
		"Provider": "Postgresql",
		"Hostname": "localhost",
		"Database": "kayn",
		"Port": 5432,
		"Username": "kayn",
		"Password": "p4$$vv0rd"
	}
}
```
ClientId and ClientSecret are used to OAuth2 authentication with the Web project.

Either rename that file to `kayn.json` and locate it next to your executable, or create an environment variable `KAYN_HOME` pointing to that file. The environment variable has a priority over the other option.

## Contributing

Since the project is at an early stage, I'm not really accepting contributions for now. Expect them to be open when the database and logging (microservice) systems are done. Issues are available if you have any question/suggestion, etc.
