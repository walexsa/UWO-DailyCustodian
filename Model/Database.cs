using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Npgsql;
using Supabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using UWO_DailyCustodian.ViewModel;

namespace UWO_DailyCustodian.Model
{
    public class Database : IDatabase
    {
        private const string SupabaseUrl = "https://qhaomokzlbyayepdehvy.supabase.co";
        private const string ApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InFoYW9tb2t6bGJ5YXllcGRlaHZ5Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDgyMDE3MzUsImV4cCI6MjAyMzc3NzczNX0.U8rh_R9musw71qqB9cId7uEosaiyZVcm9jqElnZUSag";

        ObservableCollection<CustodianForm> custodianForms = new();
        ObservableCollection<LeadForm> leadForms = new();

        private Supabase.Client supabase;
        public Database() 
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

                if (response == null)
                {
                    Console.WriteLine($"Insert failed, {response}");
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

                if (response == null)
                {
                    Console.WriteLine($"Insert failed, {response}");
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

                if (response == null)
                {
                    Console.WriteLine($"Insert failed, {response}");
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
    }
}
