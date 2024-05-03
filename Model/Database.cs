using Newtonsoft.Json.Linq;
using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
using Supabase.Interfaces;
using System.Collections.ObjectModel;
using UWO_DailyCustodian.ViewModel;

namespace UWO_DailyCustodian.Model
{
    public class Database : IDatabase
    {
        private static Database _instance;
        private string SupabaseUrl = "https://qhaomokzlbyayepdehvy.supabase.co"; //Environment.GetEnvironmentVariable("SUPABASE_URL");
        private string ApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InFoYW9tb2t6bGJ5YXllcGRlaHZ5Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDgyMDE3MzUsImV4cCI6MjAyMzc3NzczNX0.U8rh_R9musw71qqB9cId7uEosaiyZVcm9jqElnZUSag"; //Environment.GetEnvironmentVariable("API_KEY");

        ObservableCollection<CustodianForm> custodianForms = new();
        ObservableCollection<LeadForm> leadForms = new();

        private Supabase.Client supabase;
        private Session session;
        private Database() 
        {
            Initialize();
        }

        async void Initialize()
        {
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };
            supabase = new Supabase.Client(SupabaseUrl, ApiKey, options);
            await supabase.InitializeAsync();
        }

        public static IDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(Database))
                    {
                        _instance = new Database();
                    }
                }
                return _instance;
            }
        }

        public async Task<ObservableCollection<CustodianForm>> SelectAllCustodianForms()
        {
            custodianForms.Clear();
            var result = await supabase.From<CustodianForm>().Get();
            custodianForms = new ObservableCollection<CustodianForm>(result.Models);
            return custodianForms;
        }
        public async Task<ObservableCollection<LeadForm>> SelectAllLeadForms()
        {
            leadForms.Clear();
            var result = await supabase.From<LeadForm>().Get();
            leadForms = new ObservableCollection<LeadForm>(result.Models);
            return leadForms;
        }

        public async Task<string> GetRole(string email)
        {
            var result = await supabase.From<UserEmail>().Get();
            if (result == null)
            {
                return null;
            }
            List<UserEmail> users = new List<UserEmail>(result.Models);
            UserEmail userEmail = users.Find(x => x.Email.Equals(email));
            return userEmail.Role;
        }
        public async Task<string> SignUp(string email, string password, string role)
        {
            try
            {
                var session = await supabase.Auth.SignUp(email, password);
                if (session == null)
                {

                }
                return "testing";
            } 
            catch (GotrueException e)
            {
                JObject errorJson = JObject.Parse(e.Message);
                string errorMessage = errorJson["msg"].ToString();
                return errorMessage;
            }
        }
        public async Task<bool> SignIn(string email, string password)
        {
            session = await supabase.Auth.SignIn(email, password);
            if (session == null)
            {
                return false;
            }

            await supabase.Auth.SetSession(session.AccessToken, session.RefreshToken);

            var userProfile = supabase.Auth.CurrentUser;
            var userRole = userProfile.Role;
            // TODO - add roles to users?
            //if (userRole.Equals("authenticated")) {
            //    string newRole = await GetRole(email);

            //    if (userProfile == null)
            //    {
            //        Console.WriteLine("Failed to retrieve user profile.");
            //        return false;
            //    }

            //    var attr = new UserAttributes
            //    {
            //        Data = new Dictionary<string, object> { { "job", newRole } }
            //    };

            //    // adds a value to data dictionary, not role column.. 
            //    // TODO
            //    var updateResponse = await supabase.Auth.Update(attr);

            //    if (updateResponse == null)
            //    {
            //        Console.WriteLine("Failed to update user profile.");
            //        return false;
            //    }
            //}

            return true;
        }
        public async Task<bool> InsertCustodianFormAsync(CustodianForm form)
        {
            try
            {
                if (supabase == null)
                {
                    Console.WriteLine("supabaseClient is null");
                    return false;
                }

                form.AddDate(form, DateTime.Now);

                var response = await supabase
                    .From<CustodianForm>()
                    .Insert(form);

                int statusCode = (int)response.ResponseMessage.StatusCode;
                if (statusCode >= 400 && statusCode <= 599)
                {
                    Console.WriteLine($"Insert failed, {response.ResponseMessage.Content}");
                    return false;
                }

                await SelectAllCustodianForms();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Insert failed, {e}");
                return false;
            }
        }
        public async Task<int> InsertLeadFormAsync(LeadForm form)
        {
            try
            {
                if (supabase == null)
                {
                    Console.WriteLine("supabaseClient is null");
                    return -1;
                }

                form.AddDate(form, DateTime.Now);

                var response = await supabase
                    .From<LeadForm>()
                    .Insert(form, new Postgrest.QueryOptions { Returning = Postgrest.QueryOptions.ReturnType.Representation });

                int statusCode = (int)response.ResponseMessage.StatusCode;
                if (statusCode >= 400 && statusCode <= 599)
                {
                    Console.WriteLine($"Insert failed, {response.ResponseMessage.Content}");
                    return -1;
                }

                var id = response.Model.Id;

                await SelectAllLeadForms();

                return id;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Insert failed, {e}");
                return -1;
            }
        }
        public async Task<bool> InsertFormRelation(int leadFormId, int custodianFormId)
        {
            try
            {
                if (supabase == null)
                {
                    Console.WriteLine("supabaseClient is null");
                    return false;
                }

                var response = await supabase
                    .From<FormRelation>()
                    .Insert(new FormRelation(leadFormId, custodianFormId));

                int statusCode = (int)response.ResponseMessage.StatusCode;
                if (statusCode >= 400 && statusCode <= 599)
                {
                    Console.WriteLine($"Insert failed, {response.ResponseMessage.Content}");
                    return false;
                }

                return true;
            } 
            catch (Exception e)
            {
                Console.WriteLine($"Insert failed, {e}");
                return false;
            }
        }

        public async Task<bool> InsertPhoto(byte[] fileData, string filePath)
        {
            if (fileData != null)
            {
                var bucketName = "photos";
                await supabase.Storage.From(bucketName).Upload(fileData, filePath);
            }
            return true;
        }

        public async Task<byte[]> DownloadImageFromSupabaseAsync(string filePath)
        {
            var response = await supabase.Storage.From("photos").Download(filePath, null);

            return response;
        }

        public async Task<bool> AddEmployee(string email, string role)
        {
            try
            {
                if (supabase == null)
                {
                    Console.WriteLine("supabaseClient is null");
                    return false;
                }

                var response = await supabase.From<UserEmail>().Insert(new UserEmail(email, role));

                int statusCode = (int)response.ResponseMessage.StatusCode;
                if (statusCode >= 400 && statusCode <= 599)
                {
                    Console.WriteLine($"Insert failed, {response.ResponseMessage.Content}");
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Insert failed, {e}");
                return false;
            }
        }

        public async Task DeleteLeadForms(List<int> formIds)
        {
            foreach (var formId in formIds)
            {
                await supabase.From<FormRelation>().Where(x => x.LeadId == formId).Delete();

                await supabase.From<LeadForm>().Where(x => x.Id == formId).Delete();

                string imageFilePath = $"{formId}.png";
                await supabase.Storage.From("photos").Remove(imageFilePath);
            }
        }
    }
}
