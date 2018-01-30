using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Localization.SqlLocalizer.DbStringLocalizer;


namespace DataATest.Data
{
    public class DataSeeder
    {

        private readonly LocalizationModelContext _ltx;
        private IStringExtendedLocalizerFactory _stringExtendedLocalizerFactory;
        private string location;
        private string filepath;
        private string directory;
        public DataSeeder(LocalizationModelContext ltx, IStringExtendedLocalizerFactory stringExtendedLocalizerFactory)
        {
            _ltx = ltx;
            _stringExtendedLocalizerFactory = stringExtendedLocalizerFactory;
            location = System.Reflection.Assembly.GetEntryAssembly().Location;
            directory = System.IO.Path.GetDirectoryName(location);
            filepath = directory + "\\Data\\";
        }

        public async Task Seed()
        {
            _ltx.Database.EnsureCreated();

            if (!_ltx.LocalizationRecords.Any())
            {
                
                var json = File.ReadAllText(filepath + "Localization.json");


                var cultures = JsonConvert.DeserializeObject<List<LocalizationRecord>>(json);

                _stringExtendedLocalizerFactory.UpdatetLocalizationData(cultures, "Information");
                

            }
        }
    }
}
