using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWO_DailyCustodian.ViewModel
{
    [Table("custodian_forms")]
    public class CustodianForm : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("custodian_name")]
        public string CustodianName { get { return FirstName + " " + LastName; } }
        [Column("building")]
        public string Building { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("class_boards")]
        public bool ClassBoards { get; set; }
        [Column("class_garbage")]
        public bool ClassGarbage { get; set; }
        [Column("class_floors")]
        public bool ClassFloors { get; set; }
        [Column("class_dusting")]
        public bool ClassDusting { get; set; }
        [Column("class_windows")]
        public bool ClassWindows { get; set; }
        [Column("class_walls")]
        public bool ClassWalls { get; set; }
        [Column("hall_floors")]
        public bool HallFloors { get; set; }
        [Column("hall_garbage")]
        public bool HallGarbage { get; set; }
        [Column("hall_dusting")]
        public bool HallDusting { get; set; }
        [Column("hall_walls")]
        public bool HallWalls { get; set; }
        [Column("bath_sinks")]
        public bool BathSinks { get; set; }
        [Column("bath_toilets")]
        public bool BathToilets { get; set; }
        [Column("bath_dusting")]
        public bool BathDusting { get; set; }
        [Column("bath_mirrors")]
        public bool BathMirrors { get; set; }
        [Column("bath_ledges")]
        public bool BathLedges { get; set; }
        [Column("bath_dryers")]
        public bool BathDryers { get; set; }
        [Column("bath_vents")]
        public bool BathVents { get; set; }
        [Column("bath_floors")]
        public bool BathFloors { get; set; }
        [Column("bath_walls")]
        public bool BathWalls { get; set; }
        [Column("bath_curtains")]
        public bool BathCurtains { get; set; }
        [Column("bath_shower")]
        public bool BathShower { get; set; }
        [Column("bath_supply")]
        public bool BathSupply { get; set; }
        [Column("office_vacuum")]
        public bool OfficeVacuum { get; set; }
        [Column("stair_floors")]
        public bool StairFloors { get; set; }
        [Column("stair_railings")]
        public bool StairRailings { get; set; }
        [Column("stair_walls")]
        public bool StairWalls { get; set; }
        [Column("entr_glass")]
        public bool EntrGlass { get; set; }
        [Column("entr_floors")]
        public bool EntrFloors { get; set; }
        [Column("entr_rugs")]
        public bool EntrRugs { get; set; }
        [Column("entr_dusting")]
        public bool EntrDusting { get; set; }

        public CustodianForm() { }
        public CustodianForm(string firstName, string lastName, string building, bool classBoards, bool classGarbage, bool classFloors, bool classDusting, bool classWindows, bool classWalls, bool hallFloors, bool hallGarbage, bool hallDusting, bool hallWalls, bool bathSinks, bool bathToilets, bool bathDusting, bool bathMirrors, bool bathLedges, bool bathDryers, bool bathVents, bool bathFloors, bool bathWalls, bool bathCurtains, bool bathShower, bool bathSupply, bool officeVacuum, bool stairFloors, bool stairRailings, bool stairWalls, bool entrGlass, bool entrFloors, bool entrRugs, bool entrDusting)
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
        }

        public void AddDate(CustodianForm form, DateTime date)
        {
            form.Date = date;
        }
        public void AddId(CustodianForm form, int id)
        {
            form.Id = id;
        }
    }
}
