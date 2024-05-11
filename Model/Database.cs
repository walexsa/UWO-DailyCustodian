using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using Npgsql.Internal;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
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
        private string SupabaseUrl = "https://qhaomokzlbyayepdehvy.supabase.co";
        private string ApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InFoYW9tb2t6bGJ5YXllcGRlaHZ5Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDgyMDE3MzUsImV4cCI6MjAyMzc3NzczNX0.U8rh_R9musw71qqB9cId7uEosaiyZVcm9jqElnZUSag";

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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
            var result = await supabase.From<UserEmail>().Where(x => x.Email == email).Get();
            if (result == null)
            {
                return null;
            }
            UserEmail userEmail = result.Model;
            if (userEmail == null)
            {
                return null;
            }
            return userEmail.Role;
        }
        public async Task<string> SignUp(string email, string password)
        {
            try
            {
                var session = await supabase.Auth.SignUp(email, password);
                if (session == null)
                {
                    Console.WriteLine("Sign up failed");
                    return "session is null";
                }
                return "success";
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

        public async Task<bool> AddEditEmployee(string email, string role)
        {
            try
            {
                if (supabase == null)
                {
                    Console.WriteLine("supabaseClient is null");
                    return false;
                }

                // check if email exists in the table already and edit the row
                var result = await supabase.From<UserEmail>().Where(x => x.Email == email).Get();
                if (result.Model != null)
                {
                    var response = await supabase.From<UserEmail>().Where(x => x.Email == email).Set(x => x.Role, role).Update();

                    int statusCode = (int)response.ResponseMessage.StatusCode;
                    if (statusCode >= 400 && statusCode <= 599)
                    {
                        Console.WriteLine($"Update failed, {response.ResponseMessage.Content}");
                        return false;
                    }
                }
                else
                {
                    var response = await supabase.From<UserEmail>().Insert(new UserEmail(email, role));

                    int statusCode = (int)response.ResponseMessage.StatusCode;
                    if (statusCode >= 400 && statusCode <= 599)
                    {
                        Console.WriteLine($"Insert failed, {response.ResponseMessage.Content}");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Insert failed, {e}");
                return false;
            }
        }

        public async Task<bool> RemoveEmployee(string email)
        {
            try
            {
                if (supabase == null)
                {
                    Console.WriteLine("supabaseClient is null");
                    return false;
                }
                var account = await supabase.From<UserEmail>().Where(x => x.Email == email).Get();
                if (account.Model == null)
                {
                    Console.WriteLine("Email does not exist in table user_emails to delete.");
                    return false;
                }
                await supabase.From<UserEmail>().Where(x => x.Email == email).Delete();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Insert failed, {e}");
                return false;
            }
        }

        public async Task DeleteLeadForms(List<LeadForm> forms)
        {
            foreach (var form in forms)
            {
                int formId = form.Id;
                await supabase.From<FormRelation>().Where(x => x.LeadId == formId).Delete();

                await supabase.From<LeadForm>().Where(x => x.Id == formId).Delete();

                string imageFilePath = $"{formId}.png";
                await supabase.Storage.From("photos").Remove(imageFilePath);

                string fileFilePath = form.LeadCustodianName + formId + ".xlsx";
                await supabase.Storage.From("files").Remove(fileFilePath);
            }
        }

        public async Task<string> CreateAndUploadExcelDocument(LeadForm leadForm, int leadFormId, List<CustodianForm> custodianForms, string imagePath)
        {
            using (var excelPackage = new ExcelPackage())
            {
                // Add a worksheet and populate it with data
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(leadForm.Id.ToString());

                // Define column headers
                worksheet.Cells["A1"].Value = "Attribute";
                worksheet.Cells["B1"].Value = "Lead Form";
                worksheet.Cells["D1"].Value = "Custodian Forms";
                worksheet.Cells["A1:D1"].Style.Font.UnderLine = true;

                // Define the attributes and form responses
                worksheet.Cells["A2"].Value = "First Name";
                worksheet.Cells["A3"].Value = "Last Name";

                worksheet.Cells["A5"].Value = "Building";
                worksheet.Cells["A6"].Value = "Date";
                worksheet.Cells["A7"].Value = "Class Boards";
                worksheet.Cells["A8"].Value = "Class Garbage";
                worksheet.Cells["A9"].Value = "Class Floors";
                worksheet.Cells["A10"].Value = "Class Dusting";
                worksheet.Cells["A11"].Value = "Class Windows";
                worksheet.Cells["A12"].Value = "Class Walls";
                worksheet.Cells["A13"].Value = "Hall Floors";
                worksheet.Cells["A14"].Value = "Hall Garbage";
                worksheet.Cells["A15"].Value = "Hall Dusting";
                worksheet.Cells["A16"].Value = "Hall Walls";
                worksheet.Cells["A17"].Value = "Bath Sinks";
                worksheet.Cells["A18"].Value = "Bath Toilets";
                worksheet.Cells["A19"].Value = "Bath Dusting";
                worksheet.Cells["A20"].Value = "Bath Mirrors";
                worksheet.Cells["A21"].Value = "Bath Ledges";
                worksheet.Cells["A22"].Value = "Bath Dryers";
                worksheet.Cells["A23"].Value = "Bath Vents";
                worksheet.Cells["A24"].Value = "Bath Floors";
                worksheet.Cells["A25"].Value = "Bath Walls";
                worksheet.Cells["A26"].Value = "Bath Curtains";
                worksheet.Cells["A27"].Value = "Bath Shower";
                worksheet.Cells["A28"].Value = "Bath Supply";
                worksheet.Cells["A29"].Value = "Office Vacuum";
                worksheet.Cells["A30"].Value = "Stair Floors";
                worksheet.Cells["A31"].Value = "Stair Railings";
                worksheet.Cells["A32"].Value = "Stair Walls";
                worksheet.Cells["A33"].Value = "Entry Glass";
                worksheet.Cells["A34"].Value = "Entry Floors";
                worksheet.Cells["A35"].Value = "Entry Rugs";
                worksheet.Cells["A36"].Value = "Entry Dusting";
                worksheet.Cells["A37"].Value = "Remarks";
                worksheet.Cells["B2"].Value = leadForm.FirstName;
                worksheet.Cells["B3"].Value = leadForm.LastName;

                worksheet.Cells["B5"].Value = leadForm.Building;
                worksheet.Cells["B6"].Value = leadForm.Date;
                worksheet.Cells["B6"].Style.Numberformat.Format = "MM/dd/yyyy";
                worksheet.Cells["B7"].Value = leadForm.ClassBoards;
                worksheet.Cells["B8"].Value = leadForm.ClassGarbage;
                worksheet.Cells["B9"].Value = leadForm.ClassFloors;
                worksheet.Cells["B10"].Value = leadForm.ClassDusting;
                worksheet.Cells["B11"].Value = leadForm.ClassWindows;
                worksheet.Cells["B12"].Value = leadForm.ClassWalls;
                worksheet.Cells["B13"].Value = leadForm.HallFloors;
                worksheet.Cells["B14"].Value = leadForm.HallGarbage;
                worksheet.Cells["B15"].Value = leadForm.HallDusting;
                worksheet.Cells["B16"].Value = leadForm.HallWalls;
                worksheet.Cells["B17"].Value = leadForm.BathSinks;
                worksheet.Cells["B18"].Value = leadForm.BathToilets;
                worksheet.Cells["B19"].Value = leadForm.BathDusting;
                worksheet.Cells["B20"].Value = leadForm.BathMirrors;
                worksheet.Cells["B21"].Value = leadForm.BathLedges;
                worksheet.Cells["B22"].Value = leadForm.BathDryers;
                worksheet.Cells["B23"].Value = leadForm.BathVents;
                worksheet.Cells["B24"].Value = leadForm.BathFloors;
                worksheet.Cells["B25"].Value = leadForm.BathWalls;
                worksheet.Cells["B26"].Value = leadForm.BathCurtains;
                worksheet.Cells["B27"].Value = leadForm.BathShower;
                worksheet.Cells["B28"].Value = leadForm.BathSupply;
                worksheet.Cells["B29"].Value = leadForm.OfficeVacuum;
                worksheet.Cells["B30"].Value = leadForm.StairFloors;
                worksheet.Cells["B31"].Value = leadForm.StairRailings;
                worksheet.Cells["B32"].Value = leadForm.StairWalls;
                worksheet.Cells["B33"].Value = leadForm.EntrGlass;
                worksheet.Cells["B34"].Value = leadForm.EntrFloors;
                worksheet.Cells["B35"].Value = leadForm.EntrRugs;
                worksheet.Cells["B36"].Value = leadForm.EntrDusting;
                worksheet.Cells["B37"].Value = leadForm.Remarks;

                if (imagePath != null)
                {
                    byte[] photoBytes = await DownloadImageFromSupabaseAsync(imagePath);
                    if (photoBytes != null)
                    {
                        ExcelPicture picture = worksheet.Drawings.AddPicture("Photo", new MemoryStream(photoBytes));

                        picture.From.Column = 1; // Column index 2 corresponds to cell "C"
                        picture.From.Row = 41;    // Row index 1 corresponds to cell 2
                        picture.SetSize(300, 300); // Optional: set the size of the photo in pixels
                    }
                }

                if (custodianForms != null)
                {
                    int columnIndex = 4;

                    foreach (var custodianForm in custodianForms)
                    {
                        worksheet.Cells[2, columnIndex].Value = custodianForm.FirstName;
                        worksheet.Cells[3, columnIndex].Value = custodianForm.LastName;

                        worksheet.Cells[5, columnIndex].Value = custodianForm.Building;
                        worksheet.Cells[6, columnIndex].Value = custodianForm.Date;
                        worksheet.Cells[6, columnIndex].Style.Numberformat.Format = "MM/dd/yyyy";
                        worksheet.Cells[7, columnIndex].Value = custodianForm.ClassBoards;
                        worksheet.Cells[8, columnIndex].Value = custodianForm.ClassGarbage;
                        worksheet.Cells[9, columnIndex].Value = custodianForm.ClassFloors;
                        worksheet.Cells[10, columnIndex].Value = custodianForm.ClassDusting;
                        worksheet.Cells[11, columnIndex].Value = custodianForm.ClassWindows;
                        worksheet.Cells[12, columnIndex].Value = custodianForm.ClassWalls;
                        worksheet.Cells[13, columnIndex].Value = custodianForm.HallFloors;
                        worksheet.Cells[14, columnIndex].Value = custodianForm.HallGarbage;
                        worksheet.Cells[15, columnIndex].Value = custodianForm.HallDusting;
                        worksheet.Cells[16, columnIndex].Value = custodianForm.HallWalls;
                        worksheet.Cells[17, columnIndex].Value = custodianForm.BathSinks;
                        worksheet.Cells[18, columnIndex].Value = custodianForm.BathToilets;
                        worksheet.Cells[19, columnIndex].Value = custodianForm.BathDusting;
                        worksheet.Cells[20, columnIndex].Value = custodianForm.BathMirrors;
                        worksheet.Cells[21, columnIndex].Value = custodianForm.BathLedges;
                        worksheet.Cells[22, columnIndex].Value = custodianForm.BathDryers;
                        worksheet.Cells[23, columnIndex].Value = custodianForm.BathVents;
                        worksheet.Cells[24, columnIndex].Value = custodianForm.BathFloors;
                        worksheet.Cells[25, columnIndex].Value = custodianForm.BathWalls;
                        worksheet.Cells[26, columnIndex].Value = custodianForm.BathCurtains;
                        worksheet.Cells[27, columnIndex].Value = custodianForm.BathShower;
                        worksheet.Cells[28, columnIndex].Value = custodianForm.BathSupply;
                        worksheet.Cells[29, columnIndex].Value = custodianForm.OfficeVacuum;
                        worksheet.Cells[30, columnIndex].Value = custodianForm.StairFloors;
                        worksheet.Cells[31, columnIndex].Value = custodianForm.StairRailings;
                        worksheet.Cells[32, columnIndex].Value = custodianForm.StairWalls;
                        worksheet.Cells[33, columnIndex].Value = custodianForm.EntrGlass;
                        worksheet.Cells[34, columnIndex].Value = custodianForm.EntrFloors;
                        worksheet.Cells[35, columnIndex].Value = custodianForm.EntrRugs;
                        worksheet.Cells[36, columnIndex].Value = custodianForm.EntrDusting;

                        columnIndex++;
                    }
                }

                int descriptionRow = 39;
                worksheet.Cells[$"A{descriptionRow}"].Value = "Lead Response Scale:";
                worksheet.Cells[$"B{descriptionRow}"].Value = "0 = N/A";
                worksheet.Cells[$"C{descriptionRow}"].Value = "1 = Dissatisfactory";
                worksheet.Cells[$"D{descriptionRow}"].Value = "2 = Satisfactory";
                worksheet.Cells[$"E{descriptionRow}"].Value = "3 = Good";

                descriptionRow = 40;
                worksheet.Cells[$"A{descriptionRow}"].Value = "Custodian Response Scale:";
                worksheet.Cells[$"B{descriptionRow}"].Value = "TRUE = Cleaned";
                worksheet.Cells[$"C{descriptionRow}"].Value = "FALSE = Not Cleaned";

                worksheet.Cells.AutoFitColumns();

                // Convert the Excel document to a byte array
                byte[] excelFileBytes = excelPackage.GetAsByteArray();

                string fileName = leadForm.LeadCustodianName + leadFormId + ".xlsx";
                var uploadResponse = await supabase.Storage.From("files").Upload(excelFileBytes, fileName);
                return uploadResponse;
            }
        }

        public List<string> DownloadForms(List<LeadForm> forms)
        {
            List<string> urls = new List<string>();
            foreach (LeadForm form in forms)
            {
                string filePath = form.LeadCustodianName + form.Id + ".xlsx";
                var fileUrl = supabase.Storage.From("files").GetPublicUrl(filePath);
                urls.Add(fileUrl);
            }
            return urls;
        }
    }
}
