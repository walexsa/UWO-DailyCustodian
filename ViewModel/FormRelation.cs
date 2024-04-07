using Postgrest.Attributes;
using Postgrest.Models;

namespace UWO_DailyCustodian.ViewModel
{
    [Table("form_relation")]
    public class FormRelation : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }
        [Column("lead_id")]
        public int LeadId { get; set; }
        [Column("custodian_id")]
        public int CustodianId { get; set; }
        public FormRelation() { }
        public FormRelation(int leadId, int custodianId)
        {
            LeadId = leadId;
            CustodianId = custodianId;
        }
    }
}
