using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EmployeeMongo.Models;

public class Employee
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement]
    public int EmployeeID { get; set; }

    [BsonElement]
    public string FirstName { get; set; }

    [BsonElement]
    public string LastName { get; set; }
}
