﻿using BO;
using DAL;
using ITVisions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ITVisions.EFCore;
using ITVisions.EFC;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFTools
{
 public class Program
 {


  private static string GetConnectionString()
  {

   // Wenn es einen Eintrag in mehr als einer Datei gibt, gewinnt der zuletzt hinzugefügte Eintrag

   var dic = new Dictionary<string, string> { { "ConnectionStrings:MiracleListDB", "Data Source=D120;Initial Catalog=MiracleList_Test;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" } };

   var builder = new ConfigurationBuilder() // NUGET: Microsoft.Extensions.Configuration
   .AddInMemoryCollection(dic)
   .AddJsonFile("appsettings.json") // NUGET: Microsoft.Extensions.Configuration.Json
   .AddEnvironmentVariables(); // NUGET: Microsoft.Extensions.Configuration.EnvironmentVariables

   IConfigurationRoot configuration = builder.Build();

   var cs = configuration["ConnectionStrings:MiracleListDB"];
   return cs;
  }

  public static void Main(string[] args)
  {

   //var m = new ITVisions.Mail.MailUtil();
   //m.SendEmail("test", "hs@IT-Visions.de", "HS", "hs@IT-Visions.de", "TEST BETREFF", "INHALT");


   //var m = new ITVisions.Mail.MailGun();

   //new ITVisions.NetworkUtil.MailUtil().SendMailTollerant("hs@IT-Visions.de", "hs@IT-Visions.de", "TEST BETREFF", "INHALT");
   PrintInfo("MiracleList Backend EFC Tools " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

   Context.ConnectionString = GetConnectionString();

   PrintInfo("Connection String:" + Context.ConnectionString);

   if (String.IsNullOrEmpty(Context.ConnectionString))
   {
    PrintError("No Connection String!");
    System.Environment.Exit(1);
   }

   if (args.Count() == 0)
   {
    CUI.PrintError("Missing Parameter: migrate | createtestuser | recreate");
    System.Environment.Exit(1);
   }

   for (int i = 0; i < args.Length; i++)
   {
    args[i] = args[i].ToLower().Replace("-", "").Replace("/", "");
   }

   if (args.Contains("recreate")) Recreate();
   if (args.Contains("migrate")) Migrate();
   if (args.Contains("createtestuser")) CreateTestUser();


   //var ctx = new Context();
   //var sts = ctx.Set<SubTask>().Where(st=>st.Done==true).ToList();
   //Console.WriteLine(sts.Count);


   CUI.PrintSuccess("DONE!");
   System.Environment.Exit(0);
   //Console.ReadLine();

  }


  private static void Recreate()
  {
   CUI.H1("(Re-)Creating Database...");
   try
   {
    var ctx = new Context();

    if ((ctx.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
    {
     Console.WriteLine("Deleting database...");
     ctx.Database.EnsureDeleted();
    }

    Migrate();

   }
   catch (Exception ex)
   {
    PrintError("Migration Error", ex);
    System.Environment.Exit(2);
   }

  }


  private static void Migrate()
  {
   CUI.H1("Migrate Database...");

   try
   {
    var ctx = new Context();

    PrintMigrationStatus(ctx);
    ctx.Database.Migrate();

   }
   catch (Exception ex)
   {
    PrintError("Migration Error", ex);
    System.Environment.Exit(2);
   }
  }


  private static void PrintMigrationStatus(DbContext ctx)
  {
   CUI.H2("Getting Migration Status");
   try
   {
    Dictionary<string, string> migrationsStatus = new Dictionary<string, string>();
    var migrations = ctx.Database.GetMigrationStatus();

    foreach (var item in migrations)
    {
     if (item.Value) CUI.Print(item.Key + ":" + " Applied", ConsoleColor.Green);
     else CUI.Print(item.Key + ":" + " TODO", ConsoleColor.Red);
    }
   }
   catch (Exception)
   {
    CUI.PrintError("Database not available!");
   }

  }
  private static void CreateTestUser()
  {
   CUI.H1("Creating test users...");
   try
   {
    var zeit = DateTime.Now.ToString();
    var guid = new Guid("11111111-1111-1111-1111-111111111111");

    var clm = new BL.ClientManager();
    if (clm.GetByID(guid) == null)
    {
     var c = new Client();
     c.Name = "test";
     c.Company = "test";
     c.EMail = "test";
     c.Created = DateTime.Now;
     c.ClientID = guid;
     c.Type = "Test";
     clm.New(c);
     CUI.PrintSuccess($"Client {guid} angelegt!");
    }
    else
    {
     CUI.PrintSuccess($"Client {guid} vorhanden!");
    }

    var um = new BL.UserManager("test", "test", "test");

    um.InitDefaultTasks();
    var cm = new BL.CategoryManager(um.CurrentUser.UserID);
    var cs = cm.GetCategorySet();

    PrintInfo(cs.Count + " Tasks for User ID=" + um.CurrentUser.UserID + " (" + um.CurrentUser.UserName + ") Token=" + um.CurrentUser.Token);
    if (cs.Count != 4)
    {
     PrintError("Data Test Error: Count=" + cs.Count);
     System.Environment.Exit(3);
    }
   }
   catch (Exception ex)
   {
    PrintError("Data Test Error", ex);
    System.Environment.Exit(4);
    throw;
   }
  }
  public static void PrintInfo(string s)
  {
   // VSO Logging Commands https://github.com/Microsoft/vsts-tasks/blob/master/docs/authoring/commands.md
   //s = s += "##vso[task.logdetail]" + s;
   CUI.Print(s);
  }

  public static void PrintError(string s, Exception ex = null)
  {
   // VSO Logging Commands https://github.com/Microsoft/vsts-tasks/blob/master/docs/authoring/commands.md
   s = s += "##vso[task.logissue type=error;]" + s + (ex != null ? ": " + ex.Message : "");
   CUI.PrintError(s);
  }

 }
}
