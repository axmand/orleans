using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrasturcture.DB.Entity
{
    public class DBContext
    {
        IMongoClient client;

        IMongoDatabase db;

        const string connectStr = "mongodb://localhost:27017";

        public DBContext()
        {
            client = new MongoClient(connectStr);
            db = client.GetDatabase("");
        }
    }
}
