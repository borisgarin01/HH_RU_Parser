using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;

namespace HH_RU_Parser.Models
{
    public class VacansiesPrinter
    {
        StringBuilder sb = new();
        StringBuilder sb2 = new();

        public void PrintVacansiesInFile(Root root, string fileNamePlusExtension)
        {
            string output = GetFormedStringBuilderVacanciesInfoOutput(root, fileNamePlusExtension);
            string path = Path.Combine(Directory.GetCurrentDirectory(), fileNamePlusExtension);


            using (StreamWriter streamWriter = File.AppendText(path))
            {
                streamWriter.WriteLine(output);
            }
        }

        public void PrintVacansiesInTable(Root root)
        {
            //TODO: ADD UNIQUE CHECKING BY ID AS PRIMARY KEY
            string connectionString = "Server=127.0.0.1;Port=5432;Database=VacanciesDb;User Id=borisgarin;Password=Cyber2001;";

            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);

            npgsqlConnection.Open();

            string npgsqlCommand = "INSERT INTO Vacancies(Premium, Name, Has_Test, Response_Letter_Required, Published_At, Created_At, Archived, Apply_Alternate_URL, URL, Alternate_URL, Accept_Temporary, Id, AreaId, AreaName, AddressBuilding, AddressCity, AddressStreet) Values (@Premium, @Name, @Has_Test, @Response_Letter_Required, @Published_At, @Created_At, @Archived, @Apply_Alternate_URL, @URL, @Alternate_URL, @Accept_Temporary, @Id, @AreaId, @AreaName, @AddressBuilding, @AddressCity, @AddressStreet) ON CONFLICT DO NOTHING";

            foreach (var item in root.items)
            {
                var insertionArguments = new
                {
                    Id = item.id,
                    Premium = item.premium,
                    Name = item.name,
                    Has_Test = item.has_test,
                    Response_Letter_Required = item.response_letter_required,
                    Published_At = string.IsNullOrWhiteSpace(item.published_at) ? "" : item.published_at,
                    Created_At = string.IsNullOrWhiteSpace(item.created_at) ? "" : item.created_at,
                    Archived = item.archived,
                    Apply_Alternate_URL = string.IsNullOrWhiteSpace(item.apply_alternate_url) ? "" : item.apply_alternate_url,
                    Url = string.IsNullOrWhiteSpace(item.url) ? "" : item.url,
                    Alternate_URL = string.IsNullOrWhiteSpace(item.alternate_url) ? "" : item.alternate_url,
                    Accept_Temporary = item.accept_temporary,
                    AreaId = item.area == null ? "" : string.IsNullOrWhiteSpace(item.area.id) ? "" : item.area.id,
                    AreaName = item.area == null ? "" : string.IsNullOrWhiteSpace(item.area.name) ? "" : item.area.name,
                    AddressBuilding = item.address == null ? "" : string.IsNullOrWhiteSpace(item.address.building) ? "" : item.address.building,
                    AddressCity = item.address == null ? "" : string.IsNullOrWhiteSpace(item.address.city) ? "" : item.address.city,
                    AddressStreet = item.address == null ? "" : string.IsNullOrWhiteSpace(item.address.street) ? "" : item.address.street,
                };
                npgsqlConnection.Execute(npgsqlCommand, insertionArguments);
            }
        }

        public void PrintResultInConsole(string fileNamePlusExtension)
        {
            using (StreamReader streamReader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), fileNamePlusExtension)))
            {
                string allVacancies = streamReader.ReadToEnd();
                Console.WriteLine(allVacancies);
            }
        }

        public void PrintEmployeesInFile(Root root, string fileNamePlusExtension)
        {
            string output = GetFormedStringBuilderEmployeesInfoOutput(root, fileNamePlusExtension);
            string path = Path.Combine(Directory.GetCurrentDirectory(), fileNamePlusExtension);


            using (StreamWriter streamWriter = File.AppendText(path))
            {
                streamWriter.WriteLine(output);
            }
        }

        public void PrintEmpoyeesInConsole(Root root, string fileNamePlusExtension)
        {
            using (StreamReader streamReader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), fileNamePlusExtension)))
            {
                string allEmployees = streamReader.ReadToEnd();
                Console.WriteLine(allEmployees);
            }
        }

        public void PrintEmployersInTable(Root root)
        {
            //TODO: ADD UNIQUE CHECKING BY ID AS PRIMARY KEY
            string connectionString = "Server=127.0.0.1;Port=5432;Database=VacanciesDb;User Id=borisgarin;Password=Cyber2001;";

            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);

            npgsqlConnection.Open();

            string npgsqlCommand = "INSERT INTO Employers(Id, Name, Url, Alternate_Url, Vacancies_Url, Trusted) Values (@Id, @Name, @Url, @Alternate_Url, @Vacancies_Url, @Trusted) ON CONFLICT DO NOTHING";

            foreach (var item in root.items)
            {
                var insertionArguments = new
                {
                    Id = item.employer.id,
                    Name = item.employer.name,
                    Url = item.employer.url,
                    Alternate_Url = item.employer.alternate_url,
                    Vacancies_Url = item.employer.vacancies_url,
                    Trusted = item.employer.trusted
                };
                try
                {
                    npgsqlConnection.Execute(npgsqlCommand, insertionArguments);
                }
                catch
                {

                }
            }
        }

        private string GetFormedStringBuilderVacanciesInfoOutput(Root root, string fileNamePlusExtension)
        {
            sb.Clear();

            string path = Path.Combine(Directory.GetCurrentDirectory(), fileNamePlusExtension);

            string fileContent = string.Empty;

            if (!File.Exists(path))
            {
                File.Create(path);
            }
            string text = File.ReadAllText(path);

            foreach (var item in root.items)
            {
                if (!text.Contains(item.name) && item.employer != null && !text.Contains(item.employer.name))
                {
                    if (!string.IsNullOrWhiteSpace(item.name))
                        sb.AppendLine($"{item.name} ");
                    if (item.accept_temporary)
                        sb.AppendLine($"accept temporary");
                    if (item.address != null)
                    {
                        if (!string.IsNullOrWhiteSpace(item.address.city))
                            sb.AppendLine(item.address.city);
                        if (!string.IsNullOrWhiteSpace(item.address.street))
                            sb.AppendLine(item.address.street);
                        if (!string.IsNullOrWhiteSpace(item.address.building))
                            sb.AppendLine(item.address.building);
                        if (item.address.metro != null)
                        {
                            if (!string.IsNullOrWhiteSpace(item.address.metro.station_name))
                                sb.AppendLine(item.address.metro.station_name);
                            if (!string.IsNullOrWhiteSpace(item.address.metro.line_name))
                                sb.AppendLine(item.address.metro.station_name);
                        }
                        foreach (MetroStation metroStation in item.address.metro_stations)
                        {
                            if (!string.IsNullOrWhiteSpace(metroStation.line_name))
                                sb.AppendLine(metroStation.line_name);
                            if (!string.IsNullOrWhiteSpace(metroStation.station_name))
                                sb.AppendLine(metroStation.station_name);
                            if (!string.IsNullOrWhiteSpace(metroStation.line_id))
                                sb.AppendLine(metroStation.line_id);
                            sb.AppendLine(metroStation.lat.ToString());
                            sb.AppendLine(metroStation.lng.ToString());
                            if (!string.IsNullOrWhiteSpace(metroStation.station_id))
                                sb.AppendLine(metroStation.station_id);
                            if (!string.IsNullOrWhiteSpace(metroStation.station_name))
                                sb.AppendLine(metroStation.station_name);
                        }
                        if (!string.IsNullOrWhiteSpace(item.address.raw))
                            sb.AppendLine(item.address.raw);
                        if (!string.IsNullOrWhiteSpace(item.address.street))
                            sb.AppendLine(item.address.street);
                        if (!string.IsNullOrWhiteSpace(item.alternate_url))
                        {
                            sb.AppendLine(item.alternate_url);
                        }
                        if (!string.IsNullOrWhiteSpace(item.apply_alternate_url))
                        {
                            sb.AppendLine(item.apply_alternate_url);
                        }
                        sb.AppendLine(item.archived == true ? "true" : "false");
                        if (item.area != null)
                        {
                            if (!string.IsNullOrWhiteSpace(item.area.id))
                                sb.AppendLine(item.area.id);
                            if (!string.IsNullOrWhiteSpace(item.area.name))
                                sb.AppendLine(item.area.name);
                            if (!string.IsNullOrWhiteSpace(item.area.url))
                                sb.AppendLine(item.area.url);
                        }
                        if (!string.IsNullOrWhiteSpace(item.published_at))
                            sb.AppendLine(item.published_at);
                        if (!string.IsNullOrWhiteSpace(item.created_at))
                            sb.AppendLine(item.created_at);
                        if (item.employer != null)
                        {
                            if (!string.IsNullOrWhiteSpace(item.employer.alternate_url))
                                sb.AppendLine(item.employer.alternate_url);
                            if (!string.IsNullOrWhiteSpace(item.employer.id))
                                sb.AppendLine(item.employer.id);
                            if (item.employer.logo_urls != null)
                            {
                                if (!string.IsNullOrWhiteSpace(item.employer.logo_urls.original))
                                    sb.AppendLine(item.employer.logo_urls.original);
                                if (!string.IsNullOrWhiteSpace(item.employer.logo_urls._240))
                                    sb.AppendLine(item.employer.logo_urls._240);
                                if (!string.IsNullOrWhiteSpace(item.employer.logo_urls._90))
                                    sb.AppendLine(item.employer.logo_urls._90);
                            }
                            if (!string.IsNullOrWhiteSpace(item.employer.name))
                                sb.AppendLine(item.employer.name);
                            if (item.employer.trusted)
                                sb.AppendLine("Trusted employer");
                            else
                                sb.AppendLine("Not trusted employer");
                            if (!string.IsNullOrWhiteSpace(item.employer.url))
                                sb.AppendLine(item.employer.url);
                            if (!string.IsNullOrWhiteSpace(item.employer.vacancies_url))
                                sb.AppendLine(item.employer.vacancies_url);
                            if (item.has_test)
                                sb.AppendLine("Has test task");
                            if (!string.IsNullOrEmpty(item.id))
                                sb.AppendLine(item.id);
                            if (!string.IsNullOrEmpty(item.name))
                                sb.AppendLine(item.name);
                            if (item.premium)
                            {
                                sb.AppendLine("Premium");
                            }
                            else
                            {
                                sb.AppendLine("Not premium");
                            }
                            sb.AppendLine(item.published_at);
                            sb.AppendLine(item.response_letter_required ? "Response letter required" : "Response letter not required");
                            if (item.salary != null)
                            {
                                if (!string.IsNullOrWhiteSpace(item.salary.currency))
                                {
                                    sb.AppendLine(item.salary.currency);
                                }
                                sb.AppendLine(item.salary.from != null ? item.salary.from.ToString() : "");
                                sb.AppendLine(item.salary.from != null ? item.salary.to.ToString() : "");
                                sb.AppendLine(item.salary.gross == true ? "Gross" : "Not gross");
                            }
                            if (item.schedule != null)
                            {
                                if (!string.IsNullOrWhiteSpace(item.schedule.id))
                                    sb.AppendLine(item.schedule.id);
                                if (!string.IsNullOrWhiteSpace(item.schedule.name))
                                    sb.AppendLine(item.schedule.name);
                            }
                            if (item.snippet != null)
                            {
                                if (!string.IsNullOrWhiteSpace(item.snippet.requirement))
                                    sb.AppendLine(item.snippet.requirement.Replace("<highlighttext>", "").Replace("</highlighttext>", ""));
                                if (!string.IsNullOrWhiteSpace(item.snippet.responsibility))
                                    sb.AppendLine(item.snippet.responsibility.Replace("<highlighttext>", "").Replace("</highlighttext>", ""));
                            }
                            if (item.type != null)
                            {
                                if (!string.IsNullOrWhiteSpace(item.type.id))
                                    sb.AppendLine(item.type.id);
                                if (!string.IsNullOrWhiteSpace(item.type.name))
                                    sb.AppendLine(item.type.name);
                            }
                            if (!string.IsNullOrWhiteSpace(item.url))
                                sb.AppendLine(item.url);
                            foreach (WorkingTimeInterval workingTimeInterval in item.working_time_intervals)
                            {
                                if (!string.IsNullOrWhiteSpace(workingTimeInterval.id))
                                    sb.AppendLine(workingTimeInterval.id);
                                if (!string.IsNullOrWhiteSpace(workingTimeInterval.name))
                                    sb.AppendLine(workingTimeInterval.name);
                            }
                        }
                    }
                    sb.AppendLine("\n\n");
                }
            }
            return sb.ToString();
        }

        private string GetFormedStringBuilderEmployeesInfoOutput(Root root, string fileNamePlusExtension)
        {
            sb.Clear();

            string path = Path.Combine(Directory.GetCurrentDirectory(), fileNamePlusExtension);

            string fileContent = string.Empty;

            if (!File.Exists(path))
            {
                File.Create(path);
            }
            string text = File.ReadAllText(path);

            foreach (var item in root.items)
            {
                if (!text.Contains(item.employer.url))
                {

                    if (!string.IsNullOrWhiteSpace(item.employer.alternate_url))
                        sb.AppendLine(item.employer.alternate_url);
                    if (!string.IsNullOrWhiteSpace(item.employer.id))
                        sb.AppendLine(item.employer.id);
                    if (item.employer.logo_urls != null)
                    {
                        if (!string.IsNullOrWhiteSpace(item.employer.logo_urls.original))
                            sb.AppendLine(item.employer.logo_urls.original);
                        if (!string.IsNullOrWhiteSpace(item.employer.logo_urls._240))
                            sb.AppendLine(item.employer.logo_urls._240);
                        if (!string.IsNullOrWhiteSpace(item.employer.logo_urls._90))
                            sb.AppendLine(item.employer.logo_urls._90);
                    }
                    if (!string.IsNullOrWhiteSpace(item.employer.name))
                        sb.AppendLine(item.employer.name);
                    if (item.employer.trusted)
                        sb.AppendLine("Trusted employer");
                    else
                        sb.AppendLine("Not trusted employer");
                    if (!string.IsNullOrWhiteSpace(item.employer.url))
                        sb.AppendLine(item.employer.url);
                    if (!string.IsNullOrWhiteSpace(item.employer.vacancies_url))
                        sb.AppendLine(item.employer.vacancies_url);
                }
                sb.AppendLine("\n\n");
            }
            return sb.ToString();
        }

        //private string GetFormedStringBuilderOutput(Root root, string fileNamePlusExtension)
        //{
        //    sb.Clear();
        //    UniqueVacanciesSelector uniqueVacanciesSelector = new(root.items);
        //    foreach (var item in root.items)
        //    {
        //        if (!uniqueVacanciesSelector.VacancyAlreadyInList(item,fileNamePlusExtension))
        //        {
        //            if (!string.IsNullOrWhiteSpace(item.name))
        //                sb.AppendLine($"{item.name} ");
        //            if (item.accept_temporary)
        //                sb.AppendLine($"accept temporary");
        //            if (item.address != null)
        //            {
        //                if (!string.IsNullOrWhiteSpace(item.address.city))
        //                    sb.AppendLine(item.address.city);
        //                if (!string.IsNullOrWhiteSpace(item.address.street))
        //                    sb.AppendLine(item.address.street);
        //                if (!string.IsNullOrWhiteSpace(item.address.building))
        //                    sb.AppendLine(item.address.building);
        //                if (item.address.metro != null)
        //                {
        //                    if (!string.IsNullOrWhiteSpace(item.address.metro.station_name))
        //                        sb.AppendLine(item.address.metro.station_name);
        //                    if (!string.IsNullOrWhiteSpace(item.address.metro.line_name))
        //                        sb.AppendLine(item.address.metro.station_name);
        //                }
        //                foreach (MetroStation metroStation in item.address.metro_stations)
        //                {
        //                    if (!string.IsNullOrWhiteSpace(metroStation.line_name))
        //                        sb.AppendLine(metroStation.line_name);
        //                    if (!string.IsNullOrWhiteSpace(metroStation.station_name))
        //                        sb.AppendLine(metroStation.station_name);
        //                    if (!string.IsNullOrWhiteSpace(metroStation.line_id))
        //                        sb.AppendLine(metroStation.line_id);
        //                    sb.AppendLine(metroStation.lat.ToString());
        //                    sb.AppendLine(metroStation.lng.ToString());
        //                    if (!string.IsNullOrWhiteSpace(metroStation.station_id))
        //                        sb.AppendLine(metroStation.station_id);
        //                    if (!string.IsNullOrWhiteSpace(metroStation.station_name))
        //                        sb.AppendLine(metroStation.station_name);
        //                }
        //                if (!string.IsNullOrWhiteSpace(item.address.raw))
        //                    sb.AppendLine(item.address.raw);
        //                if (!string.IsNullOrWhiteSpace(item.address.street))
        //                    sb.AppendLine(item.address.street);
        //                if (!string.IsNullOrWhiteSpace(item.alternate_url))
        //                {
        //                    sb.AppendLine(item.alternate_url);
        //                }
        //                if (!string.IsNullOrWhiteSpace(item.apply_alternate_url))
        //                {
        //                    sb.AppendLine(item.apply_alternate_url);
        //                }
        //                sb.AppendLine(item.archived == true ? "true" : "false");
        //                if (item.area != null)
        //                {
        //                    if (!string.IsNullOrWhiteSpace(item.area.id))
        //                        sb.AppendLine(item.area.id);
        //                    if (!string.IsNullOrWhiteSpace(item.area.name))
        //                        sb.AppendLine(item.area.name);
        //                    if (!string.IsNullOrWhiteSpace(item.area.url))
        //                        sb.AppendLine(item.area.url);
        //                }
        //                if (!string.IsNullOrWhiteSpace(item.published_at))
        //                    sb.AppendLine(item.published_at);
        //                if (!string.IsNullOrWhiteSpace(item.created_at))
        //                    sb.AppendLine(item.created_at);
        //                if (item.employer != null)
        //                {
        //                    if (!string.IsNullOrWhiteSpace(item.employer.alternate_url))
        //                        sb.AppendLine(item.employer.alternate_url);
        //                    if (!string.IsNullOrWhiteSpace(item.employer.id))
        //                        sb.AppendLine(item.employer.id);
        //                    if (item.employer.logo_urls != null)
        //                    {
        //                        if (!string.IsNullOrWhiteSpace(item.employer.logo_urls.original))
        //                            sb.AppendLine(item.employer.logo_urls.original);
        //                        if (!string.IsNullOrWhiteSpace(item.employer.logo_urls._240))
        //                            sb.AppendLine(item.employer.logo_urls._240);
        //                        if (!string.IsNullOrWhiteSpace(item.employer.logo_urls._90))
        //                            sb.AppendLine(item.employer.logo_urls._90);
        //                    }
        //                    if (!string.IsNullOrWhiteSpace(item.employer.name))
        //                        sb.AppendLine(item.employer.name);
        //                    if (item.employer.trusted)
        //                        sb.AppendLine("Trusted employer");
        //                    else
        //                        sb.AppendLine("Not trusted employer");
        //                    if (!string.IsNullOrWhiteSpace(item.employer.url))
        //                        sb.AppendLine(item.employer.url);
        //                    if (!string.IsNullOrWhiteSpace(item.employer.vacancies_url))
        //                        sb.AppendLine(item.employer.vacancies_url);
        //                    if (item.has_test)
        //                        sb.AppendLine("Has test task");
        //                    if (!string.IsNullOrEmpty(item.id))
        //                        sb.AppendLine(item.id);
        //                    if (!string.IsNullOrEmpty(item.name))
        //                        sb.AppendLine(item.name);
        //                    if (item.premium)
        //                    {
        //                        sb.AppendLine("Premium");
        //                    }
        //                    else
        //                    {
        //                        sb.AppendLine("Not premium");
        //                    }
        //                    sb.AppendLine(item.published_at);
        //                    sb.AppendLine(item.response_letter_required ? "Response letter required" : "Response letter not required");
        //                    if (item.salary != null)
        //                    {
        //                        if (!string.IsNullOrWhiteSpace(item.salary.currency))
        //                        {
        //                            sb.AppendLine(item.salary.currency);
        //                        }
        //                        sb.AppendLine(item.salary.from != null ? item.salary.from.ToString() : "");
        //                        sb.AppendLine(item.salary.from != null ? item.salary.to.ToString() : "");
        //                        sb.AppendLine(item.salary.gross == true ? "Gross" : "Not gross");
        //                    }
        //                    if (item.schedule != null)
        //                    {
        //                        if (!string.IsNullOrWhiteSpace(item.schedule.id))
        //                            sb.AppendLine(item.schedule.id);
        //                        if (!string.IsNullOrWhiteSpace(item.schedule.name))
        //                            sb.AppendLine(item.schedule.name);
        //                    }
        //                    if (item.snippet != null)
        //                    {
        //                        if (!string.IsNullOrWhiteSpace(item.snippet.requirement))
        //                            sb.AppendLine(item.snippet.requirement.Replace("<highlighttext>", "").Replace("</highlighttext>", ""));
        //                        if (!string.IsNullOrWhiteSpace(item.snippet.responsibility))
        //                            sb.AppendLine(item.snippet.responsibility.Replace("<highlighttext>", "").Replace("</highlighttext>", ""));
        //                    }
        //                    if (item.type != null)
        //                    {
        //                        if (!string.IsNullOrWhiteSpace(item.type.id))
        //                            sb.AppendLine(item.type.id);
        //                        if (!string.IsNullOrWhiteSpace(item.type.name))
        //                            sb.AppendLine(item.type.name);
        //                    }
        //                    if (!string.IsNullOrWhiteSpace(item.url))
        //                        sb.AppendLine(item.url);
        //                    foreach (WorkingTimeInterval workingTimeInterval in item.working_time_intervals)
        //                    {
        //                        if (!string.IsNullOrWhiteSpace(workingTimeInterval.id))
        //                            sb.AppendLine(workingTimeInterval.id);
        //                        if (!string.IsNullOrWhiteSpace(workingTimeInterval.name))
        //                            sb.AppendLine(workingTimeInterval.name);
        //                    }
        //                }
        //            }
        //            sb.AppendLine("\n\n");
        //        }
        //    }
        //    return sb.ToString();
        //}
    }
}
