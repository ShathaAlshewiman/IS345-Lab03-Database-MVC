using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using Npgsql;


namespace WebApplication3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    
    //insert Example
    public IActionResult Insertphone()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Insertphone(string na, int nu)
    {
        // Connection string for PostgreSQL
        /*string connectionString = "Host=localhost;Port=5432;Database=LabsDB;Username=postgres;Password=shatha1425";
        NpgsqlConnection conn = new NpgsqlConnection(connectionString);
        NpgsqlCommand comm = new NpgsqlCommand("insert into phones (username,usernumber)  values  ('"+ na +"' ,'"+ nu +"')", conn);*/
        string sql = "insert into phones (username,usernumber)  values  ('"+ na +"' ,'"+ nu +"')";
        var builder = WebApplication.CreateBuilder();
        string constr = builder.Configuration.GetConnectionString("WebApplication3");
        NpgsqlConnection conn = new NpgsqlConnection(constr);
        NpgsqlCommand comm  = new NpgsqlCommand(sql, conn);
        
        conn.Open();
        comm.ExecuteNonQuery();
        conn.Close();
        ViewData["message"] = "Successfully added";

        return View();
    }
    
    public IActionResult registration()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult registration([Bind("name,age,job,married,gender")] client cli)
    {
        var builder = WebApplication.CreateBuilder();
        string constr = builder.Configuration.GetConnectionString("WebApplication3");
        NpgsqlConnection conn = new NpgsqlConnection(constr);
        
        conn.Open();
        string sql;
        sql = "select * from client  where name = '" + cli.name + "'";
        NpgsqlCommand comm  = new NpgsqlCommand(sql, conn);
        NpgsqlDataReader reader = comm.ExecuteReader();
        if (reader.Read())
        {
            ViewData["message"] = "name already exists";
            reader.Close();
        }
        else
        {
            reader.Close();        
            sql = "insert into client (name,age,job,gender,married) values ('" + cli.name + "','" + cli.age + "','" + cli.job + "','" 
                  + cli.gender + "','" + cli.married + "')";
            comm = new NpgsqlCommand(sql, conn);
            comm.ExecuteNonQuery();
            ViewData["message"] = "Sucessfully added";
        }
        conn.Close();
        return View();
    }

    public IActionResult Edit(int? id)
    {
        var builder = WebApplication.CreateBuilder();
        string constr = builder.Configuration.GetConnectionString("WebApplication3");
        NpgsqlConnection conn = new NpgsqlConnection(constr);
        
        client cli = new client();
        
        string sql = "";
        sql = "select * from client  where id = '" + id + "'";
        NpgsqlCommand comm  = new NpgsqlCommand(sql, conn);
        conn.Open();
        NpgsqlDataReader reader = comm.ExecuteReader();
        if (reader.Read())
        {
            cli.id = (int)reader["Id"];
            cli.name = (string)reader["name"];
            cli.age = (int)reader["age"];
            cli.job = (string)reader["job"];
            cli.gender = (string)reader["gender"];
            cli.married = (bool)reader["married"];
        }
        reader.Close();
        return View(cli);
    }

    [HttpPost]
    public IActionResult Edit([Bind("name,age,job,gender,married")] client cli)
    {
        var builder = WebApplication.CreateBuilder();
        string constr = builder.Configuration.GetConnectionString("WebApplication3");
        NpgsqlConnection conn = new NpgsqlConnection(constr);

        string sql = "";
        sql = "update client set name = '" + cli.name + "' , age = '"+cli.age+"' , job = '"+cli.job+"' , gender = '"
              +cli.gender+"' , married = '"+cli.married+"'where Id = '" + cli.id + "'";
        NpgsqlCommand comm  = new NpgsqlCommand(sql, conn);
        conn.Open();
        comm.ExecuteNonQuery();
        conn.Close();
        ViewData["Message"] = "Sucessfully Edited";
        return View();
    }
    
    public IActionResult GetClient()
    {
        List<client> li = new List<client>();
        var builder = WebApplication.CreateBuilder();
        string constr = builder.Configuration.GetConnectionString("WebApplication3");
        NpgsqlConnection conn = new NpgsqlConnection(constr);
        
        string sql;
        sql = "select * from client ";
        NpgsqlCommand comm  = new NpgsqlCommand(sql, conn);
        
        conn.Open();

        NpgsqlDataReader reader = comm.ExecuteReader();

        while (reader.Read())
        {
            li.Add(new client
            {
                name = (string)reader["name"],
                age = (int)reader["age"],
                job = (string)reader["job"],
                id = (int)reader["Id"]
            });
        }
        reader.Close();
        conn.Close();
        return View(li);
    }



    
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}