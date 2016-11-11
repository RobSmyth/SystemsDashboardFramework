using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Framework.Config
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

        public void Save(DashboardConfigurations configuration)
        {
            var configPath = GetFilePath();
            var serializer = new XmlSerializer(typeof (DashboardConfigurations));
            using (var writer = new FileStream(configPath, FileMode.Create))
            {
                serializer.Serialize(writer, configuration);
                writer.Close();
            }
        }

        private void SaveDefaultConfiguration(string configPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "NoeticTools.TeamStatusBoard.Framework.dashboard.config.default.xml";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
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
            var configFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"Dashboard");
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }
            return Path.Combine(configFolder, @"dashboard.config.xml");
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