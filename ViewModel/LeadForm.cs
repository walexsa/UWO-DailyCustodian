using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace UWO_DailyCustodian.ViewModel;

[Table("lead_forms")]
public class LeadForm : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }
    [Column("first_name")]
    public string FirstName { get; set; }
    [Column("last_name")]
    public string LastName { get; set; }
    [Column("lead_name")]
    public string LeadCustodianName { get { return FirstName + " " + LastName; } }
    [Column("building")]
    public string Building { get; set; }
    [Column("date")]
    public DateTime Date { get; set; }
    [Column("class_boards")]
    public int ClassBoards { get; set; }
    [Column("class_garbage")]
    public int ClassGarbage { get; set; }
    [Column("class_floors")]
    public int ClassFloors { get; set; }
    [Column("class_dusting")]
    public int ClassDusting { get; set; }
    [Column("class_windows")]
    public int ClassWindows { get; set; }
    [Column("class_walls")]
    public int ClassWalls { get; set; }
    [Column("hall_floors")]
    public int HallFloors { get; set; }
    [Column("hall_garbage")]
    public int HallGarbage { get; set; }
    [Column("hall_dusting")]
    public int HallDusting { get; set; }
    [Column("hall_walls")]
    public int HallWalls { get; set; }
    [Column("bath_sinks")]
    public int BathSinks { get; set; }
    [Column("bath_toilets")]
    public int BathToilets { get; set; }
    [Column("bath_dusting")]
    public int BathDusting { get; set; }
    [Column("bath_mirrors")]
    public int BathMirrors { get; set; }
    [Column("bath_ledges")]
    public int BathLedges { get; set; }
    [Column("bath_dryers")]
    public int BathDryers { get; set; }
    [Column("bath_vents")]
    public int BathVents { get; set; }
    [Column("bath_floors")]
    public int BathFloors { get; set; }
    [Column("bath_walls")]
    public int BathWalls { get; set; }
    [Column("bath_curtains")]
    public int BathCurtains { get; set; }
    [Column("bath_shower")]
    public int BathShower { get; set; }
    [Column("bath_supply")]
    public int BathSupply { get; set; }
    [Column("office_vacuum")]
    public int OfficeVacuum { get; set; }
    [Column("stair_floors")]
    public int StairFloors { get; set; }
    [Column("stair_railings")]
    public int StairRailings { get; set; }
    [Column("stair_walls")]
    public int StairWalls { get; set; }
    [Column("entr_glass")]
    public int EntrGlass { get; set; }
    [Column("entr_floors")]
    public int EntrFloors { get; set; }
    [Column("entr_rugs")]
    public int EntrRugs { get; set; }
    [Column("entr_dusting")]
    public int EntrDusting { get; set; }
    [Column("remarks")]
    public string Remarks { get; set; }

    public LeadForm() { }
    public LeadForm(string firstName, string lastName, string building, int classBoards, int classGarbage, int classFloors, int classDusting, int classWindows, int classWalls, int hallFloors, int hallGarbage, int hallDusting, int hallWalls, int bathSinks, int bathToilets, int bathDusting, int bathMirrors, int bathLedges, int bathDryers, int bathVents, int bathFloors, int bathWalls, int bathCurtains, int bathShower, int bathSupply, int officeVacuum, int stairFloors, int stairRailings, int stairWalls, int entrGlass, int entrFloors, int entrRugs, int entrDusting, string remarks)
    {
        FirstName = firstName;
        LastName = lastName;
        Building = building;
        ClassBoards = classBoards;
        ClassGarbage = classGarbage;
        ClassFloors = classFloors;
        ClassDusting = classDusting;
        ClassWindows = classWindows;
        ClassWalls = classWalls;
        HallFloors = hallFloors;
        HallGarbage = hallGarbage;
        HallDusting = hallDusting;
        HallWalls = hallWalls;
        BathSinks = bathSinks;
        BathToilets = bathToilets;
        BathDusting = bathDusting;
        BathMirrors = bathMirrors;
        BathLedges = bathLedges;
        BathDryers = bathDryers;
        BathVents = bathVents;
        BathFloors = bathFloors;
        BathWalls = bathWalls;
        BathCurtains = bathCurtains;
        BathShower = bathShower;
        BathSupply = bathSupply;
        OfficeVacuum = officeVacuum;
        StairFloors = stairFloors;
        StairRailings = stairRailings;
        StairWalls = stairWalls;
        EntrGlass = entrGlass;
        EntrFloors = entrFloors;
        EntrRugs = entrRugs;
        EntrDusting = entrDusting;
        Remarks = remarks;
    }

    public void AddDate(LeadForm form, DateTime date)
    {
        form.Date = date;
    }
    public void AddId(LeadForm form, int id)
    {
        form.Id = id;
    }
}