using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Dashboard.Config;

namespace Dashboard.Framework.Config
{
    public class DashboardConfigurationManager
    {
        public DashboardConfigurations Load()
        {
            var configPath = GetFilePath();
            if (!File.Exists(configPath))
            {
                try
                {
                    SaveDefaultConfiguration(configPath);
                    Load();
                }
                catch (Exception)
                {
                    Save(CreateDefaultConfig());
                }
            }

            DashboardConfigurations configuration = null;
            try
            {
                var serializer = new XmlSerializer(typeof (DashboardConfigurations));
                using (var reader = new FileStream(configPath, FileMode.Open))
                {
                    configuration = (DashboardConfigurations) serializer.Deserialize(reader);
                    reader.Close();
                }

                if (string.IsNullOrWhiteSpace(configuration.Current) ||
                    !configuration.Configurations.Any(
                        x => configuration.Current.Equals(x.Name, StringComparison.InvariantCulture)))
                {
                    configuration.Current = configuration.Configurations.Length > 0
                        ? configuration.Configurations.First().Name
                        : "default";
                }
            }
            catch (Exception)
            {
                configuration = CreateDefaultConfig();
            }

            return configuration;
        }

        private void SaveDefaultConfiguration(string configPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "NoeticTools.Dashboard.Framework.dashboard.config.default.xml";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                var defaultContent = reader.ReadToEnd();
                using (var output = File.CreateText(configPath))
                {
                    output.Write(defaultContent);
                }
            }
        }

        private string GetFilePath()
        {
            string configFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"Dashboard");
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }
            return Path.Combine(configFolder, @"dashboard.config.xml");
        }

        public void Save(DashboardConfigurations configuration)
        {
            string configPath = GetFilePath();
            var serializer = new XmlSerializer(typeof (DashboardConfigurations));
            using (var writer = new FileStream(configPath, FileMode.Create))
            {
                serializer.Serialize(writer, configuration);
                writer.Close();
            }
        }

        private DashboardConfigurations CreateDefaultConfig()
        {
            var configuration = new DashboardConfigurations();
            configuration.Configurations = new[]
            {new DashboardConfiguration {Name = "default"}, new DashboardConfiguration {Name = "one"}};
            return configuration;
        }
    }
}