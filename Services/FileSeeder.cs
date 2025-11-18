using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace journal2.Services
{
    public interface IFileSeeder
    {
        Task SeedAsync();
    }
    public class FileSeeder : IFileSeeder
    {
        public async Task SeedAsync()
        {
            var appDir = FileSystem.AppDataDirectory;
            var files = new[] { "Jungle_ch1.txt",
                                "Jungle_ch2.txt",
                                "Jungle_ch3.txt",
                                "Jungle_contents.txt" };

            foreach (var file in files)
            {
                var target = Path.Combine(appDir, file);
                if (!File.Exists(target))
                {
                    using var input = await FileSystem.OpenAppPackageFileAsync(file);
                    using var output = File.Create(target);
                    await input.CopyToAsync(output);
                }
            }


        }
    }
}