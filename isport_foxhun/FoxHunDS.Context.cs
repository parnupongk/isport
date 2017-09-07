﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;

public partial class Entities : DbContext
{
    public Entities()
        : base("name=Entities")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }

    public virtual DbSet<foxhun_match> foxhun_match { get; set; }
    public virtual DbSet<foxhun_player> foxhun_player { get; set; }
    public virtual DbSet<foxhun_player_history> foxhun_player_history { get; set; }
    public virtual DbSet<foxhun_province> foxhun_province { get; set; }
    public virtual DbSet<foxhun_region> foxhun_region { get; set; }
    public virtual DbSet<foxhun_scout> foxhun_scout { get; set; }
    public virtual DbSet<foxhun_team> foxhun_team { get; set; }
    public virtual DbSet<foxhun_users> foxhun_users { get; set; }

    public virtual int usp_foxhun_playerinsert(string id, string region, string team, Nullable<System.DateTime> datetime, Nullable<int> seq, string position, string name, string control, string attack, string tacktick, string defense, string physical, string mental, string adversary, string contact, string hT, string fT, Nullable<int> competition, Nullable<int> score, Nullable<int> see, string detail, Nullable<int> wight, Nullable<int> hight, Nullable<int> age, string country, string image, Nullable<int> sum, string team_id, Nullable<int> number, string birthday)
    {
        var idParameter = id != null ?
            new ObjectParameter("id", id) :
            new ObjectParameter("id", typeof(string));

        var regionParameter = region != null ?
            new ObjectParameter("region", region) :
            new ObjectParameter("region", typeof(string));

        var teamParameter = team != null ?
            new ObjectParameter("team", team) :
            new ObjectParameter("team", typeof(string));

        var datetimeParameter = datetime.HasValue ?
            new ObjectParameter("datetime", datetime) :
            new ObjectParameter("datetime", typeof(System.DateTime));

        var seqParameter = seq.HasValue ?
            new ObjectParameter("seq", seq) :
            new ObjectParameter("seq", typeof(int));

        var positionParameter = position != null ?
            new ObjectParameter("position", position) :
            new ObjectParameter("position", typeof(string));

        var nameParameter = name != null ?
            new ObjectParameter("name", name) :
            new ObjectParameter("name", typeof(string));

        var controlParameter = control != null ?
            new ObjectParameter("control", control) :
            new ObjectParameter("control", typeof(string));

        var attackParameter = attack != null ?
            new ObjectParameter("attack", attack) :
            new ObjectParameter("attack", typeof(string));

        var tacktickParameter = tacktick != null ?
            new ObjectParameter("tacktick", tacktick) :
            new ObjectParameter("tacktick", typeof(string));

        var defenseParameter = defense != null ?
            new ObjectParameter("defense", defense) :
            new ObjectParameter("defense", typeof(string));

        var physicalParameter = physical != null ?
            new ObjectParameter("physical", physical) :
            new ObjectParameter("physical", typeof(string));

        var mentalParameter = mental != null ?
            new ObjectParameter("mental", mental) :
            new ObjectParameter("mental", typeof(string));

        var adversaryParameter = adversary != null ?
            new ObjectParameter("adversary", adversary) :
            new ObjectParameter("adversary", typeof(string));

        var contactParameter = contact != null ?
            new ObjectParameter("contact", contact) :
            new ObjectParameter("contact", typeof(string));

        var hTParameter = hT != null ?
            new ObjectParameter("HT", hT) :
            new ObjectParameter("HT", typeof(string));

        var fTParameter = fT != null ?
            new ObjectParameter("FT", fT) :
            new ObjectParameter("FT", typeof(string));

        var competitionParameter = competition.HasValue ?
            new ObjectParameter("competition", competition) :
            new ObjectParameter("competition", typeof(int));

        var scoreParameter = score.HasValue ?
            new ObjectParameter("score", score) :
            new ObjectParameter("score", typeof(int));

        var seeParameter = see.HasValue ?
            new ObjectParameter("see", see) :
            new ObjectParameter("see", typeof(int));

        var detailParameter = detail != null ?
            new ObjectParameter("detail", detail) :
            new ObjectParameter("detail", typeof(string));

        var wightParameter = wight.HasValue ?
            new ObjectParameter("wight", wight) :
            new ObjectParameter("wight", typeof(int));

        var hightParameter = hight.HasValue ?
            new ObjectParameter("hight", hight) :
            new ObjectParameter("hight", typeof(int));

        var ageParameter = age.HasValue ?
            new ObjectParameter("age", age) :
            new ObjectParameter("age", typeof(int));

        var countryParameter = country != null ?
            new ObjectParameter("country", country) :
            new ObjectParameter("country", typeof(string));

        var imageParameter = image != null ?
            new ObjectParameter("image", image) :
            new ObjectParameter("image", typeof(string));

        var sumParameter = sum.HasValue ?
            new ObjectParameter("sum", sum) :
            new ObjectParameter("sum", typeof(int));

        var team_idParameter = team_id != null ?
            new ObjectParameter("team_id", team_id) :
            new ObjectParameter("team_id", typeof(string));

        var numberParameter = number.HasValue ?
            new ObjectParameter("number", number) :
            new ObjectParameter("number", typeof(int));

        var birthdayParameter = birthday != null ?
            new ObjectParameter("birthday", birthday) :
            new ObjectParameter("birthday", typeof(string));

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_foxhun_playerinsert", idParameter, regionParameter, teamParameter, datetimeParameter, seqParameter, positionParameter, nameParameter, controlParameter, attackParameter, tacktickParameter, defenseParameter, physicalParameter, mentalParameter, adversaryParameter, contactParameter, hTParameter, fTParameter, competitionParameter, scoreParameter, seeParameter, detailParameter, wightParameter, hightParameter, ageParameter, countryParameter, imageParameter, sumParameter, team_idParameter, numberParameter, birthdayParameter);
    }

    public virtual int usp_foxhun_playerupdate(string id, string region, string team, Nullable<System.DateTime> datetime, Nullable<int> seq, string position, string name, string control, string attack, string defense, string tacktick, string physical, string mental, string adversary, string contact, string hT, string fT, Nullable<int> competition, Nullable<int> score, Nullable<int> see, string detail, Nullable<int> wight, Nullable<int> hight, Nullable<int> age, string country, string image, Nullable<int> sum)
    {
        var idParameter = id != null ?
            new ObjectParameter("id", id) :
            new ObjectParameter("id", typeof(string));

        var regionParameter = region != null ?
            new ObjectParameter("region", region) :
            new ObjectParameter("region", typeof(string));

        var teamParameter = team != null ?
            new ObjectParameter("team", team) :
            new ObjectParameter("team", typeof(string));

        var datetimeParameter = datetime.HasValue ?
            new ObjectParameter("datetime", datetime) :
            new ObjectParameter("datetime", typeof(System.DateTime));

        var seqParameter = seq.HasValue ?
            new ObjectParameter("seq", seq) :
            new ObjectParameter("seq", typeof(int));

        var positionParameter = position != null ?
            new ObjectParameter("position", position) :
            new ObjectParameter("position", typeof(string));

        var nameParameter = name != null ?
            new ObjectParameter("name", name) :
            new ObjectParameter("name", typeof(string));

        var controlParameter = control != null ?
            new ObjectParameter("control", control) :
            new ObjectParameter("control", typeof(string));

        var attackParameter = attack != null ?
            new ObjectParameter("attack", attack) :
            new ObjectParameter("attack", typeof(string));

        var defenseParameter = defense != null ?
            new ObjectParameter("defense", defense) :
            new ObjectParameter("defense", typeof(string));

        var tacktickParameter = tacktick != null ?
            new ObjectParameter("tacktick", tacktick) :
            new ObjectParameter("tacktick", typeof(string));

        var physicalParameter = physical != null ?
            new ObjectParameter("physical", physical) :
            new ObjectParameter("physical", typeof(string));

        var mentalParameter = mental != null ?
            new ObjectParameter("mental", mental) :
            new ObjectParameter("mental", typeof(string));

        var adversaryParameter = adversary != null ?
            new ObjectParameter("adversary", adversary) :
            new ObjectParameter("adversary", typeof(string));

        var contactParameter = contact != null ?
            new ObjectParameter("contact", contact) :
            new ObjectParameter("contact", typeof(string));

        var hTParameter = hT != null ?
            new ObjectParameter("HT", hT) :
            new ObjectParameter("HT", typeof(string));

        var fTParameter = fT != null ?
            new ObjectParameter("FT", fT) :
            new ObjectParameter("FT", typeof(string));

        var competitionParameter = competition.HasValue ?
            new ObjectParameter("competition", competition) :
            new ObjectParameter("competition", typeof(int));

        var scoreParameter = score.HasValue ?
            new ObjectParameter("score", score) :
            new ObjectParameter("score", typeof(int));

        var seeParameter = see.HasValue ?
            new ObjectParameter("see", see) :
            new ObjectParameter("see", typeof(int));

        var detailParameter = detail != null ?
            new ObjectParameter("detail", detail) :
            new ObjectParameter("detail", typeof(string));

        var wightParameter = wight.HasValue ?
            new ObjectParameter("wight", wight) :
            new ObjectParameter("wight", typeof(int));

        var hightParameter = hight.HasValue ?
            new ObjectParameter("hight", hight) :
            new ObjectParameter("hight", typeof(int));

        var ageParameter = age.HasValue ?
            new ObjectParameter("age", age) :
            new ObjectParameter("age", typeof(int));

        var countryParameter = country != null ?
            new ObjectParameter("country", country) :
            new ObjectParameter("country", typeof(string));

        var imageParameter = image != null ?
            new ObjectParameter("image", image) :
            new ObjectParameter("image", typeof(string));

        var sumParameter = sum.HasValue ?
            new ObjectParameter("sum", sum) :
            new ObjectParameter("sum", typeof(int));

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_foxhun_playerupdate", idParameter, regionParameter, teamParameter, datetimeParameter, seqParameter, positionParameter, nameParameter, controlParameter, attackParameter, defenseParameter, tacktickParameter, physicalParameter, mentalParameter, adversaryParameter, contactParameter, hTParameter, fTParameter, competitionParameter, scoreParameter, seeParameter, detailParameter, wightParameter, hightParameter, ageParameter, countryParameter, imageParameter, sumParameter);
    }

    public virtual int usp_foxhun_regioninsert(string id, Nullable<int> seq, Nullable<System.DateTime> datetime, string name, string detail)
    {
        var idParameter = id != null ?
            new ObjectParameter("id", id) :
            new ObjectParameter("id", typeof(string));

        var seqParameter = seq.HasValue ?
            new ObjectParameter("seq", seq) :
            new ObjectParameter("seq", typeof(int));

        var datetimeParameter = datetime.HasValue ?
            new ObjectParameter("datetime", datetime) :
            new ObjectParameter("datetime", typeof(System.DateTime));

        var nameParameter = name != null ?
            new ObjectParameter("name", name) :
            new ObjectParameter("name", typeof(string));

        var detailParameter = detail != null ?
            new ObjectParameter("detail", detail) :
            new ObjectParameter("detail", typeof(string));

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_foxhun_regioninsert", idParameter, seqParameter, datetimeParameter, nameParameter, detailParameter);
    }

    public virtual ObjectResult<usp_foxhun_regionselectall_Result> usp_foxhun_regionselectall()
    {
        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_foxhun_regionselectall_Result>("usp_foxhun_regionselectall");
    }

    public virtual ObjectResult<usp_foxhun_regionselectbyid_Result> usp_foxhun_regionselectbyid(string region_id)
    {
        var region_idParameter = region_id != null ?
            new ObjectParameter("region_id", region_id) :
            new ObjectParameter("region_id", typeof(string));

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_foxhun_regionselectbyid_Result>("usp_foxhun_regionselectbyid", region_idParameter);
    }

    public virtual int usp_foxhun_teaminsert(string id, string region, Nullable<int> seq, Nullable<System.DateTime> create_datetime, Nullable<System.DateTime> update_date, string name, string detail, string email, string image, string username, string password, string contact, string contact1, string contact2, string phone, string phone1, string contact3, string file1, string file2, string file3, string file4, string file5, string file6, string file7, string file8, string file9, string file10, string contact4, string contact5, string contact6, string phone2, string phone3, string phone4, string phone5, string phone6)
    {
        var idParameter = id != null ?
            new ObjectParameter("id", id) :
            new ObjectParameter("id", typeof(string));

        var regionParameter = region != null ?
            new ObjectParameter("region", region) :
            new ObjectParameter("region", typeof(string));

        var seqParameter = seq.HasValue ?
            new ObjectParameter("seq", seq) :
            new ObjectParameter("seq", typeof(int));

        var create_datetimeParameter = create_datetime.HasValue ?
            new ObjectParameter("create_datetime", create_datetime) :
            new ObjectParameter("create_datetime", typeof(System.DateTime));

        var update_dateParameter = update_date.HasValue ?
            new ObjectParameter("update_date", update_date) :
            new ObjectParameter("update_date", typeof(System.DateTime));

        var nameParameter = name != null ?
            new ObjectParameter("name", name) :
            new ObjectParameter("name", typeof(string));

        var detailParameter = detail != null ?
            new ObjectParameter("detail", detail) :
            new ObjectParameter("detail", typeof(string));

        var emailParameter = email != null ?
            new ObjectParameter("email", email) :
            new ObjectParameter("email", typeof(string));

        var imageParameter = image != null ?
            new ObjectParameter("image", image) :
            new ObjectParameter("image", typeof(string));

        var usernameParameter = username != null ?
            new ObjectParameter("username", username) :
            new ObjectParameter("username", typeof(string));

        var passwordParameter = password != null ?
            new ObjectParameter("password", password) :
            new ObjectParameter("password", typeof(string));

        var contactParameter = contact != null ?
            new ObjectParameter("contact", contact) :
            new ObjectParameter("contact", typeof(string));

        var contact1Parameter = contact1 != null ?
            new ObjectParameter("contact1", contact1) :
            new ObjectParameter("contact1", typeof(string));

        var contact2Parameter = contact2 != null ?
            new ObjectParameter("contact2", contact2) :
            new ObjectParameter("contact2", typeof(string));

        var phoneParameter = phone != null ?
            new ObjectParameter("phone", phone) :
            new ObjectParameter("phone", typeof(string));

        var phone1Parameter = phone1 != null ?
            new ObjectParameter("phone1", phone1) :
            new ObjectParameter("phone1", typeof(string));

        var contact3Parameter = contact3 != null ?
            new ObjectParameter("contact3", contact3) :
            new ObjectParameter("contact3", typeof(string));

        var file1Parameter = file1 != null ?
            new ObjectParameter("file1", file1) :
            new ObjectParameter("file1", typeof(string));

        var file2Parameter = file2 != null ?
            new ObjectParameter("file2", file2) :
            new ObjectParameter("file2", typeof(string));

        var file3Parameter = file3 != null ?
            new ObjectParameter("file3", file3) :
            new ObjectParameter("file3", typeof(string));

        var file4Parameter = file4 != null ?
            new ObjectParameter("file4", file4) :
            new ObjectParameter("file4", typeof(string));

        var file5Parameter = file5 != null ?
            new ObjectParameter("file5", file5) :
            new ObjectParameter("file5", typeof(string));

        var file6Parameter = file6 != null ?
            new ObjectParameter("file6", file6) :
            new ObjectParameter("file6", typeof(string));

        var file7Parameter = file7 != null ?
            new ObjectParameter("file7", file7) :
            new ObjectParameter("file7", typeof(string));

        var file8Parameter = file8 != null ?
            new ObjectParameter("file8", file8) :
            new ObjectParameter("file8", typeof(string));

        var file9Parameter = file9 != null ?
            new ObjectParameter("file9", file9) :
            new ObjectParameter("file9", typeof(string));

        var file10Parameter = file10 != null ?
            new ObjectParameter("file10", file10) :
            new ObjectParameter("file10", typeof(string));

        var contact4Parameter = contact4 != null ?
            new ObjectParameter("contact4", contact4) :
            new ObjectParameter("contact4", typeof(string));

        var contact5Parameter = contact5 != null ?
            new ObjectParameter("contact5", contact5) :
            new ObjectParameter("contact5", typeof(string));

        var contact6Parameter = contact6 != null ?
            new ObjectParameter("contact6", contact6) :
            new ObjectParameter("contact6", typeof(string));

        var phone2Parameter = phone2 != null ?
            new ObjectParameter("phone2", phone2) :
            new ObjectParameter("phone2", typeof(string));

        var phone3Parameter = phone3 != null ?
            new ObjectParameter("phone3", phone3) :
            new ObjectParameter("phone3", typeof(string));

        var phone4Parameter = phone4 != null ?
            new ObjectParameter("phone4", phone4) :
            new ObjectParameter("phone4", typeof(string));

        var phone5Parameter = phone5 != null ?
            new ObjectParameter("phone5", phone5) :
            new ObjectParameter("phone5", typeof(string));

        var phone6Parameter = phone6 != null ?
            new ObjectParameter("phone6", phone6) :
            new ObjectParameter("phone6", typeof(string));

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_foxhun_teaminsert", idParameter, regionParameter, seqParameter, create_datetimeParameter, update_dateParameter, nameParameter, detailParameter, emailParameter, imageParameter, usernameParameter, passwordParameter, contactParameter, contact1Parameter, contact2Parameter, phoneParameter, phone1Parameter, contact3Parameter, file1Parameter, file2Parameter, file3Parameter, file4Parameter, file5Parameter, file6Parameter, file7Parameter, file8Parameter, file9Parameter, file10Parameter, contact4Parameter, contact5Parameter, contact6Parameter, phone2Parameter, phone3Parameter, phone4Parameter, phone5Parameter, phone6Parameter);
    }

    public virtual int usp_foxhun_teamselectall(string region_id)
    {
        var region_idParameter = region_id != null ?
            new ObjectParameter("region_id", region_id) :
            new ObjectParameter("region_id", typeof(string));

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_foxhun_teamselectall", region_idParameter);
    }

    public virtual int usp_foxhun_usersinsert(string id, Nullable<System.DateTime> createdate, string username, string password, string role, string team_id)
    {
        var idParameter = id != null ?
            new ObjectParameter("id", id) :
            new ObjectParameter("id", typeof(string));

        var createdateParameter = createdate.HasValue ?
            new ObjectParameter("createdate", createdate) :
            new ObjectParameter("createdate", typeof(System.DateTime));

        var usernameParameter = username != null ?
            new ObjectParameter("username", username) :
            new ObjectParameter("username", typeof(string));

        var passwordParameter = password != null ?
            new ObjectParameter("password", password) :
            new ObjectParameter("password", typeof(string));

        var roleParameter = role != null ?
            new ObjectParameter("role", role) :
            new ObjectParameter("role", typeof(string));

        var team_idParameter = team_id != null ?
            new ObjectParameter("team_id", team_id) :
            new ObjectParameter("team_id", typeof(string));

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_foxhun_usersinsert", idParameter, createdateParameter, usernameParameter, passwordParameter, roleParameter, team_idParameter);
    }
}
