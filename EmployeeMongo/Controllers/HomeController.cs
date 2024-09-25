using EmployeeMongo.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EmployeeMongo.Controllers;
public class HomeController : Controller
{
    private IMongoCollection<Employee> collection;

    public HomeController()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        IMongoDatabase db = client.GetDatabase("FirstDatabase");
        this.collection = db.GetCollection<Employee>("Employees");
    }

    public IActionResult Index()
    {
        var model = collection.Find
        (FilterDefinition<Employee>.Empty).ToList();

        return View(model);
    }

    public IActionResult Insert()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Insert(Employee emp)
    {
        collection.InsertOne(emp);
        ViewBag.Message = "Employee added successfully!";
        return View();
    }

    public IActionResult Update(string id)
    {
        ObjectId oId = new ObjectId(id);
        Employee emp = collection.Find(e => e.Id == oId).FirstOrDefault();

        return View(emp);
    }

    [HttpPost]
    public IActionResult Update(string id, Employee emp)
    {
        emp.Id = new ObjectId(id);
        var filter = Builders<Employee>.Filter.Eq("Id", emp.Id);
        var updateDef = Builders<Employee>.Update.Set("FirstName", emp.FirstName);
        updateDef = updateDef.Set("LastName", emp.LastName);
        var result = collection.UpdateOne(filter, updateDef);

        if (result.IsAcknowledged)
        {
            ViewBag.Message = "Employee updated successfully!";
        }
        else
        {
            ViewBag.Message = "Error while updating Employee!";
        }
        return View(emp);
    }

    public IActionResult ConfirmDelete(string id)
    {
        ObjectId oId = new ObjectId(id);
        Employee emp = collection.Find(e => e.Id == oId).FirstOrDefault();
        return View(emp);
    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        ObjectId oId = new ObjectId(id);
        var result = collection.DeleteOne<Employee>(e => e.Id == oId);

        if (result.IsAcknowledged)
        {
            TempData["Message"] = "Employee deleted successfully!";
        }
        else
        {
            TempData["Message"] = "Error while deleting Employee!";
        }
        return RedirectToAction("Index");
    }
}
