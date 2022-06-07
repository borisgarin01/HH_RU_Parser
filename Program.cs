using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using HH_RU_Parser.Models;

namespace HH_RU_Parser
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            HttpClient httpClient = new();
            Authenticator authenticator = new();
            Requester requester = new();
            VacansiesPrinter vacansiesPrinter = new();
            //FavoriteVacanciesAdder favoriteVacanciesAdder = new();

            const string allResultsWithoutExperienceFileName = "AllResultsWithNoExperience.docx";
            const string allResultsWithExperienceFromOneToThreeYearsFileName = "AllResultsWithExperienceFromOneToThreeYears.docx";
            const string resultsFromPenzaWithoutExperienceFileName = "ResultsFromPenzaWithoutExperience.docx";
            const string resultsFromPenzaWithExperienceBetweenOneAndThreeYearsFileName = "ResultsFromPenzaWithExperienceBetweenOneAndThreeYears.docx";

            const string allEmployeesHiringProgrammersWithoutExperienceFileName = "AllEmployeesHiringProgrammersWithNoExperience.docx";
            const string allEmployeesHiringProgrammersWithExperienceFromOneToThreeYearsFileName = "AllEmployeesHiringProgrammersWithExperienceFromOneToThreeYears.docx";
            const string employeesFromPenzaHiringProgrammersWithoutExperienceFileName = "EmployeesFromPenzaHiringProgrammersWithoutExperience.docx";
            const string employeesFromPenzaHiringProgrammersWithExperienceBetweenOneAndThreeYearsFileName = "EmployeesFromPenzaHiringProgrammersWithExperienceBetweenOneAndThreeYears.docx";

            try
            {
                await authenticator.AuthenticateAsync(httpClient);
                Console.WriteLine("Authenticated\n\n");

                var allResultsWithNoExperience = await requester.GetResponseAsync("https://api.hh.ru/vacancies?text=C%23&experience=noExperience&search_field=name&search_field=company_name&search_field=description");
                Root rootAllResultsWithNoExperience = await JsonSerializer.DeserializeAsync<Root>(await allResultsWithNoExperience.ReadAsStreamAsync());

                var allResultsWithExperienceBetweenOneAndThreeYears = await requester.GetResponseAsync("https://api.hh.ru/vacancies?text=C%23&experience=between1And3&search_field=name&search_field=company_name&search_field=description");
                Root rootAllResultsWithExperienceFromOneToThreeYears = await JsonSerializer.DeserializeAsync<Root>(await allResultsWithExperienceBetweenOneAndThreeYears.ReadAsStreamAsync());

                #region PrintingAllResultsWithoutExperience

                Console.WriteLine("All results without experience");

                vacansiesPrinter.PrintVacansiesInFile(rootAllResultsWithNoExperience, allResultsWithoutExperienceFileName);
                vacansiesPrinter.PrintResultInConsole(allResultsWithoutExperienceFileName);

                #endregion

                var resultsFromPenzaWithoutExperience = await requester.GetResponseAsync("https://api.hh.ru/vacancies?text=C%23&area=71&experience=noExperience&search_field=name&search_field=company_name&search_field=description");
                Root rootResultsFromPenzaWithoutExperience = await JsonSerializer.DeserializeAsync<Root>(await resultsFromPenzaWithoutExperience.ReadAsStreamAsync());

                var resultsFromPenzaWithExperienceBetweenOneAndThreeYears = await requester.GetResponseAsync("https://api.hh.ru/vacancies?text=C%23&area=71&experience=between1And3&search_field=name&search_field=company_name&search_field=description");
                Root rootResultsFromPenzaWithExperienceBetweenOneAndThreeYears = await JsonSerializer.DeserializeAsync<Root>(await resultsFromPenzaWithExperienceBetweenOneAndThreeYears.ReadAsStreamAsync());

                #region PrintingAllResultsWithNoExperience

                Console.WriteLine("All results without experience");

                vacansiesPrinter.PrintVacansiesInFile(rootAllResultsWithNoExperience, allResultsWithoutExperienceFileName);
                vacansiesPrinter.PrintResultInConsole(allResultsWithoutExperienceFileName);

                #endregion

                #region PrintingAllResultsWithExperienceBeetweenOneToThree

                Console.WriteLine("All results with experience from one to three years");

                vacansiesPrinter.PrintVacansiesInFile(rootAllResultsWithExperienceFromOneToThreeYears, allResultsWithExperienceFromOneToThreeYearsFileName);
                vacansiesPrinter.PrintResultInConsole(allResultsWithExperienceFromOneToThreeYearsFileName);

                #endregion


                #region PrintingResultsFromPenzaWithNoExperience

                Console.WriteLine("Results from Penza without experience");

                vacansiesPrinter.PrintVacansiesInFile(rootResultsFromPenzaWithoutExperience, resultsFromPenzaWithoutExperienceFileName);
                vacansiesPrinter.PrintResultInConsole(resultsFromPenzaWithoutExperienceFileName);

                #endregion

                #region PrintingResultsFromPenzaWithExperienceBeetweenOneToThree

                Console.WriteLine("Results from Penza with experience from one to three years");

                vacansiesPrinter.PrintVacansiesInFile(rootResultsFromPenzaWithExperienceBetweenOneAndThreeYears, resultsFromPenzaWithExperienceBetweenOneAndThreeYearsFileName);
                vacansiesPrinter.PrintResultInConsole(resultsFromPenzaWithExperienceBetweenOneAndThreeYearsFileName);

                vacansiesPrinter.PrintVacansiesInTable(rootAllResultsWithNoExperience);
                vacansiesPrinter.PrintVacansiesInTable(rootAllResultsWithExperienceFromOneToThreeYears);
                vacansiesPrinter.PrintVacansiesInTable(rootResultsFromPenzaWithoutExperience);
                vacansiesPrinter.PrintVacansiesInTable(rootResultsFromPenzaWithExperienceBetweenOneAndThreeYears);

                vacansiesPrinter.PrintEmployeesInFile(rootAllResultsWithNoExperience, allEmployeesHiringProgrammersWithoutExperienceFileName);
                vacansiesPrinter.PrintResultInConsole(allEmployeesHiringProgrammersWithoutExperienceFileName);

                vacansiesPrinter.PrintEmployeesInFile(rootAllResultsWithExperienceFromOneToThreeYears, allEmployeesHiringProgrammersWithExperienceFromOneToThreeYearsFileName);
                vacansiesPrinter.PrintResultInConsole(allEmployeesHiringProgrammersWithExperienceFromOneToThreeYearsFileName);

                vacansiesPrinter.PrintEmployeesInFile(rootResultsFromPenzaWithoutExperience, employeesFromPenzaHiringProgrammersWithoutExperienceFileName);
                vacansiesPrinter.PrintResultInConsole(employeesFromPenzaHiringProgrammersWithoutExperienceFileName);

                vacansiesPrinter.PrintEmployeesInFile(rootResultsFromPenzaWithExperienceBetweenOneAndThreeYears, employeesFromPenzaHiringProgrammersWithExperienceBetweenOneAndThreeYearsFileName);
                vacansiesPrinter.PrintResultInConsole(employeesFromPenzaHiringProgrammersWithExperienceBetweenOneAndThreeYearsFileName);
                #endregion

                vacansiesPrinter.PrintEmployersInTable(rootAllResultsWithNoExperience);
                vacansiesPrinter.PrintEmployersInTable(rootAllResultsWithExperienceFromOneToThreeYears);
                vacansiesPrinter.PrintEmployersInTable(rootResultsFromPenzaWithoutExperience);
                vacansiesPrinter.PrintEmployersInTable(rootResultsFromPenzaWithExperienceBetweenOneAndThreeYears);

                //var allNegotiations = await requester.GetResponseAsync("https://api.hh.ru/negotiations");//403 FORBIDDEN
                //Console.WriteLine(await allNegotiations.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
