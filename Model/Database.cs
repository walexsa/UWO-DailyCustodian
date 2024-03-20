using Microsoft.Extensions.Options;
using Npgsql;
using Supabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
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

        public async Task<bool> InsertCustodianFormAsync(CustodianForm form)
        {
            try
            {
                if (supabase == null)
                {
                    Console.WriteLine("supabaseClient is null");
                    return false;
                }

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
    }
}
