using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositoryInitializer.UnitTests
{
    [TestClass]
    public class ReplacerTests
    {
        [TestMethod]
        public void FilterTest()
        {
            const string folder = @"C:\Users\haven\source\repos\HavenDV\CSharpProjectTemplate";
            var paths = new[]
            {
                @$"{folder}\$PROJECT_NAME$.sln",
                @$"{folder}\.gitignore",
                @$"{folder}\LICENSE",
                @$"{folder}\README.md",
                @$"{folder}\template.settings.json",
                @$"{folder}\.git\COMMIT_EDITMSG",
                @$"{folder}\.git\config",
                @$"{folder}\.git\description",
                @$"{folder}\.git\HEAD",
                @$"{folder}\.git\index",
                @$"{folder}\.git\ms-persist.xml",
                @$"{folder}\.git\packed-refs",
                @$"{folder}\.vs\ProjectSettings.json",
                @$"{folder}\.vs\slnx.sqlite",
                @$"{folder}\.vs\VSWorkspaceState.json",
                @$"{folder}\docs\nuget_icon.png",
                @$"{folder}\src\Directory.Build.props",
                @$"{folder}\src\key.snk",
                @$"{folder}\.git\hooks\applypatch-msg.sample",
                @$"{folder}\.git\hooks\commit-msg.sample",
                @$"{folder}\.git\hooks\fsmonitor-watchman.sample",
                @$"{folder}\.git\hooks\post-update.sample",
                @$"{folder}\.git\hooks\pre-applypatch.sample",
                @$"{folder}\.git\hooks\pre-commit.sample",
                @$"{folder}\.git\hooks\pre-merge-commit.sample",
                @$"{folder}\.git\hooks\pre-push.sample",
                @$"{folder}\.git\hooks\pre-rebase.sample",
                @$"{folder}\.git\hooks\pre-receive.sample",
                @$"{folder}\.git\hooks\prepare-commit-msg.sample",
                @$"{folder}\.git\hooks\update.sample",
                @$"{folder}\.git\info\exclude",
                @$"{folder}\.git\logs\HEAD",
                @$"{folder}\.github\workflows\dotnet.yml",
                @$"{folder}\src\libs\Directory.Build.props",
                @$"{folder}\src\tests\Directory.Build.props",
                @$"{folder}\.git\objects\05\b429a27f83af3fbe061d9a35896ff90e2bbb67",
                @$"{folder}\.git\objects\06\8bdd4e3e1e675c919603c438eeec65ca385008",
                @$"{folder}\.git\objects\07\a161c85f62509e9905d521a3e5f1c251348c35",
                @$"{folder}\.git\objects\0d\d43d47ffcc7eb5cff1fa6f29baef92ad9e47bf",
                @$"{folder}\.git\objects\10\fd21982bb0b8233dce591190141336d1f351a1",
                @$"{folder}\.git\objects\11\ba113c88ad632e957ba234a87be92058ac1e64",
                @$"{folder}\.git\objects\13\1979933cb26411417aa4246ad99debfd838391",
                @$"{folder}\.git\objects\13\3a6ceb2ee6ff3b184e114a22771e8705fb9986",
                @$"{folder}\.git\objects\28\f4236116db4ca14cbaaf0da36aaf77230f6087",
                @$"{folder}\.git\objects\2a\7368470479ed0a69a998575edecf96d325df13",
                @$"{folder}\.git\objects\2b\5862d3e46d823211a1e5728c1048d03ab3ef7b",
                @$"{folder}\.git\objects\32\73fa36cd0d2e344bb860780906f8d2b0165b6a",
                @$"{folder}\.git\objects\3b\a18fe5554abb3fa39dbee951de0aaa67043695",
                @$"{folder}\.git\objects\3d\32376a901a83dfe07d64b8a231488f1ad52942",
                @$"{folder}\.git\objects\42\cecdbf4f55381dff8b5107286d5e3d9e9753cf",
                @$"{folder}\.git\objects\46\a385fa768830368543c5d11f4eeefc6e14aebf",
                @$"{folder}\.git\objects\4e\d05db7cb65a943a689be1f4947b6e34a84f5e2",
                @$"{folder}\.git\objects\52\8637281b11677aa529f9dad44d2d3ebbe9d939",
                @$"{folder}\.git\objects\54\95137e14947a5c7dd0252abf3dae0334f9fba2",
                @$"{folder}\.git\objects\5d\8e248c84bc8af1bbb36614f717faff9ab0311c",
                @$"{folder}\.git\objects\5f\89e812df2da22fd836cc7987bc903a8aec4472",
                @$"{folder}\.git\objects\65\50326947f9d87b56d720d12092123f8ea228a6",
                @$"{folder}\.git\objects\66\2f557b83599c8dc37eb8f5ec2f08931e578990",
                @$"{folder}\.git\objects\68\ca6d1d05a0310cbd5a3538f1a2e10629d713ad",
                @$"{folder}\.git\objects\6d\aefdf9cc4e224d59c37b56d30017b93b7bf149",
                @$"{folder}\.git\objects\6e\7c3a80e5262dc09efc0056ed3cf73c7c3b38dc",
                @$"{folder}\.git\objects\71\8763e1e24408fb49264109fd3674c28b40998f",
                @$"{folder}\.git\objects\79\4251ede756ef69cecd1014800f426775c417e3",
                @$"{folder}\.git\objects\79\8ebfc96001ffe977ef913211038eda5a0c8184",
                @$"{folder}\.git\objects\7f\7b70f5f979956c6a9ab6c1512fc00818d845a6",
                @$"{folder}\.git\objects\82\b663e97fbe4434ff3f33137c8196b4c91058ee",
                @$"{folder}\.git\objects\88\3d945e867cf106433a6843eb9e8841cbf097d5",
                @$"{folder}\.git\objects\89\ae3ec16f5053fb4fbc01c9a49d031f5242f17d",
                @$"{folder}\.git\objects\95\54e0c906325f20c0d7c6c285cd644b63127505",
                @$"{folder}\.git\objects\ac\90f5ce9f60b44f8c35a2fedf520f449ca12745",
                @$"{folder}\.git\objects\b3\d9067075dc39188bc03cc802ee8c0441394b31",
                @$"{folder}\.git\objects\b5\7e4a2b35a9876dcb778f3d16599ac60f2d8b73",
                @$"{folder}\.git\objects\ba\f36e9c7c656c81fe382fd2d1cb13c5f7956000",
                @$"{folder}\.git\objects\c0\5d44beda0df6de41e3dfd3f2774042ba05265c",
                @$"{folder}\.git\objects\c1\3a69dc262b2e970bdb6b7709d559eef87fea17",
                @$"{folder}\.git\objects\dd\36749b7ffd7a3a34b2131535acf6b04bf57d67",
                @$"{folder}\.git\objects\e5\2d3a21d9b556bfa2b339fee5b42747c9c03bfd",
                @$"{folder}\.git\objects\e9\f7d65d8dbddafbe4b36358999cfc2681fba75a",
                @$"{folder}\.git\objects\f4\864b61822163d026a2ad6ae3b7eaa9f521e5e8",
                @$"{folder}\.git\objects\f5\4491252829c93f2c3a3c9176ee6a59b312e701",
                @$"{folder}\.git\objects\fa\bc01e557503532f49ce763c60088027f8a202a",
                @$"{folder}\.git\objects\fb\735843be3abc7d5ddefd6d535e8535741c5081",
                @$"{folder}\.git\objects\fc\d8cad7ca2ff3d65b0e31375ae05462e65c531b",
                @$"{folder}\.git\objects\pack\pack-545031a0cc82cb326e394b4952ee7f6bbcd011e7.idx",
                @$"{folder}\.git\objects\pack\pack-545031a0cc82cb326e394b4952ee7f6bbcd011e7.pack",
                @$"{folder}\.git\refs\heads\master",
                @$"{folder}\.vs\CSharpProjectTemplate\v16\.suo",
                @$"{folder}\src\libs\$PROJECT_NAME$\$PROJECT_NAME$.csproj",
                @$"{folder}\src\tests\$PROJECT_NAME$.IntegrationTests\$PROJECT_NAME$.IntegrationTests.csproj",
                @$"{folder}\src\tests\$PROJECT_NAME$.IntegrationTests\Tests.cs",
                @$"{folder}\src\tests\$PROJECT_NAME$.UnitTests\$PROJECT_NAME$.UnitTests.csproj",
                @$"{folder}\src\tests\$PROJECT_NAME$.UnitTests\Tests.cs",
                @$"{folder}\.git\logs\refs\heads\master",
                @$"{folder}\.git\refs\remotes\origin\HEAD",
                @$"{folder}\.git\refs\remotes\origin\master",
                @$"{folder}\.git\logs\refs\remotes\origin\HEAD",
                @$"{folder}\.git\logs\refs\remotes\origin\master",
            };
            var expected = new[]
            {
                @$"{folder}\$PROJECT_NAME$.sln",
                @$"{folder}\LICENSE",
                @$"{folder}\README.md",
                @$"{folder}\template.settings.json",
                @$"{folder}\docs\nuget_icon.png",
                @$"{folder}\src\Directory.Build.props",
                @$"{folder}\src\key.snk",
                @$"{folder}\src\libs\Directory.Build.props",
                @$"{folder}\src\tests\Directory.Build.props",
                @$"{folder}\src\libs\$PROJECT_NAME$\$PROJECT_NAME$.csproj",
                @$"{folder}\src\tests\$PROJECT_NAME$.IntegrationTests\$PROJECT_NAME$.IntegrationTests.csproj",
                @$"{folder}\src\tests\$PROJECT_NAME$.IntegrationTests\Tests.cs",
                @$"{folder}\src\tests\$PROJECT_NAME$.UnitTests\$PROJECT_NAME$.UnitTests.csproj",
                @$"{folder}\src\tests\$PROJECT_NAME$.UnitTests\Tests.cs",
            };

            CollectionAssert.AreEqual(expected, paths.Filter(folder).ToArray());
        }

        [TestMethod]
        public void PrepareReplaceFileNamesTest()
        {
            const string folder = @"C:\Users\haven\source\repos\HavenDV\CSharpProjectTemplate";
            var paths = new[]
            {
                @$"{folder}\$PROJECT_NAME$.sln",
                @$"{folder}\LICENSE",
                @$"{folder}\README.md",
                @$"{folder}\template.settings.json",
                @$"{folder}\docs\nuget_icon.png",
                @$"{folder}\src\Directory.Build.props",
                @$"{folder}\src\key.snk",
                @$"{folder}\src\libs\Directory.Build.props",
                @$"{folder}\src\tests\Directory.Build.props",
                @$"{folder}\src\libs\$PROJECT_NAME$\$PROJECT_NAME$.csproj",
                @$"{folder}\src\tests\$PROJECT_NAME$.IntegrationTests\$PROJECT_NAME$.IntegrationTests.csproj",
                @$"{folder}\src\tests\$PROJECT_NAME$.IntegrationTests\Tests.cs",
                @$"{folder}\src\tests\$PROJECT_NAME$.UnitTests\$PROJECT_NAME$.UnitTests.csproj",
                @$"{folder}\src\tests\$PROJECT_NAME$.UnitTests\Tests.cs",
            };
            var expected = new[]
            {
                (@$"{folder}\$PROJECT_NAME$.sln", @$"{folder}\Test.sln"),
                (@$"{folder}\src\libs\$PROJECT_NAME$\$PROJECT_NAME$.csproj", @$"{folder}\src\libs\Test\Test.csproj"),
                (@$"{folder}\src\tests\$PROJECT_NAME$.IntegrationTests\$PROJECT_NAME$.IntegrationTests.csproj", @$"{folder}\src\tests\Test.IntegrationTests\Test.IntegrationTests.csproj"),
                (@$"{folder}\src\tests\$PROJECT_NAME$.IntegrationTests\Tests.cs", @$"{folder}\src\tests\Test.IntegrationTests\Tests.cs"),
                (@$"{folder}\src\tests\$PROJECT_NAME$.UnitTests\$PROJECT_NAME$.UnitTests.csproj", @$"{folder}\src\tests\Test.UnitTests\Test.UnitTests.csproj"),
                (@$"{folder}\src\tests\$PROJECT_NAME$.UnitTests\Tests.cs", @$"{folder}\src\tests\Test.UnitTests\Tests.cs"),
            };

            CollectionAssert.AreEqual(expected, paths.PrepareReplaceFileNames(new Dictionary<string, string>
            {
                { "$PROJECT_NAME$", "Test" },
            }).ToArray());
        }
    }
}
